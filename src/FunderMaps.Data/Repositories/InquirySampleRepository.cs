using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using FunderMaps.Data.Extensions;
using FunderMaps.Data.Providers;
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
        ///     Create a new instance.
        /// </summary>
        /// <param name="dbProvider">Database provider.</param>
        public InquirySampleRepository(DbProvider dbProvider)
            : base(dbProvider)
        {
        }

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
                    base_measurement_level,
                    built_year,
                    substructure)
                VALUES (
                    @inquiry,
                    @address,
                    @note,
                    @base_measurement_level,
                    @built_year,
                    @substructure)
                RETURNING id;
            ";

            await using var connection = await DbProvider.OpenConnectionScopeAsync();
            await using var cmd = DbProvider.CreateCommand(sql, connection);

            MapToWriter(cmd, entity);

            return await cmd.ExecuteScalarIntAsync(); // TODO: Ensure
        }

        /// <summary>
        ///     Retrieve number of entities.
        /// </summary>
        /// <returns>Number of entities.</returns>
        public override ValueTask<ulong> CountAsync()
        {
            var sql = @"
                SELECT  COUNT(*)
                FROM    report.inquiry_sample";

            return ExecuteScalarUnsignedLongCommandAsync(sql); // TODO: Ensure
        }

        public Task<uint> CountAsync(Guid orgId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Delete <see cref="InquirySample"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        public override async ValueTask DeleteAsync(int id)
        {
            var sql = @"
                DELETE
                FROM    report.inquiry_sample
                WHERE   id = @id";

            await using var connection = await DbProvider.OpenConnectionScopeAsync();
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("id", id);
            await cmd.ExecuteNonQueryEnsureAffectedAsync();
        }

        private static void MapToWriter(DbCommand cmd, InquirySample entity)
        {
            cmd.AddParameterWithValue("inquiry", entity.Inquiry);
            cmd.AddParameterWithValue("address", entity.Address);
            cmd.AddParameterWithValue("note", entity.Note);
            cmd.AddParameterWithValue("base_measurement_level", entity.BaseMeasurementLevel);
            cmd.AddParameterWithValue("built_year", entity.BuiltYear);
            cmd.AddParameterWithValue("substructure", entity.Substructure);
        }

        private static InquirySample MapFromReader(DbDataReader reader)
            => new InquirySample
            {
                Id = reader.GetInt(0),
                Inquiry = reader.GetInt(1),
                Address = reader.GetSafeString(2),
                Note = reader.GetSafeString(3),
                CreateDate = reader.GetDateTime(4),
                UpdateDate = reader.GetSafeDateTime(5),
                DeleteDate = reader.GetSafeDateTime(6),
                BaseMeasurementLevel = reader.GetFieldValue<BaseMeasurementLevel>(7),
                BuiltYear = reader.GetDateTime(8),
                Substructure = reader.GetFieldValue<Substructure>(9),
            };

        /// <summary>
        ///     Retrieve <see cref="InquirySample"/> by id.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <returns><see cref="InquirySample"/>.</returns>
        public override async ValueTask<InquirySample> GetByIdAsync(int id)
        {
            var sql = @"
                SELECT  id,
                        inquiry,
                        address,
                        note,
                        create_date,
                        update_date,
                        delete_date,
                        base_measurement_level,
                        built_year,
                        substructure
                FROM    report.inquiry_sample
                WHERE   id = @id
                LIMIT   1";

            await using var connection = await DbProvider.OpenConnectionScopeAsync();
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("id", id);

            await using var reader = await cmd.ExecuteReaderAsyncEnsureRowAsync();
            await reader.ReadAsync();

            return MapFromReader(reader);
        }

        public Task<InquirySample> GetByIdAsync(int id, Guid orgId)
        {
            throw new NotImplementedException();
        }

        public Task<InquirySample> GetPublicAndByIdAsync(int id, Guid orgId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieve all <see cref="InquirySample"/>.
        /// </summary>
        /// <returns>List of <see cref="InquirySample"/>.</returns>
        public override async IAsyncEnumerable<InquirySample> ListAllAsync(INavigation navigation)
        {
            if (navigation == null)
            {
                throw new ArgumentNullException(nameof(navigation));
            }

            var sql = @"
                SELECT  id,
                        inquiry,
                        address,
                        note,
                        create_date,
                        update_date,
                        delete_date,
                        base_measurement_level,
                        built_year,
                        substructure
                FROM    report.inquiry_sample";

            ConstructNavigation(ref sql, navigation);

            await using var connection = await DbProvider.OpenConnectionScopeAsync();
            await using var cmd = DbProvider.CreateCommand(sql, connection);

            await using var reader = await cmd.ExecuteReaderAsyncEnsureRowAsync();
            while (await reader.ReadAsync())
            {
                yield return MapFromReader(reader);
            }
        }

        public Task<IReadOnlyList<InquirySample>> ListAllAsync(Guid orgId, INavigation navigation)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<InquirySample>> ListAllReportAsync(int report, INavigation navigation)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<InquirySample>> ListAllReportAsync(int report, Guid orgId, INavigation navigation)
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
                    UPDATE  report.inquiry_sample
                    SET     inquiry = @inquiry,
                            address = @address,
                            note = @note,
                            base_measurement_level = @base_measurement_level,
                            built_year = @built_year,
                            substructure = @substructure
                    WHERE   id = @id";

            using var connection = await DbProvider.OpenConnectionScopeAsync();
            using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("id", entity.Id);

            MapToWriter(cmd, entity);

            await cmd.ExecuteNonQueryEnsureAffectedAsync();
        }
    }
}
