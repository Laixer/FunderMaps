using Dapper;
using FunderMaps.Core;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data.Abstractions;
using System.Security.Cryptography;
using System.Text;

namespace FunderMaps.Data.Repositories;

internal class UserRepository : DbServiceBase, IUserRepository
{
    public async Task<Guid> AddAsync(User entity)
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
                lower(trim(@Email)),
                NULLIF(trim(@JobTitle), ''),
                REGEXP_REPLACE(@PhoneNumber,'\D','','g'),
                @Role)
            RETURNING id";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.ExecuteScalarAsync<Guid>(sql, entity);
    }

    public async Task<long> CountAsync()
    {
        var sql = @"
            SELECT  COUNT(*)
            FROM    application.user";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.ExecuteScalarAsync<long>(sql);
    }

    // FUTURE: If user is in use it violates foreign key constraint, returning
    //         a ReferenceNotFoundException, which is invalid.
    public async Task DeleteAsync(Guid id)
    {
        var sql = @"
            DELETE
            FROM    application.user
            WHERE   id = @id";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await connection.ExecuteAsync(sql, new { id });
    }

    public async Task<User> GetByIdAsync(Guid id)
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
            WHERE   u.id = @id
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.QuerySingleOrDefaultAsync<User>(sql, new { id })
            ?? throw new EntityNotFoundException(nameof(User));
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
            WHERE   u.email = lower(trim(@email))
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.QuerySingleOrDefaultAsync<User>(sql, new { email })
            ?? throw new EntityNotFoundException(nameof(User));
    }

    public async Task<User> GetByAuthKeyAsync(string key)
    {
        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        var keyHash = Sha256Hex(key);
        var user = await connection.QuerySingleOrDefaultAsync<User>(@"
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
            WHERE   ak.key_hash = @keyHash
            LIMIT   1", new { keyHash });

        return user ?? throw new EntityNotFoundException(nameof(User));
    }

    private static string Sha256Hex(string input) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(input)))
            .ToLowerInvariant();

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
            AND     u.email = lower(trim(@email))
            AND     rk.create_date > NOW() - INTERVAL '2 hours'
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.QuerySingleOrDefaultAsync<User>(sql, new { email, key })
            ?? throw new EntityNotFoundException(nameof(User));
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

    public async IAsyncEnumerable<User> ListAllAsync(Navigation navigation)
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

    public async Task UpdateAsync(User entity)
    {
        var sql = @"
            UPDATE  application.user
            SET     given_name = @GivenName,
                    last_name = @LastName,
                    job_title = @JobTitle,
                    phone_number = @PhoneNumber
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
        var sql = @"
            UPDATE  application.user
            SET     last_login = CURRENT_TIMESTAMP
            WHERE   id = @id";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await connection.ExecuteAsync(sql, new { id });
    }
}
