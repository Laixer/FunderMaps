using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using FunderMaps.Data.Extensions;
using FunderMaps.Data.Providers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Data.Repositories
{
    /// <summary>
    /// Recovery repository.
    /// </summary>
    internal class RecoveryRepository : RepositoryBase<Recovery, int>, IRecoveryRepository
    {
        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="dbProvider">Database provider.</param>
        public RecoveryRepository(DbProvider dbProvider) : base(dbProvider) { }

        /// <summary>
        /// Create new <see cref="Recovery"/>.
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
                RETURNING id;
            ";

            await using var connection = await DbProvider.OpenConnectionScopeAsync().ConfigureAwait(false);
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("note", entity.Note);
            cmd.AddParameterWithValue("attribution", entity.Attribution);
            cmd.AddParameterWithValue("access_policy", entity.AccessPolicy);
            cmd.AddParameterWithValue("type", entity.Type);
            cmd.AddParameterWithValue("document_date", entity.DocumentDate);
            cmd.AddParameterWithValue("document_file", entity.DocumentFile);
            return await cmd.ExecuteScalarIntAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieve number of entities.
        /// </summary>
        /// <returns>Number of entities.</returns>
        public override ValueTask<ulong> CountAsync()
        {
            var sql = @"
                SELECT  COUNT(*)
                FROM    report.recovery";

            return ExecuteScalarUnsignedLongCommandAsync(sql);
        }

        /// <summary>
        /// Delete <see cref="Recovery"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        public override async ValueTask DeleteAsync(int id)
        {
            var sql = @"
                DELETE
                FROM    report.recovery
                WHERE   id = @id";

            await using var connection = await DbProvider.OpenConnectionScopeAsync().ConfigureAwait(false);
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("id", id);
            await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieve <see cref="Recovery"/> by id.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <returns><see cref="Recovery"/>.</returns>
        public override async ValueTask<Recovery> GetByIdAsync(int id)
        {
            var sql = @"
                SELECT  id,
                        note,
                        create_date,
                        update_date,
                        delete_date,
                        attribution,
                        access_policy,
                        type,
                        document_date,
                        document_file
                FROM    report.recovery
                WHERE   id = @id
                LIMIT   1";

            await using var connection = await DbProvider.OpenConnectionScopeAsync().ConfigureAwait(false);
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("id", id);

            await using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
            await reader.ReadAsync().ConfigureAwait(false);

            return new Recovery
            {
                Id = reader.GetInt(0),
                Note = reader.SafeGetString(1),
                CreateDate = reader.GetDateTime(2),
                UpdateDate = reader.GetSafeDateTime(3),
                DeleteDate = reader.GetSafeDateTime(4),
                Attribution = reader.GetInt(5),
                AccessPolicy = reader.GetFieldValue<AccessPolicy>(6),
                Type = reader.GetFieldValue<RecoveryDocumentType>(7),
                DocumentDate = reader.GetDateTime(8),
                DocumentFile = reader.SafeGetString(9),
            };
        }

        /// <summary>
        /// Retrieve all <see cref="Recovery"/>.
        /// </summary>
        /// <returns>List of <see cref="Recovery"/>.</returns>
        public override async IAsyncEnumerable<Recovery> ListAllAsync(INavigation navigation)
        {
            if (navigation == null)
            {
                throw new ArgumentNullException(nameof(navigation));
            }

            var sql = @"
                SELECT  id,
                        note,
                        create_date,
                        update_date,
                        delete_date,
                        attribution,
                        access_policy,
                        type,
                        document_date,
                        document_file
                FROM    report.recovery";

            ConstructNavigation(ref sql, navigation);

            await using var connection = await DbProvider.OpenConnectionScopeAsync().ConfigureAwait(false);
            await using var cmd = DbProvider.CreateCommand(sql, connection);

            await using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
            while (await reader.ReadAsync().ConfigureAwait(false))
            {
                yield return new Recovery
                {
                    Id = reader.GetInt(0),
                    Note = reader.SafeGetString(1),
                    CreateDate = reader.GetDateTime(2),
                    UpdateDate = reader.GetSafeDateTime(3),
                    DeleteDate = reader.GetSafeDateTime(4),
                    Attribution = reader.GetInt(5),
                    AccessPolicy = reader.GetFieldValue<AccessPolicy>(6),
                    Type = reader.GetFieldValue<RecoveryDocumentType>(7),
                    DocumentDate = reader.GetDateTime(8),
                    DocumentFile = reader.SafeGetString(9),
                };
            }
        }

        /// <summary>
        /// Update <see cref="Recovery"/>.
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

            using var connection = await DbProvider.OpenConnectionScopeAsync().ConfigureAwait(false);
            using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("note", entity.Note);
            cmd.AddParameterWithValue("access_policy", entity.AccessPolicy);
            cmd.AddParameterWithValue("type", entity.Type);
            cmd.AddParameterWithValue("document_date", entity.DocumentDate);
            cmd.AddParameterWithValue("document_file", entity.DocumentFile);
            cmd.AddParameterWithValue("id", entity.Id);
            await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
        }
    }
}
