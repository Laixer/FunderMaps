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
    ///     Recovery sample repository.
    /// </summary>
    internal class RecoverySampleRepository : RepositoryBase<RecoverySample, int>, IRecoverySampleRepository
    {
        public static void MapToWriter(DbContext context, RecoverySample entity)
        {
            context.AddParameterWithValue("recovery", entity.Recovery);
            context.AddParameterWithValue("address", entity.Address);
            context.AddParameterWithValue("note", entity.Note);
            context.AddParameterWithValue("status", entity.Status);
            context.AddParameterWithValue("type", entity.Type);
            context.AddParameterWithValue("pile_type", entity.PileType);
            context.AddParameterWithValue("contractor", entity.Contractor);
            context.AddParameterWithValue("facade", entity.Facade);
            context.AddParameterWithValue("permit", entity.Permit);
            context.AddParameterWithValue("permit_date", entity.PermitDate);
            context.AddParameterWithValue("recovery_date", entity.RecoveryDate);
        }

        public static RecoverySample MapFromReader(DbDataReader reader, bool fullMap = false, int offset = 0)
            => new RecoverySample
            {
                Id = reader.GetInt(offset + 0),
                Recovery = reader.GetInt(offset + 1),
                Address = reader.GetSafeString(offset + 2),
                CreateDate = reader.GetDateTime(offset + 3),
                UpdateDate = reader.GetSafeDateTime(offset + 4),
                DeleteDate = reader.GetSafeDateTime(offset + 5),
                Note = reader.GetSafeString(offset + 6),
                Status = reader.GetFieldValue<RecoveryStatus>(offset + 7),
                Type = reader.GetFieldValue<RecoveryType>(offset + 8),
                PileType = reader.GetFieldValue<PileType>(offset + 9),
                Contractor = reader.GetFieldValue<Guid?>(offset + 10),
                Facade = reader.GetFieldValue<Facade[]>(offset + 11),
                Permit = reader.GetSafeString(offset + 12),
                PermitDate = reader.GetDateTime(offset + 13),
                RecoveryDate = reader.GetDateTime(offset + 14),
            };

        /// <summary>
        ///     Create new <see cref="RecoverySample"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        /// <returns>Created <see cref="RecoverySample"/>.</returns>
        public override async ValueTask<int> AddAsync(RecoverySample entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var sql = @"
                INSERT INTO report.recovery_sample(
                    recovery,
                    address,
                    note,
                    status,
                    type,
                    pile_type,
                    contractor,
                    facade,
                    permit,
                    permit_date,
                    recovery_date)
                VALUES (
                    @recovery,
                    @address,
                    @note,
                    @status,
                    @type,
                    @pile_type,
                    @contractor,
                    @facade,
                    @permit,
                    @permit_date,
                    @recovery_date)
                RETURNING id;
            ";

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
                FROM    report.recovery_sample";

            await using var context = await DbContextFactory(sql);

            return await context.ScalarAsync<ulong>();
        }

        /// <summary>
        ///     Delete <see cref="RecoverySample"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        public override async ValueTask DeleteAsync(int id)
        {
            var sql = @"
                DELETE
                FROM    report.recovery_sample
                WHERE   id = @id";

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("id", id);

            await context.NonQueryAsync();
        }

        /// <summary>
        ///     Retrieve <see cref="RecoverySample"/> by id.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <returns><see cref="RecoverySample"/>.</returns>
        public override async ValueTask<RecoverySample> GetByIdAsync(int id)
        {
            var sql = @"
                SELECT  id,
                        recovery,
                        address,
                        create_date,
                        update_date,
                        delete_date,
                        note,
                        status,
                        type,
                        pile_type,
                        contractor,
                        facade,
                        permit,
                        permit_date,
                        recovery_date
                FROM    report.recovery_sample
                WHERE   id = @id
                LIMIT   1";

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("id", id);

            await using var reader = await context.ReaderAsync();

            return MapFromReader(reader);
        }

        /// <summary>
        ///     Retrieve all <see cref="RecoverySample"/>.
        /// </summary>
        /// <returns>List of <see cref="RecoverySample"/>.</returns>
        public override async IAsyncEnumerable<RecoverySample> ListAllAsync(INavigation navigation)
        {
            if (navigation == null)
            {
                throw new ArgumentNullException(nameof(navigation));
            }

            var sql = @"
                SELECT  id,
                        recovery,
                        address,
                        create_date,
                        update_date,
                        delete_date,
                        note,
                        status,
                        type,
                        pile_type,
                        contractor,
                        facade,
                        permit,
                        permit_date,
                        recovery_date
                FROM    report.recovery_sample";

            ConstructNavigation(ref sql, navigation);

            await using var context = await DbContextFactory(sql);

            await foreach (var reader in context.EnumerableReaderAsync())
            {
                yield return MapFromReader(reader);
            }
        }

        public override async ValueTask UpdateAsync(RecoverySample entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var sql = @"
                    UPDATE  report.inquiry_sample
                    SET     recovery = @recovery,
                            address = @address,
                            note = @note,
                            status = @status,
                            type = @type,
                            pile_type = @pile_type,
                            contractor = @contractor,
                            facade = @facade,
                            permit = @permit,
                            permit_date = @permit_date,
                            recovery_date = @recovery_date,
                    WHERE   id = @id";

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("id", entity.Id);

            MapToWriter(context, entity);

            await context.NonQueryAsync();
        }
    }
}
