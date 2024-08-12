using Dapper;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data.Abstractions;

namespace FunderMaps.Data.Repositories;

internal sealed class UserdataRepository : DbServiceBase, IUserdataRepository
{
    public async Task<UserData> GetAsync(Guid user_id, string application_id)
    {
        var sql = @"
            SELECT  metadata, update_date
            FROM    application.application_user
            WHERE   user_id = @user_id AND application_id = @application_id
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.QuerySingleOrDefaultAsync<UserData>(sql, new { user_id, application_id })
            ?? new UserData();
    }

    public async Task UpdateAsync(Guid user_id, string application_id, UserData userdata)
    {
        var sql = @"
            INSERT INTO application.application_user (user_id, application_id, metadata)
            VALUES (@user_id, @application_id, @metadata)
            ON CONFLICT (user_id, application_id)
            DO UPDATE SET
                metadata = @metadata,
                update_date = NOW()";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await connection.ExecuteAsync(sql, new { user_id, application_id, userdata.Metadata });
    }
}
