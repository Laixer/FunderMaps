using Dapper;
using FunderMaps.Core.Exceptions;
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

        var metadata = await connection.QuerySingleOrDefaultAsync<object>(sql, new { user_id, application_id })
            ?? throw new EntityNotFoundException();

        return metadata;
    }

    public async Task UpdateAsync(Guid user_id, string application_id, object metadata)
    {
        var sql = @"
            UPDATE application.application_user
            SET metadata = @metadata
            WHERE user_id = @user_id AND application_id = @application_id";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await connection.ExecuteAsync(sql, new { user_id, application_id, metadata });
    }
}
