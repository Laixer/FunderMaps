using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using FunderMaps.Core.Types.Control;
using FunderMaps.Data.Extensions;
using FunderMaps.Data.Providers;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;

namespace FunderMaps.Data.Repositories
{
    /// <summary>
    ///     Inquiry repository.
    /// </summary>
    internal class InquiryRepository : RepositoryBase<InquiryFull, int>, IInquiryRepository
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
        ///     Create new <see cref="InquiryFull"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        /// <returns>Created <see cref="InquiryFull"/>.</returns>
        public override async ValueTask<int> AddAsync(InquiryFull entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var sql = @"
                WITH attribution AS (
	                INSERT INTO application.attribution(
                        reviewer,
                        creator,
                        owner,
                        contractor)
		            VALUES (
                        @reviewer,
                        @creator,
                        @owner,
                        @contractor)
	                RETURNING id AS attribution_id
                )
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
                SELECT @document_name,
                    @inspection,
                    @joint_measurement,
                    @floor_measurement,
                    @note,
                    @document_date,
                    @document_file,
	                attribution_id,
                    @access_policy,
                    @audit_status,
	                @type,
                    @standard_f3o
                FROM attribution
                RETURNING id";

            await using var connection = await DbProvider.OpenConnectionScopeAsync();
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("reviewer", entity.Attribution.Reviewer);
            cmd.AddParameterWithValue("creator", entity.Attribution.Creator);
            cmd.AddParameterWithValue("owner", entity.Attribution.Owner);
            cmd.AddParameterWithValue("contractor", entity.Attribution.Contractor);

            MapToWriter(cmd, entity);

            return await cmd.ExecuteScalarIntAsync();
        }

        public Task<uint> CountAsync(Guid orgId) => throw new NotImplementedException();

        /// <summary>
        ///     Retrieve number of entities.
        /// </summary>
        /// <returns>Number of entities.</returns>
        public override ValueTask<ulong> CountAsync()
        {
            var sql = @"
                SELECT  COUNT(*)
                FROM    report.inquiry";

            return ExecuteScalarUnsignedLongCommandAsync(sql);
        }

        /// <summary>
        ///     Delete <see cref="InquiryFull"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        public override async ValueTask DeleteAsync(int id)
        {
            var sql = @"
                DELETE
                FROM    report.inquiry
                WHERE   id = @id";

            await using var connection = await DbProvider.OpenConnectionScopeAsync();
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("id", id);
            await cmd.ExecuteNonQueryEnsureAffectedAsync();
        }

        private static void MapToWriter(DbCommand cmd, InquiryFull entity)
        {
            cmd.AddParameterWithValue("document_name", entity.DocumentName);
            cmd.AddParameterWithValue("inspection", entity.Inspection);
            cmd.AddParameterWithValue("joint_measurement", entity.JointMeasurement);
            cmd.AddParameterWithValue("floor_measurement", entity.FloorMeasurement);
            cmd.AddParameterWithValue("note", entity.Note);
            cmd.AddParameterWithValue("document_date", entity.DocumentDate);
            cmd.AddParameterWithValue("document_file", entity.DocumentFile);
            cmd.AddParameterWithValue("access_policy", entity.Access.AccessPolicy);
            cmd.AddParameterWithValue("audit_status", entity.State.AuditStatus);
            cmd.AddParameterWithValue("type", entity.Type);
            cmd.AddParameterWithValue("standard_f3o", entity.StandardF3o);
        }

        private static InquiryFull MapFromReader(DbDataReader reader)
            => new InquiryFull
            {
                Id = reader.GetInt(0),
                DocumentName = reader.GetSafeString(1),
                Inspection = reader.GetBoolean(2),
                JointMeasurement = reader.GetBoolean(3),
                FloorMeasurement = reader.GetBoolean(4),
                Note = reader.GetSafeString(5),
                DocumentDate = reader.GetDateTime(6),
                DocumentFile = reader.GetSafeString(7),
                Type = reader.GetFieldValue<InquiryType>(8),
                StandardF3o = reader.GetBoolean(9),
                Attribution = new AttributionControl
                {
                    Reviewer = reader.GetFieldValue<Guid?>(10),
                    Creator = reader.GetGuid(11),
                    Owner = reader.GetGuid(12),
                    Contractor = reader.GetGuid(13),
                },
                State = new StateControl
                {
                    AuditStatus = reader.GetFieldValue<AuditStatus>(14),
                },
                Access = new AccessControl
                {
                    AccessPolicy = reader.GetFieldValue<AccessPolicy>(15),
                },
                Record = new RecordControl
                {
                    CreateDate = reader.GetDateTime(16),
                    UpdateDate = reader.GetSafeDateTime(17),
                    DeleteDate = reader.GetSafeDateTime(18),
                },
            };

        public Task<InquiryFull> GetByIdAsync(int id, Guid orgId) => throw new NotImplementedException();

        /// <summary>
        ///     Retrieve <see cref="InquiryFull"/> by id.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <returns><see cref="InquiryFull"/>.</returns>
        public override async ValueTask<InquiryFull> GetByIdAsync(int id)
        {
            var sql = @"
                SELECT  inquiry.id,
                        document_name,
                        inspection,
                        joint_measurement,
                        floor_measurement,
                        note,
                        document_date,
                        document_file,
                        type,
                        standard_f3o,

                        -- attribution
                        attribution.reviewer,
                        attribution.creator,
                        attribution.owner,
                        attribution.contractor,

                        -- state control
                        audit_status,

                        -- access control
                        access_policy,
		                
                        -- record control
                        create_date,
		                update_date,
		                delete_date
                FROM    report.inquiry
                JOIN 	application.attribution ON attribution.id = inquiry.attribution
                WHERE   inquiry.id = @id
                LIMIT   1";

            await using var connection = await DbProvider.OpenConnectionScopeAsync();
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("id", id);

            await using var reader = await cmd.ExecuteReaderAsyncEnsureRowAsync();
            await reader.ReadAsync();

            return MapFromReader(reader);
        }

        public Task<InquiryFull> GetPublicAndByIdAsync(int id, Guid orgId) => throw new NotImplementedException();

        public Task<IReadOnlyList<InquiryFull>> ListAllAsync(Guid orgId, INavigation navigation) => throw new NotImplementedException();

        /// <summary>
        ///     Retrieve all <see cref="InquiryFull"/>.
        /// </summary>
        /// <returns>List of <see cref="InquiryFull"/>.</returns>
        public override async IAsyncEnumerable<InquiryFull> ListAllAsync(INavigation navigation)
        {
            if (navigation == null)
            {
                throw new ArgumentNullException(nameof(navigation));
            }

            var sql = @"
                SELECT  inquiry.id,
                        document_name,
                        inspection,
                        joint_measurement,
                        floor_measurement,
                        note,
                        document_date,
                        document_file,
                        type,
                        standard_f3o,

                        -- attribution
                        attribution.reviewer,
                        attribution.creator,
                        attribution.owner,
                        attribution.contractor,

                        -- state control
                        audit_status,

                        -- access control
                        access_policy,
		                
                        -- record control
                        create_date,
		                update_date,
		                delete_date
                FROM    report.inquiry
                JOIN 	application.attribution ON attribution.id = inquiry.attribution";

            ConstructNavigation(ref sql, navigation);

            await using var connection = await DbProvider.OpenConnectionScopeAsync();
            await using var cmd = DbProvider.CreateCommand(sql, connection);

            await using var reader = await cmd.ExecuteReaderCanHaveZeroRowsAsync();
            while (await reader.ReadAsync())
            {
                yield return MapFromReader(reader);
            }
        }

        /// <summary>
        ///     Update <see cref="InquiryFull"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        public override async ValueTask UpdateAsync(InquiryFull entity)
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
                            access_policy = @access_policy,
                            audit_status = @audit_status,
                            type = @type,
                            standard_f3o = @standard_f3o
                    WHERE   id = @id";

            using var connection = await DbProvider.OpenConnectionScopeAsync();
            using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("id", entity.Id);

            MapToWriter(cmd, entity);

            await cmd.ExecuteNonQueryEnsureAffectedAsync();
        }
    }
}
