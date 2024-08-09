using Dapper;
using FunderMaps.Core;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data.Abstractions;

namespace FunderMaps.Data.Repositories;

internal class IncidentRepository : DbServiceBase, IIncidentRepository
{
    public async Task<string> AddAsync(Incident entity)
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

    // public async Task<long> CountAsync()
    // {
    //     var sql = @"
    //         SELECT  COUNT(*)
    //         FROM    report.incident";

    //     await using var connection = DbContextFactory.DbProvider.ConnectionScope();

    //     return await connection.ExecuteScalarAsync<long>(sql);
    // }

    public async Task DeleteAsync(string id)
    {
        var sql = @"
            DELETE
            FROM    report.incident
            WHERE   id = @id";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await connection.ExecuteAsync(sql, new { id });
    }

    public async Task<Incident> GetByIdAsync(string id)
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
            LEFT JOIN    application.portal p ON p.id = substring(i.id from 'FIR(\d{2})')::int
            WHERE   i.id = upper(@id)
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.QuerySingleOrDefaultAsync<Incident>(sql, new { id })
            ?? throw new EntityNotFoundException(nameof(Incident));
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
            yield return item;
        }
    }

    public async IAsyncEnumerable<Incident> ListAllAsync(Navigation navigation)
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

        // sql = ConstructNavigation(sql, navigation);

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await foreach (var item in connection.QueryUnbufferedAsync<Incident>(sql, navigation))
        {
            yield return item;
        }
    }

    public async Task UpdateAsync(Incident entity)
    {
        Cache.Remove(entity.Id);

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
