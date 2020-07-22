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
    ///     Inquiry repository.
    /// </summary>
    internal class InquiryRepository : RepositoryBase<Inquiry, int>, IInquiryRepository
    {
        /// <summary>
        ///     Create a new instance.
        /// </summary>
        /// <param name="dbProvider">Database provider.</param>
        public InquiryRepository(DbProvider dbProvider)
            : base(dbProvider)
        {
        }

        /// <summary>
        ///     Create new <see cref="Inquiry"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        /// <returns>Created <see cref="Inquiry"/>.</returns>
        public override async ValueTask<int> AddAsync(Inquiry entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var sql = @"
                INSERT INTO report.inquiry(
                    document_name,
                    inspection,
                    joint_measurement,
                    floor_measurement,
                    note,
                    document_date,
                    document_file,
                    attribution,
                    access_policy,
                    audit_status,
                    type,
                    standard_f3o)
                VALUES (
                    @document_name,
                    @inspection,
                    @joint_measurement,
                    @floor_measurement,
                    @note,
                    @document_date,
                    @document_file,
                    @attribution,
                    @access_policy,
                    @audit_status,
                    @type,
                    @standard_f3o)
                RETURNING id;
            ";

            await using var connection = await DbProvider.OpenConnectionScopeAsync().ConfigureAwait(false);
            await using var cmd = DbProvider.CreateCommand(sql, connection);

            MapToWriter(cmd, entity);

            return await cmd.ExecuteScalarIntAsync().ConfigureAwait(false); // TODO: Ensure
        }

        public Task<uint> CountAsync(Guid orgId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Retrieve number of entities.
        /// </summary>
        /// <returns>Number of entities.</returns>
        public override ValueTask<ulong> CountAsync()
        {
            var sql = @"
                SELECT  COUNT(*)
                FROM    report.inquiry";

            return ExecuteScalarUnsignedLongCommandAsync(sql); // TODO: Ensure
        }

        /// <summary>
        ///     Delete <see cref="Inquiry"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        public override async ValueTask DeleteAsync(int id)
        {
            var sql = @"
                DELETE
                FROM    report.inquiry
                WHERE   id = @id";

            await using var connection = await DbProvider.OpenConnectionScopeAsync().ConfigureAwait(false);
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("id", id);
            await cmd.ExecuteNonQueryEnsureAffectedAsync().ConfigureAwait(false);
        }

        private static void MapToWriter(DbCommand cmd, Inquiry entity)
        {
            cmd.AddParameterWithValue("document_name", entity.DocumentName);
            cmd.AddParameterWithValue("inspection", entity.Inspection);
            cmd.AddParameterWithValue("joint_measurement", entity.JointMeasurement);
            cmd.AddParameterWithValue("floor_measurement", entity.FloorMeasurement);
            cmd.AddParameterWithValue("note", entity.Note);
            cmd.AddParameterWithValue("document_date", entity.DocumentDate);
            cmd.AddParameterWithValue("document_file", entity.DocumentFile);
            cmd.AddParameterWithValue("attribution", entity.Attribution);
            cmd.AddParameterWithValue("access_policy", entity.AccessPolicy);
            cmd.AddParameterWithValue("audit_status", entity.AuditStatus);
            cmd.AddParameterWithValue("type", entity.Type);
            cmd.AddParameterWithValue("standard_f3o", entity.StandardF3o);
        }

        private static Inquiry MapFromReader(DbDataReader reader)
            => new Inquiry
            {
                Id = reader.GetInt(0),
                DocumentName = reader.SafeGetString(1),
                Inspection = reader.GetBoolean(2),
                JointMeasurement = reader.GetBoolean(3),
                FloorMeasurement = reader.GetBoolean(4),
                CreateDate = reader.GetDateTime(5),
                UpdateDate = reader.GetSafeDateTime(6),
                DeleteDate = reader.GetSafeDateTime(7),
                Note = reader.SafeGetString(8),
                DocumentDate = reader.GetDateTime(9),
                DocumentFile = reader.SafeGetString(10),
                Attribution = reader.GetInt(11),
                AccessPolicy = reader.GetFieldValue<AccessPolicy>(12),
                AuditStatus = reader.GetFieldValue<AuditStatus>(13),
                Type = reader.GetFieldValue<InquiryType>(14),
                StandardF3o = reader.GetBoolean(15),
            };

        public Task<Inquiry> GetByIdAsync(int id, Guid orgId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Retrieve <see cref="Inquiry"/> by id.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <returns><see cref="Inquiry"/>.</returns>
        public override async ValueTask<Inquiry> GetByIdAsync(int id)
        {
            var sql = @"
                SELECT  id,
                        document_name,
                        inspection,
                        joint_measurement,
                        floor_measurement,
                        create_date,
                        update_date,
                        delete_date,
                        note,
                        document_date,
                        document_file,
                        attribution,
                        access_policy,
                        audit_status,
                        type,
                        standard_f3o
                FROM    report.inquiry
                WHERE   id = @id
                LIMIT   1";

            await using var connection = await DbProvider.OpenConnectionScopeAsync().ConfigureAwait(false);
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("id", id);

            await using var reader = await cmd.ExecuteReaderAsyncEnsureRowAsync().ConfigureAwait(false);
            await reader.ReadAsync().ConfigureAwait(false);

            return MapFromReader(reader);
        }

        public Task<Inquiry> GetPublicAndByIdAsync(int id, Guid orgId)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Inquiry>> ListAllAsync(Guid orgId, INavigation navigation)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Retrieve all <see cref="Inquiry"/>.
        /// </summary>
        /// <returns>List of <see cref="Inquiry"/>.</returns>
        public override async IAsyncEnumerable<Inquiry> ListAllAsync(INavigation navigation)
        {
            if (navigation == null)
            {
                throw new ArgumentNullException(nameof(navigation));
            }

            var sql = @"
                SELECT  id,
                        document_name,
                        inspection,
                        joint_measurement,
                        floor_measurement,
                        create_date,
                        update_date,
                        delete_date,
                        note,
                        document_date,
                        document_file,
                        attribution,
                        access_policy,
                        audit_status,
                        type,
                        standard_f3o
                FROM    report.inquiry";

            ConstructNavigation(ref sql, navigation);

            await using var connection = await DbProvider.OpenConnectionScopeAsync().ConfigureAwait(false);
            await using var cmd = DbProvider.CreateCommand(sql, connection);

            await using var reader = await cmd.ExecuteReaderAsyncEnsureRowAsync().ConfigureAwait(false);
            while (await reader.ReadAsync().ConfigureAwait(false))
            {
                yield return MapFromReader(reader);
            }
        }

        /// <summary>
        ///     Update <see cref="Inquiry"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        public override async ValueTask UpdateAsync(Inquiry entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var sql = @"
                    UPDATE  report.inquiry
                    SET     document_name = @document_name,
                            inspection = @inspection,
                            joint_measurement = @joint_measurement,
                            floor_measurement = @floor_measurement,
                            note = @note,
                            document_date = @document_date,
                            document_file = @document_file,
                            attribution = @attribution,
                            access_policy = @access_policy,
                            audit_status = @audit_status,
                            type = @type,
                            standard_f3o = @standard_f3o
                    WHERE   id = @id";

            using var connection = await DbProvider.OpenConnectionScopeAsync().ConfigureAwait(false);
            using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("id", entity.Id);

            MapToWriter(cmd, entity);

            await cmd.ExecuteNonQueryEnsureAffectedAsync().ConfigureAwait(false);
        }
    }
}
