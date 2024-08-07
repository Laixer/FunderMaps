using Dapper;
using FunderMaps.Core;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace FunderMaps.Data.Repositories;

internal class UserRepository : RepositoryBase<User, Guid>, IUserRepository
{
    public override async Task<Guid> AddAsync(User entity)
    {
        var sql = @$"
            INSERT INTO application.user (
                given_name,
                last_name,
                email,
                job_title,
                phone_number,
                role)
            VALUES (
                NULLIF(trim(@GivenName), ''),
                NULLIF(trim(@LastName), ''),
                application.normalize2(@Email),
                NULLIF(trim(@JobTitle), ''),
                REGEXP_REPLACE(@PhoneNumber,'\D','','g'),
                @Role)
            RETURNING id";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.ExecuteScalarAsync<Guid>(sql, entity);
    }

    public override async Task<long> CountAsync()
    {
        var sql = @"
            SELECT  COUNT(*)
            FROM    application.user";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.ExecuteScalarAsync<long>(sql);
    }

    // FUTURE: If user is in use it violates foreign key constraint, returning
    //         a ReferenceNotFoundException, which is invalid.
    public override async Task DeleteAsync(Guid id)
    {
        ResetCacheEntity(id);

        var sql = @"
            DELETE
            FROM    application.user
            WHERE   id = @id";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await connection.ExecuteAsync(sql, new { id });
    }

    public override async Task<User> GetByIdAsync(Guid id)
    {
        if (Cache.TryGetValue(id, out User? value))
        {
            return value ?? throw new EntityNotFoundException(nameof(User));
        }

        var sql = @"
            SELECT  -- User
                    u.id,
                    u.given_name,
                    u.last_name,
                    u.email,
                    u.job_title,
                    u.phone_number,
                    u.role
            FROM    application.user AS u
            WHERE   u.id = @id
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        var user = await connection.QuerySingleOrDefaultAsync<User>(sql, new { id })
            ?? throw new EntityNotFoundException(nameof(User));

        var options = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromHours(1))
            .SetAbsoluteExpiration(TimeSpan.FromHours(4));

        return Cache.Set(user.Id, user, options);
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        var sql = @"
            SELECT  -- User
                    u.id,
                    u.given_name,
                    u.last_name,
                    u.email,
                    u.job_title,
                    u.phone_number,
                    u.role
            FROM    application.user AS u
            WHERE   u.email = application.normalize2(@email)
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        var user = await connection.QuerySingleOrDefaultAsync<User>(sql, new { email })
            ?? throw new EntityNotFoundException(nameof(User));

        var options = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromHours(1))
            .SetAbsoluteExpiration(TimeSpan.FromHours(4));

        return Cache.Set(user.Id, user, options);
    }

    public async Task<User> GetByAuthKeyAsync(string key)
    {
        if (Cache.TryGetValue(key, out User? value))
        {
            return value ?? throw new EntityNotFoundException(nameof(User));
        }

        var sql = @"
            SELECT  -- User
                    u.id,
                    u.given_name,
                    u.last_name,
                    u.email,
                    u.job_title,
                    u.phone_number,
                    u.role
            FROM    application.user AS u
            JOIN    application.auth_key ak ON ak.user_id = u.id
            WHERE   ak.key = @key
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        var user = await connection.QuerySingleOrDefaultAsync<User>(sql, new { key })
            ?? throw new EntityNotFoundException(nameof(User));

        var options = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromHours(1))
            .SetAbsoluteExpiration(TimeSpan.FromHours(4));

        Cache.Set(key, user, options);

        return Cache.Set(user.Id, user, options);
    }

    public async Task<User> GetByResetKeyAsync(string email, Guid key)
    {
        var sql = @"
            SELECT  -- User
                    u.id,
                    u.given_name,
                    u.last_name,
                    u.email,
                    u.job_title,
                    u.phone_number,
                    u.role
            FROM    application.user AS u
            JOIN    application.reset_key rk ON rk.user_id = u.id
            WHERE   rk.key = @key
            AND     u.email = application.normalize2(@email)
            AND     rk.create_date > NOW() - INTERVAL '2 hours'
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        var user = await connection.QuerySingleOrDefaultAsync<User>(sql, new { email, key })
            ?? throw new EntityNotFoundException(nameof(User));

        var options = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromHours(1))
            .SetAbsoluteExpiration(TimeSpan.FromHours(4));

        return Cache.Set(user.Id, user, options);
    }

    public async Task<string?> GetPasswordHashAsync(Guid id)
    {
        var sql = @"
            SELECT  u.password_hash
            FROM    application.user AS u
            WHERE   u.id = @id
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.ExecuteScalarAsync<string>(sql, new { id });
    }

    public async Task<int> GetAccessFailedCount(Guid id)
    {
        var sql = @"
            SELECT  u.access_failed_count
            FROM    application.user AS u
            WHERE   id = @id
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.ExecuteScalarAsync<int>(sql, new { id });
    }

    public override async IAsyncEnumerable<User> ListAllAsync(Navigation navigation)
    {
        var sql = @"
            SELECT  -- User
                    u.id,
                    u.given_name,
                    u.last_name,
                    u.email,
                    u.job_title,
                    u.phone_number,
                    u.role
            FROM    application.user AS u";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await foreach (var item in connection.QueryUnbufferedAsync<User>(sql))
        {
            yield return item;
        }
    }

    public override async Task UpdateAsync(User entity)
    {
        ResetCacheEntity(entity);

        var sql = @"
            UPDATE  application.user
            SET     given_name = NULLIF(trim(@GivenName), ''),
                    last_name = NULLIF(trim(@LastName), ''),
                    job_title = NULLIF(trim(@JobTitle), ''),
                    phone_number = REGEXP_REPLACE(@PhoneNumber,'\D','','g')
            WHERE   id = @Id";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await connection.ExecuteAsync(sql, entity);
    }

    public async Task SetPasswordHashAsync(Guid id, string passwordHash)
    {
        var sql = @"
            UPDATE  application.user
            SET     password_hash = @password_hash
            WHERE   id = @id";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await connection.ExecuteAsync(sql, new { id, password_hash = passwordHash });
    }

    public async Task<Guid> CreateResetKeyAsync(Guid id)
    {
        var sql = @"
            INSERT INTO application.reset_key(user_id)
            VALUES (@id)
            RETURNING key";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.ExecuteScalarAsync<Guid>(sql, new { id });
    }

    public async Task BumpAccessFailed(Guid id)
    {
        var sql = @"
            UPDATE  application.user
            SET     access_failed_count = access_failed_count + 1
            WHERE   id = @id";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await connection.ExecuteAsync(sql, new { id });
    }

    public async Task ResetAccessFailed(Guid id)
    {
        var sql = @"
            UPDATE  application.user
            SET     access_failed_count = 0
            WHERE   id = @id";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await connection.ExecuteAsync(sql, new { id });
    }

    public async Task ResetResetKey(Guid id)
    {
        var sql = @"
            DELETE FROM application.reset_key
            WHERE   user_id = @id";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await connection.ExecuteAsync(sql, new { id });
    }

    public async Task RegisterAccess(Guid id)
    {
        var sql = @"SELECT application.log_access(@id)";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await connection.ExecuteScalarAsync(sql, new { id });
    }
}
