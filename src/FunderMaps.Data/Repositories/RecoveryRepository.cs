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
    ///     Recovery repository.
    /// </summary>
    internal class RecoveryRepository : RepositoryBase<Recovery, int>, IRecoveryRepository
    {
        public static void MapToWriter(DbContext context, Recovery entity)
        {
            context.AddParameterWithValue("note", entity.Note);
            context.AddParameterWithValue("attribution", entity.Attribution);
            context.AddParameterWithValue("access_policy", entity.AccessPolicy);
            context.AddParameterWithValue("type", entity.Type);
            context.AddParameterWithValue("document_date", entity.DocumentDate);
            context.AddParameterWithValue("document_file", entity.DocumentFile);
        }

        public static Recovery MapFromReader(DbDataReader reader, bool fullMap = false, int offset = 0)
            => new Recovery
            {
                Id = reader.GetInt(offset + 0),
                Note = reader.GetSafeString(offset + 1),
                CreateDate = reader.GetDateTime(offset + 2),
                UpdateDate = reader.GetSafeDateTime(offset + 3),
                DeleteDate = reader.GetSafeDateTime(offset + 4),
                Attribution = reader.GetInt(offset + 5),
                AccessPolicy = reader.GetFieldValue<AccessPolicy>(offset + 6),
                Type = reader.GetFieldValue<RecoveryDocumentType>(offset + 7),
                DocumentDate = reader.GetDateTime(offset + 8),
                DocumentFile = reader.GetSafeString(offset + 9),
            };

        /// <summary>
        ///     Create new <see cref="Recovery"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        /// <returns>Created <see cref="Recovery"/>.</returns>
        public override async ValueTask<int> AddAsync(Recovery entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var sql = @"
                INSERT INTO report.recovery(
                    note,
                    attribution,
                    access_policy,
                    type,
                    document_date,
                    document_file)
                VALUES (
                    @note,
                    @attribution,
                    @access_policy,
                    @type,
                    @document_date,
                    @document_file)
                RETURNING id";

            await using var context = await DbContextFactory(sql);

            MapToWriter(context, entity);

            return await context.ScalarAsync<int>();
        }

        /// <summary>
        ///     Retrieve number of entities.
        /// </summary>
        /// <returns>Number of entities.</returns>
        public override async ValueTask<long> CountAsync()
        {
            var sql = @"
                SELECT  COUNT(*)
                FROM    report.recovery";

            await using var context = await DbContextFactory(sql);

            return await context.ScalarAsync<long>();
        }

        /// <summary>
        ///     Delete <see cref="Recovery"/>.
        /// </summary>
        /// <param name="id">Entity id.</param>
        public override async ValueTask DeleteAsync(int id)
        {
            var sql = @"
                DELETE
                FROM    report.recovery
                WHERE   id = @id";

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("id", id);

            await context.NonQueryAsync();
        }

        /// <summary>
        ///     Retrieve <see cref="Recovery"/> by id.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <returns><see cref="Recovery"/>.</returns>
        public override async ValueTask<Recovery> GetByIdAsync(int id)
        {
            var sql = @"
                SELECT  -- Recovery
                        r.id,
                        r.note,
                        r.create_date,
                        r.update_date,
                        r.delete_date,
                        r.attribution,
                        r.access_policy,
                        r.type,
                        r.document_date,
                        r.document_file
                FROM    report.recovery AS r
                WHERE   r.id = @id
                LIMIT   1";

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("id", id);

            await using var reader = await context.ReaderAsync();

            return MapFromReader(reader);
        }

        /// <summary>
        ///     Retrieve all <see cref="Recovery"/>.
        /// </summary>
        /// <returns>List of <see cref="Recovery"/>.</returns>
        public override async IAsyncEnumerable<Recovery> ListAllAsync(INavigation navigation)
        {
            if (navigation == null)
            {
                throw new ArgumentNullException(nameof(navigation));
            }

            var sql = @"
                SELECT  -- Recovery
                        r.id,
                        r.note,
                        r.create_date,
                        r.update_date,
                        r.delete_date,
                        r.attribution,
                        r.access_policy,
                        r.type,
                        r.document_date,
                        r.document_file
                FROM    report.recovery AS r";

            ConstructNavigation(ref sql, navigation, "r");

            await using var context = await DbContextFactory(sql);

            await foreach (var reader in context.EnumerableReaderAsync())
            {
                yield return MapFromReader(reader);
            }
        }

        /// <summary>
        ///     Update <see cref="Recovery"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        public override async ValueTask UpdateAsync(Recovery entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var sql = @"
                    UPDATE  report.recovery
                    SET     note = @note,
                            access_policy = @access_policy,
                            type = @type,
                            document_date = @document_date,
                            document_file = @document_file
                    WHERE   id = @id";

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("id", entity.Id);

            MapToWriter(context, entity);

            await context.NonQueryAsync();
        }
    }
}
