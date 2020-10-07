using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using FunderMaps.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace FunderMaps.Data.Repositories
{
    /// <summary>
    ///     Inquiry sample repository.
    /// </summary>
    internal class InquirySampleRepository : RepositoryBase<InquirySample, int>, IInquirySampleRepository
    {
        /// <summary>
        ///     Create new <see cref="InquirySample"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        /// <returns>Created <see cref="InquirySample"/>.</returns>
        public override async ValueTask<int> AddAsync(InquirySample entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var sql = @"
                INSERT INTO report.inquiry_sample(
                    inquiry,
                    address,
                    note,
                    built_year,
                    substructure)
                VALUES (
                    @inquiry,
                    @address,
                    @note,
                    @built_year,
                    @substructure)
                RETURNING id";

            await using var context = await DbContextFactory(sql);

            MapToWriter(context, entity);

            return await context.ScalarAsync<int>();
        }

        /// <summary>
        ///     Retrieve number of entities.
        /// </summary>
        /// <returns>Number of entities.</returns>
        public override async ValueTask<ulong> CountAsync()
        {
            var sql = @"
                SELECT  COUNT(*)
                FROM    report.inquiry_sample AS s
                JOIN 	report.inquiry AS i ON i.id = s.inquiry
                JOIN 	application.attribution AS a ON a.id = i.attribution
                WHERE   a.owner = @tenant";

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("tenant", AppContext.TenantId);

            return await context.ScalarAsync<ulong>();
        }

        /// <summary>
        ///     Delete <see cref="InquirySample"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        public override async ValueTask DeleteAsync(int id)
        {
            var sql = @"
                DELETE
                FROM    report.inquiry_sample AS s
                JOIN 	report.inquiry AS i ON i.id = s.inquiry
                JOIN 	application.attribution AS a ON a.id = i.attribution
                WHERE   s.id = @id
                AND     a.owner = @tenant";

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("id", id);
            context.AddParameterWithValue("tenant", AppContext.TenantId);

            await context.NonQueryAsync();
        }

        private static void MapToWriter(DbContext context, InquirySample entity)
        {
            context.AddParameterWithValue("inquiry", entity.Inquiry);
            context.AddParameterWithValue("address", entity.Address);
            context.AddParameterWithValue("note", entity.Note);
            context.AddParameterWithValue("built_year", entity.BuiltYear);
            context.AddParameterWithValue("substructure", entity.Substructure);
        }

        private static InquirySample MapFromReader(DbDataReader reader, bool fullMap = false, int offset = 0)
            => new InquirySample
            {
                Id = reader.GetInt(offset + 0),
                Inquiry = reader.GetInt(offset + 1),
                Address = reader.GetSafeString(offset + 2),
                Note = reader.GetSafeString(offset + 3),
                CreateDate = reader.GetDateTime(offset + 4),
                UpdateDate = reader.GetSafeDateTime(offset + 5),
                DeleteDate = reader.GetSafeDateTime(offset + 6),
                BaseMeasurementLevel = reader.GetFieldValue<BaseMeasurementLevel>(offset + 7),
                BuiltYear = reader.GetDateTime(offset + 8),
                Substructure = reader.GetFieldValue<Substructure>(offset + 9),
            };

        /// <summary>
        ///     Retrieve <see cref="InquirySample"/> by id.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <returns><see cref="InquirySample"/>.</returns>
        public override async ValueTask<InquirySample> GetByIdAsync(int id)
        {
            var sql = @"
                SELECT  -- InquirySample
                        s.id,
                        s.inquiry,
                        s.address,
                        s.note,
                        s.create_date,
                        s.update_date,
                        s.delete_date,
                        s.base_measurement_level,
                        s.built_year,
                        s.substructure
                FROM    report.inquiry_sample AS s
                JOIN 	report.inquiry AS i ON i.id = s.inquiry
                JOIN 	application.attribution AS a ON a.id = i.attribution
                WHERE   s.id = @id
                AND     a.owner = @tenant
                LIMIT   1";

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("id", id);
            context.AddParameterWithValue("tenant", AppContext.TenantId);

            await using var reader = await context.ReaderAsync();

            return MapFromReader(reader);
        }

        public Task<InquirySample> GetPublicAndByIdAsync(int id, Guid orgId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Retrieve all <see cref="InquirySample"/>.
        /// </summary>
        /// <returns>List of <see cref="InquirySample"/>.</returns>
        public override async IAsyncEnumerable<InquirySample> ListAllAsync(INavigation navigation)
        {
            if (navigation == null)
            {
                throw new ArgumentNullException(nameof(navigation));
            }

            var sql = @"
                SELECT  -- InquirySample
                        s.id,
                        s.inquiry,
                        s.address,
                        s.note,
                        s.create_date,
                        s.update_date,
                        s.delete_date,
                        s.base_measurement_level,
                        s.built_year,
                        s.substructure
                FROM    report.inquiry_sample AS s
                JOIN 	report.inquiry AS i ON i.id = s.inquiry
                JOIN 	application.attribution AS a ON a.id = i.attribution
                WHERE   a.owner = @tenant";

            ConstructNavigation(ref sql, navigation);

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("tenant", AppContext.TenantId);

            await foreach (var reader in context.EnumerableReaderAsync())
            {
                yield return MapFromReader(reader);
            }
        }

        public Task<IReadOnlyList<InquirySample>> ListAllReportAsync(int report, INavigation navigation)
        {
            throw new NotImplementedException();
        }

        public override async ValueTask UpdateAsync(InquirySample entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var sql = @"
                    UPDATE  report.inquiry_sample AS s
                    SET     inquiry = @inquiry,
                            address = @address,
                            note = @note,
                            built_year = @built_year,
                            substructure = @substructure
                    FROM 	application.attribution AS a, report.inquiry AS i
                    WHERE   i.id = s.inquiry
                    AND     a.id = i.attribution
                    AND     s.id = @id
                    AND     a.owner = @tenant";

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("id", entity.Id);
            context.AddParameterWithValue("tenant", AppContext.TenantId);

            MapToWriter(context, entity);

            await context.NonQueryAsync();
        }
    }
}
