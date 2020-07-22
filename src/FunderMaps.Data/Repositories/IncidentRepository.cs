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
    ///     Incident repository.
    /// </summary>
    internal class IncidentRepository : RepositoryBase<Incident, string>, IIncidentRepository
    {
        /// <summary>
        ///     Create a new instance.
        /// </summary>
        /// <param name="dbProvider">Database provider.</param>
        public IncidentRepository(DbProvider dbProvider)
            : base(dbProvider)
        {
        }

        /// <summary>
        ///     Create new <see cref="Incident"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        /// <returns>Created <see cref="Incident"/>.</returns>
        /// <exception cref="NullResultException"> is thrown if statement had no affect.</exception>
        public override async ValueTask<string> AddAsync(Incident entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var sql = @"
                INSERT INTO report.incident(
                    id,
                    foundation_type,
                    chained_building,
                    owner,
                    foundation_recovery,
                    neightbor_recovery,
                    foundation_damage_cause,
                    document_file,
                    note,
                    internal_note,
                    contact,
                    foundation_damage_characteristics,
                    environment_damage_characteristics,
                    address,
                    audit_status,
                    question_type,
                    meta)
                VALUES (
                    report.fir_generate_id(@client_id),
                    @foundation_type,
                    @chained_building,
                    @owner,
                    @foundation_recovery,
                    @neightbor_recovery,
                    @foundation_damage_cause,
                    @document_file,
                    @note,
                    @internal_note,
                    @email,
                    @foundation_damage_characteristics,
                    @environment_damage_characteristics,
                    @address,
                    @audit_status,
                    @question_type,
                    @meta)
                RETURNING id;
            ";

            await using var connection = await DbProvider.OpenConnectionScopeAsync().ConfigureAwait(false);
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("client_id", entity.ClientId);

            MapToWriter(cmd, entity);

            await using var reader = await cmd.ExecuteReaderAsyncEnsureRowAsync().ConfigureAwait(false);
            await reader.ReadAsync().ConfigureAwait(false);
            return reader.SafeGetString(0);
        }

        /// <summary>
        ///     Retrieve number of entities.
        /// </summary>
        /// <returns>Number of entities.</returns>
        public override ValueTask<ulong> CountAsync()
        {
            var sql = @"
                SELECT  COUNT(*)
                FROM    report.incident";

            return ExecuteScalarUnsignedLongCommandAsync(sql);
        }

        /// <summary>
        ///     Delete <see cref="Incident"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        /// <exception cref="NullResultException"> is thrown if statement had no affect.</exception>
        public override async ValueTask DeleteAsync(string id)
        {
            var sql = @"
                DELETE
                FROM    report.incident
                WHERE   id = @id";

            await using var connection = await DbProvider.OpenConnectionScopeAsync().ConfigureAwait(false);
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("id", id);
            await cmd.ExecuteNonQueryEnsureAffectedAsync().ConfigureAwait(false);
        }

        private static void MapToWriter(DbCommand cmd, Incident entity)
        {
            cmd.AddParameterWithValue("foundation_type", entity.FoundationType);
            cmd.AddParameterWithValue("chained_building", entity.ChainedBuilding);
            cmd.AddParameterWithValue("owner", entity.Owner);
            cmd.AddParameterWithValue("foundation_recovery", entity.FoundationRecovery);
            cmd.AddParameterWithValue("neightbor_recovery", entity.NeightborRecovery);
            cmd.AddParameterWithValue("foundation_damage_cause", entity.FoundationDamageCause);
            cmd.AddParameterWithValue("document_file", entity.DocumentFile);
            cmd.AddParameterWithValue("note", entity.Note);
            cmd.AddParameterWithValue("internal_note", entity.InternalNote);
            cmd.AddParameterWithValue("foundation_damage_characteristics", entity.FoundationDamageCharacteristics);
            cmd.AddParameterWithValue("environment_damage_characteristics", entity.EnvironmentDamageCharacteristics);
            cmd.AddParameterWithValue("email", entity.Email);
            cmd.AddParameterWithValue("address", entity.Address);
            cmd.AddParameterWithValue("audit_status", entity.AuditStatus);
            cmd.AddParameterWithValue("question_type", entity.QuestionType);
            cmd.AddJsonParameterWithValue("meta", entity.Meta);
        }

        private static Incident MapFromReader(DbDataReader reader)
            => new Incident
            {
                Id = reader.SafeGetString(0),
                FoundationType = reader.GetFieldValue<FoundationType>(1),
                ChainedBuilding = reader.GetBoolean(2),
                Owner = reader.GetBoolean(3),
                FoundationRecovery = reader.GetBoolean(4),
                NeightborRecovery = reader.GetBoolean(5),
                FoundationDamageCause = reader.GetFieldValue<FoundationDamageCause>(6),
                DocumentFile = reader.GetSafeFieldValue<string[]>(7),
                Note = reader.SafeGetString(8),
                InternalNote = reader.SafeGetString(9),
                Email = reader.SafeGetString(10),
                CreateDate = reader.GetDateTime(11),
                UpdateDate = reader.GetSafeDateTime(12),
                DeleteDate = reader.GetSafeDateTime(13),
                FoundationDamageCharacteristics = reader.GetFieldValue<FoundationDamageCharacteristics[]>(14),
                EnvironmentDamageCharacteristics = reader.GetFieldValue<EnvironmentDamageCharacteristics[]>(15),
                Address = reader.SafeGetString(16),
                AuditStatus = reader.GetFieldValue<AuditStatus>(17),
                QuestionType = reader.GetFieldValue<IncidentQuestionType>(18),
                Meta = reader.GetFieldValue<object>(19),
            };

        /// <summary>
        ///     Retrieve <see cref="Incident"/> by id.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <returns><see cref="Incident"/>.</returns>
        /// <exception cref="NullResultException"> is thrown if statement had no affect.</exception>
        public override async ValueTask<Incident> GetByIdAsync(string id)
        {
            var sql = @"
                SELECT  id,
                        foundation_type,
                        chained_building,
                        owner,
                        foundation_recovery,
                        neightbor_recovery,
                        foundation_damage_cause,
                        document_file,
                        note,
                        internal_note,
                        contact,
                        create_date,
                        update_date,
                        delete_date,
                        foundation_damage_characteristics,
                        environment_damage_characteristics,
                        address,
                        audit_status,
                        question_type,
                        meta
                FROM    report.incident
                WHERE   id = @id
                LIMIT   1";

            await using var connection = await DbProvider.OpenConnectionScopeAsync().ConfigureAwait(false);
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("id", id);

            await using var reader = await cmd.ExecuteReaderAsyncEnsureRowAsync().ConfigureAwait(false);
            await reader.ReadAsync().ConfigureAwait(false);

            return MapFromReader(reader);
        }

        /// <summary>
        ///     Retrieve all <see cref="Incident"/>.
        /// </summary>
        /// <returns>List of <see cref="Incident"/>.</returns>
        /// <exception cref="NullResultException"> is thrown if statement had no affect.</exception>
        public override async IAsyncEnumerable<Incident> ListAllAsync(INavigation navigation)
        {
            if (navigation == null)
            {
                throw new ArgumentNullException(nameof(navigation));
            }

            var sql = @"
                SELECT  id,
                        foundation_type,
                        chained_building,
                        owner,
                        foundation_recovery,
                        neightbor_recovery,
                        foundation_damage_cause,
                        document_file,
                        note,
                        internal_note,
                        contact,
                        create_date,
                        update_date,
                        delete_date,
                        foundation_damage_characteristics,
                        environment_damage_characteristics,
                        address,
                        audit_status,
                        question_type,
                        meta
                FROM    report.incident";

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
        ///     Update <see cref="Incident"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        /// <exception cref="NullResultException"> is thrown if statement had no affect.</exception>
        public override async ValueTask UpdateAsync(Incident entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var sql = @"
                    UPDATE  report.incident
                    SET     foundation_type = @foundation_type,
                            chained_building = @chained_building,
                            owner = @owner,
                            foundation_recovery = @foundation_recovery,
                            neightbor_recovery = @neightbor_recovery,
                            foundation_damage_cause = @foundation_damage_cause,
                            document_file = @document_file,
                            note = @note,
                            internal_note = @internal_note,
                            contact = @email,
                            foundation_damage_characteristics = @foundation_damage_characteristics,
                            environment_damage_characteristics = @environment_damage_characteristics,
                            address = @address,
                            audit_status = @audit_status,
                            question_type = @question_type,
                            meta = @meta
                    WHERE   id = @id";

            using var connection = await DbProvider.OpenConnectionScopeAsync().ConfigureAwait(false);
            using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("id", entity.Id);

            MapToWriter(cmd, entity);

            await cmd.ExecuteNonQueryEnsureAffectedAsync().ConfigureAwait(false);
        }
    }
}
