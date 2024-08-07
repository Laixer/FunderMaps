using Dapper;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data.Abstractions;

namespace FunderMaps.Data.Repositories;

internal sealed class UserdataRepository : DbServiceBase, IUserdataRepository
{
    public async Task<object> GetAsync(Guid user_id, string application_id)
    {
        var sql = @"
            SELECT  metadata
            FROM    application.application_user
            WHERE   user_id = @user_id AND application_id = @application_id
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.ExecuteScalarAsync<object>(sql, new { user_id, application_id })
            ?? new { };
    }

    public async Task UpdateAsync(Guid user_id, string application_id, object metadata)
    {
        var sql = @"
            INSERT INTO application.application_user (user_id, application_id, metadata)
            VALUES (@user_id, @application_id, @metadata)
            ON CONFLICT (user_id, application_id)
            DO UPDATE SET
                metadata = @metadata";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await connection.ExecuteAsync(sql, new { user_id, application_id, metadata });
    }
}
