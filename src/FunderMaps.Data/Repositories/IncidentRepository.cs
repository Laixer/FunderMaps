using Dapper;
using FunderMaps.Core;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;

namespace FunderMaps.Data.Repositories;

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
                contact_name,
                contact_phone_number,
                foundation_damage_characteristics,
                environment_damage_characteristics,
                building,
                audit_status,
                question_type,
                meta)
            VALUES (
                report.fir_generate_id(@ClientId),
                @FoundationType,
                @ChainedBuilding,
                @Owner,
                @FoundationRecovery,
                @NeightborRecovery,
                @FoundationDamageCause,
                NULLIF(@DocumentFile, '{}'::text[]),
                NULLIF(trim(@Note), ''),
                NULLIF(trim(@InternalNote), ''),
                trim(lower(@Email)),
                NULLIF(trim(@Name), ''),
                NULLIF(trim(@PhoneNumber), ''),
                NULLIF(@FoundationDamageCharacteristics, '{}'::report.foundation_damage_characteristics[]),
                NULLIF(@EnvironmentDamageCharacteristics, '{}'::report.environment_damage_characteristics[]),
                @Building,
                @AuditStatus,
                @QuestionType,
                @Meta)
            RETURNING id";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.ExecuteScalarAsync<string>(sql, entity) ?? throw new InvalidOperationException();
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

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.ExecuteScalarAsync<long>(sql);
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

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await connection.ExecuteAsync(sql, new { id });
    }

    // private static void MapToWriter(DbContext context, Incident entity)
    // {
    //     context.AddParameterWithValue("foundation_type", entity.FoundationType);
    //     context.AddParameterWithValue("chained_building", entity.ChainedBuilding);
    //     context.AddParameterWithValue("owner", entity.Owner);
    //     context.AddParameterWithValue("foundation_recovery", entity.FoundationRecovery);
    //     context.AddParameterWithValue("neightbor_recovery", entity.NeighborRecovery);
    //     context.AddParameterWithValue("foundation_damage_cause", entity.FoundationDamageCause);
    //     context.AddParameterWithValue("document_file", entity.DocumentFile);
    //     context.AddParameterWithValue("note", entity.Note);
    //     context.AddParameterWithValue("internal_note", entity.InternalNote);
    //     context.AddParameterWithValue("foundation_damage_characteristics", entity.FoundationDamageCharacteristics);
    //     context.AddParameterWithValue("environment_damage_characteristics", entity.EnvironmentDamageCharacteristics);
    //     context.AddParameterWithValue("email", entity.Email);
    //     context.AddParameterWithValue("name", entity.Name);
    //     context.AddParameterWithValue("phone_number", entity.PhoneNumber);
    //     context.AddParameterWithValue("building", entity.Building);
    //     context.AddParameterWithValue("audit_status", entity.AuditStatus);
    //     context.AddParameterWithValue("question_type", entity.QuestionType);
    //     context.AddJsonParameterWithValue("meta", entity.Meta);
    // }

    // private static Incident MapFromReader(DbDataReader reader, int offset = 0)
    //     => new()
    //     {
    //         Id = reader.GetString(offset++),
    //         ClientName = reader.GetString(offset++),
    //         FoundationType = reader.GetSafeStructValue<FoundationType>(offset++),
    //         ChainedBuilding = reader.GetBoolean(offset++),
    //         Owner = reader.GetBoolean(offset++),
    //         FoundationRecovery = reader.GetBoolean(offset++),
    //         NeighborRecovery = reader.GetBoolean(offset++),
    //         FoundationDamageCause = reader.GetSafeStructValue<FoundationDamageCause>(offset++),
    //         DocumentFile = reader.GetSafeStringArray(offset++),
    //         Note = reader.GetSafeString(offset++),
    //         InternalNote = reader.GetSafeString(offset++),
    //         Email = reader.GetString(offset++),
    //         Name = reader.GetSafeString(offset++),
    //         PhoneNumber = reader.GetSafeString(offset++),
    //         CreateDate = reader.GetDateTime(offset++),
    //         UpdateDate = reader.GetSafeDateTime(offset++),
    //         DeleteDate = reader.GetSafeDateTime(offset++),
    //         FoundationDamageCharacteristics = reader.GetSafeFieldValue<FoundationDamageCharacteristics[]>(offset++),
    //         EnvironmentDamageCharacteristics = reader.GetSafeFieldValue<EnvironmentDamageCharacteristics[]>(offset++),
    //         Building = reader.GetString(offset++),
    //         AuditStatus = reader.GetFieldValue<AuditStatus>(offset++),
    //         QuestionType = reader.GetFieldValue<IncidentQuestionType>(offset++),
    //         Meta = reader.GetSafeFieldValue<object>(offset++),
    //     };

    /// <summary>
    ///     Retrieve <see cref="Incident"/> by id.
    /// </summary>
    /// <param name="id">Unique identifier.</param>
    /// <returns><see cref="Incident"/>.</returns>
    public override async Task<Incident> GetByIdAsync(string id)
    {
        if (TryGetEntity(id, out Incident? entity))
        {
            return entity ?? throw new InvalidOperationException();
        }

        var sql = @"
            SELECT  i.id,
                    coalesce(p.name, 'other'),
                    i.foundation_type,
                    i.chained_building,
                    i.owner,
                    i.foundation_recovery,
                    i.neightbor_recovery,
                    i.foundation_damage_cause,
                    i.document_file,
                    i.note,
                    i.internal_note,
                    i.contact,
                    i.contact_name,
                    i.contact_phone_number,
                    i.create_date,
                    i.update_date,
                    i.delete_date,
                    i.foundation_damage_characteristics,
                    i.environment_damage_characteristics,
                    b.external_id,
                    i.audit_status,
                    i.question_type,
                    i.meta
            FROM    report.incident AS i
            JOIN    geocoder.building b ON b.id = i.building
            LEFT JOIN    application.portal p ON p.id = substring(i.id from 'FIR(\d{2})')::int
            WHERE   i.id = upper(@id)
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        var incident = await connection.QuerySingleOrDefaultAsync<Incident>(sql, new { id });
        return incident is null ? throw new EntityNotFoundException(nameof(Building)) : CacheEntity(incident);
    }

    public async IAsyncEnumerable<Incident> ListAllByBuildingIdAsync(string id)
    {
        // TODO: Maybe use a view for this query.
        var sql = @"
            SELECT  i.id,
                    coalesce(p.name, 'other'),
                    i.foundation_type,
                    i.chained_building,
                    i.owner,
                    i.foundation_recovery,
                    i.neightbor_recovery,
                    i.foundation_damage_cause,
                    i.document_file,
                    i.note,
                    i.internal_note,
                    i.contact,
                    i.contact_name,
                    i.contact_phone_number,
                    i.create_date,
                    i.update_date,
                    i.delete_date,
                    i.foundation_damage_characteristics,
                    i.environment_damage_characteristics,
                    b.external_id,
                    i.audit_status,
                    i.question_type,
                    i.meta
            FROM    report.incident AS i
            JOIN    geocoder.building b ON b.id = i.building
            LEFT JOIN    application.portal p ON p.id = substring(i.id from 'FIR(\d{2})')::int
            WHERE   i.building = @building";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await foreach (var item in connection.QueryUnbufferedAsync<Incident>(sql, new { building = id }))
        {
            yield return CacheEntity(item);
        }
    }

    /// <summary>
    ///     Retrieve all <see cref="Incident"/>.
    /// </summary>
    /// <returns>List of <see cref="Incident"/>.</returns>
    public override async IAsyncEnumerable<Incident> ListAllAsync(Navigation navigation)
    {
        var sql = @"
            SELECT  i.id,
                    coalesce(p.name, 'other'),
                    i.foundation_type,
                    i.chained_building,
                    i.owner,
                    i.foundation_recovery,
                    i.neightbor_recovery,
                    i.foundation_damage_cause,
                    i.document_file,
                    i.note,
                    i.internal_note,
                    i.contact,
                    i.contact_name,
                    i.contact_phone_number,
                    i.create_date,
                    i.update_date,
                    i.delete_date,
                    i.foundation_damage_characteristics,
                    i.environment_damage_characteristics,
                    b.external_id,
                    i.audit_status,
                    i.question_type,
                    i.meta
            FROM    report.incident AS i
            JOIN    geocoder.building b ON b.id = i.building
            LEFT JOIN    application.portal p ON p.id = substring(i.id from 'FIR(\d{2})')::int";

        sql = ConstructNavigation(sql, navigation);

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await foreach (var item in connection.QueryUnbufferedAsync<Incident>(sql, navigation))
        {
            yield return CacheEntity(item);
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
            SET     foundation_type = @FoundationType,
                    chained_building = @ChainedBuilding,
                    owner = @Owner,
                    foundation_recovery = @FoundationRecovery,
                    neightbor_recovery = @NeightborRecovery,
                    foundation_damage_cause = @FoundationDamageCause,
                    document_file = NULLIF(@DocumentFile, '{}'::text[]),
                    note = NULLIF(trim(@Note), ''),
                    internal_note = NULLIF(trim(@InternalNote), ''),
                    foundation_damage_characteristics = NULLIF(@FoundationDamageCharacteristics, '{}'::report.foundation_damage_characteristics[]),
                    environment_damage_characteristics = NULLIF(@EnvironmentDamageCharacteristics, '{}'::report.environment_damage_characteristics[]),
                    audit_status = @AuditStatus,
                    question_type = @QuestionType,
                    meta = @Meta
            WHERE   id = upper(@Id)";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await connection.ExecuteAsync(sql, entity);
    }
}
