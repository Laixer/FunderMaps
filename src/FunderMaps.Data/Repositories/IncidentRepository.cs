using FunderMaps.Core;
using FunderMaps.Core.Entities;
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
    ///     Incident repository.
    /// </summary>
    internal class IncidentRepository : RepositoryBase<Incident, string>, IIncidentRepository
    {
        /// <summary>
        ///     Create new <see cref="Incident"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        /// <returns>Created <see cref="Incident"/>.</returns>
        public override async Task<string> AddAsync(Incident entity)
        {
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
                    NULLIF(@document_file, '{}'::text[]),
                    NULLIF(trim(@note), ''),
                    NULLIF(trim(@internal_note), ''),
                    NULLIF(trim(@email), ''),
                    NULLIF(@foundation_damage_characteristics, '{}'::report.foundation_damage_characteristics[]),
                    NULLIF(@environment_damage_characteristics, '{}'::report.environment_damage_characteristics[]),
                    @address,
                    @audit_status,
                    @question_type,
                    @meta)
                RETURNING id";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("client_id", entity.ClientId);

            MapToWriter(context, entity);

            await using var reader = await context.ReaderAsync();

            return reader.GetString(0);
        }

        /// <summary>
        ///     Retrieve number of entities.
        /// </summary>
        /// <returns>Number of entities.</returns>
        public override async Task<long> CountAsync()
        {
            var sql = @"
                SELECT  COUNT(*)
                FROM    report.incident";

            await using var context = await DbContextFactory.CreateAsync(sql);

            return await context.ScalarAsync<long>();
        }

        /// <summary>
        ///     Delete <see cref="Incident"/>.
        /// </summary>
        /// <param name="id">Entity identifier.</param>
        public override async Task DeleteAsync(string id)
        {
            ResetCacheEntity(id);

            var sql = @"
                DELETE
                FROM    report.incident
                WHERE   id = @id";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("id", id);

            await context.NonQueryAsync();
        }

        public static void MapToWriter(DbContext context, Incident entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            context.AddParameterWithValue("foundation_type", entity.FoundationType);
            context.AddParameterWithValue("chained_building", entity.ChainedBuilding);
            context.AddParameterWithValue("owner", entity.Owner);
            context.AddParameterWithValue("foundation_recovery", entity.FoundationRecovery);
            context.AddParameterWithValue("neightbor_recovery", entity.NeighborRecovery);
            context.AddParameterWithValue("foundation_damage_cause", entity.FoundationDamageCause);
            context.AddParameterWithValue("document_file", entity.DocumentFile);
            context.AddParameterWithValue("note", entity.Note);
            context.AddParameterWithValue("internal_note", entity.InternalNote);
            context.AddParameterWithValue("foundation_damage_characteristics", entity.FoundationDamageCharacteristics);
            context.AddParameterWithValue("environment_damage_characteristics", entity.EnvironmentDamageCharacteristics);
            context.AddParameterWithValue("email", entity.Email);
            context.AddParameterWithValue("address", entity.Address);
            context.AddParameterWithValue("audit_status", entity.AuditStatus);
            context.AddParameterWithValue("question_type", entity.QuestionType);
            context.AddJsonParameterWithValue("meta", entity.Meta);
        }

        public static Incident MapFromReader(DbDataReader reader, int offset = 0)
            => new()
            {
                Id = reader.GetSafeString(offset + 0),
                FoundationType = reader.GetFieldValue<FoundationType>(offset + 1),
                ChainedBuilding = reader.GetBoolean(offset + 2),
                Owner = reader.GetBoolean(offset + 3),
                FoundationRecovery = reader.GetBoolean(offset + 4),
                NeighborRecovery = reader.GetBoolean(offset + 5),
                FoundationDamageCause = reader.GetFieldValue<FoundationDamageCause>(offset + 6),
                DocumentFile = reader.GetSafeFieldValue<string[]>(offset + 7),
                Note = reader.GetSafeString(offset + 8),
                InternalNote = reader.GetSafeString(offset + 9),
                Email = reader.GetSafeString(offset + 10),
                CreateDate = reader.GetDateTime(offset + 11),
                UpdateDate = reader.GetSafeDateTime(offset + 12),
                DeleteDate = reader.GetSafeDateTime(offset + 13),
                FoundationDamageCharacteristics = reader.GetSafeFieldValue<FoundationDamageCharacteristics[]>(offset + 14),
                EnvironmentDamageCharacteristics = reader.GetSafeFieldValue<EnvironmentDamageCharacteristics[]>(offset + 15),
                Address = reader.GetSafeString(offset + 16),
                AuditStatus = reader.GetFieldValue<AuditStatus>(offset + 17),
                QuestionType = reader.GetFieldValue<IncidentQuestionType>(offset + 18),
                Meta = reader.GetFieldValue<object>(offset + 19),
            };

        /// <summary>
        ///     Retrieve <see cref="Incident"/> by id.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <returns><see cref="Incident"/>.</returns>
        public override async Task<Incident> GetByIdAsync(string id)
        {
            if (TryGetEntity(id, out Incident entity))
            {
                return entity;
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
                FROM    report.incident
                WHERE   id = @id
                LIMIT   1";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("id", id);

            await using var reader = await context.ReaderAsync();

            return CacheEntity(MapFromReader(reader));
        }

        /// <summary>
        ///     Retrieve all <see cref="Incident"/>.
        /// </summary>
        /// <returns>List of <see cref="Incident"/>.</returns>
        public override async IAsyncEnumerable<Incident> ListAllAsync(Navigation navigation)
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
                FROM    report.incident";

            ConstructNavigation(sql, navigation);

            await using var context = await DbContextFactory.CreateAsync(sql);

            await foreach (var reader in context.EnumerableReaderAsync())
            {
                yield return CacheEntity(MapFromReader(reader));
            }
        }

        /// <summary>
        ///     Update <see cref="Incident"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        public override async Task UpdateAsync(Incident entity)
        {
            ResetCacheEntity(entity);

            var sql = @"
                    UPDATE  report.incident
                    SET     foundation_type = @foundation_type,
                            chained_building = @chained_building,
                            owner = @owner,
                            foundation_recovery = @foundation_recovery,
                            neightbor_recovery = @neightbor_recovery,
                            foundation_damage_cause = @foundation_damage_cause,
                            document_file = NULLIF(@document_file, '{}'::text[]),
                            note = NULLIF(trim(@note), ''),
                            internal_note = NULLIF(trim(@internal_note), ''),
                            foundation_damage_characteristics = NULLIF(@foundation_damage_characteristics, '{}'::report.foundation_damage_characteristics[]),
                            environment_damage_characteristics = NULLIF(@environment_damage_characteristics, '{}'::report.environment_damage_characteristics[]),
                            audit_status = @audit_status,
                            question_type = @question_type,
                            meta = @meta
                    WHERE   id = @id";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("id", entity.Id);

            MapToWriter(context, entity);

            await context.NonQueryAsync();
        }
    }
}
