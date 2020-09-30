using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using FunderMaps.Core.Types.Control;
using FunderMaps.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace FunderMaps.Data.Repositories
{
    /// <summary>
    ///     Inquiry repository.
    /// </summary>
    internal class InquiryRepository : RepositoryBase<InquiryFull, int>, IInquiryRepository
    {
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

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("reviewer", entity.Attribution.Reviewer);
            context.AddParameterWithValue("creator", entity.Attribution.Creator);
            context.AddParameterWithValue("owner", entity.Attribution.Owner);
            context.AddParameterWithValue("contractor", entity.Attribution.Contractor);

            MapToWriter(context.Command, entity);

            return await context.ScalarAsync<int>();
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

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("id", id);

            await context.NonQueryAsync();
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

        public Task<InquiryFull> GetByIdAsync(int id, Guid orgId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Retrieve <see cref="InquiryFull"/> by id.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <returns><see cref="InquiryFull"/>.</returns>
        public override async ValueTask<InquiryFull> GetByIdAsync(int id)
        {
            var sql = @"
                SELECT  -- Inquiry
                        inquiry.id,
                        document_name,
                        inspection,
                        joint_measurement,
                        floor_measurement,
                        note,
                        document_date,
                        document_file,
                        type,
                        standard_f3o,

                        -- Attribution
                        attribution.reviewer,
                        attribution.creator,
                        attribution.owner,
                        attribution.contractor,

                        -- State control
                        audit_status,

                        -- Access control
                        access_policy,
		                
                        -- Record control
                        create_date,
		                update_date,
		                delete_date
                FROM    report.inquiry
                JOIN 	application.attribution ON attribution.id = inquiry.attribution
                WHERE   inquiry.id = @id
                LIMIT   1";

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("id", id);

            await using var reader = await context.ReaderAsync();

            return MapFromReader(reader);
        }

        public Task<InquiryFull> GetPublicAndByIdAsync(int id, Guid orgId)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<InquiryFull>> ListAllAsync(Guid orgId, INavigation navigation)
        {
            throw new NotImplementedException();
        }

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
                SELECT  -- Inquiry
                        inquiry.id,
                        document_name,
                        inspection,
                        joint_measurement,
                        floor_measurement,
                        note,
                        document_date,
                        document_file,
                        type,
                        standard_f3o,

                        -- Attribution
                        attribution.reviewer,
                        attribution.creator,
                        attribution.owner,
                        attribution.contractor,

                        -- State control
                        audit_status,

                        -- Access control
                        access_policy,
		                
                        -- Record control
                        create_date,
		                update_date,
		                delete_date
                FROM    report.inquiry
                JOIN 	application.attribution ON attribution.id = inquiry.attribution";

            ConstructNavigation(ref sql, navigation);

            await using var context = await DbContextFactory(sql);

            await foreach (var reader in context.EnumerableReaderAsync())
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

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("id", entity.Id);

            MapToWriter(context.Command, entity);

            await context.NonQueryAsync();
        }
    }
}
