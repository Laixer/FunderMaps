using Dapper;
using FunderMaps.Core;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;

namespace FunderMaps.Data.Repositories;

/// <summary>
///     User repository.
/// </summary>
internal class UserRepository : RepositoryBase<User, Guid>, IUserRepository
{
    // TODO: Move NULLIF and REGEXP_REPLACE to UserService.
    /// <summary>
    ///     Create new <see cref="User"/>.
    /// </summary>
    /// <param name="entity">Entity object.</param>
    /// <returns>Created <see cref="User"/>.</returns>
    public override async Task<Guid> AddAsync(User entity)
    {
        var entityName = EntityTable("application");

        var sql = @$"
            INSERT INTO {entityName} (
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

    /// <summary>
    ///     Retrieve number of entities.
    /// </summary>
    /// <returns>Number of entities.</returns>
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
    /// <summary>
    ///     Delete <see cref="User"/>.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
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

    /// <summary>
    ///     Retrieve <see cref="User"/> by id.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <returns><see cref="User"/>.</returns>
    public override async Task<User> GetByIdAsync(Guid id)
    {
        if (TryGetEntity(id, out User? entity))
        {
            return entity ?? throw new InvalidOperationException();
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

        var user = await connection.QuerySingleOrDefaultAsync<User>(sql, new { id });
        return user is null ? throw new EntityNotFoundException(nameof(User)) : CacheEntity(user);
    }

    /// <summary>
    ///     Retrieve <see cref="User"/> by email and password hash.
    /// </summary>
    /// <param name="email">Unique identifier.</param>
    /// <returns><see cref="User"/>.</returns>
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

        var user = await connection.QuerySingleOrDefaultAsync<User>(sql, new { email });
        return user is null ? throw new EntityNotFoundException(nameof(User)) : CacheEntity(user);
    }

    /// <summary>
    ///     Retrieve <see cref="User"/> by authentication key.
    /// </summary>
    /// <param name="key">Authentication key.</param>
    /// <returns><see cref="User"/>.</returns>
    public async Task<User> GetByAuthKeyAsync(string key)
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
            JOIN    application.auth_key ak ON ak.user_id = u.id
            WHERE   ak.key = @key
            LIMIT   1";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        var user = await connection.QuerySingleOrDefaultAsync<User>(sql, new { key });
        return user is null ? throw new EntityNotFoundException(nameof(User)) : CacheEntity(user);
    }

    /// <summary>
    ///     Retrieve <see cref="User"/> by password reset key.
    /// </summary>
    /// <param name="email">User email.</param>
    /// <param name="key">Authentication key.</param>
    /// <returns><see cref="User"/>.</returns>
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

        var user = await connection.QuerySingleOrDefaultAsync<User>(sql, new { key, email });
        return user is null ? throw new EntityNotFoundException(nameof(User)) : CacheEntity(user);
    }

    /// <summary>
    ///     Get password hash.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <returns>Password hash as string.</returns>
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

    /// <summary>
    ///     Get access failed count.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <returns>Failed access count.</returns>
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

    /// <summary>
    ///     Retrieve all <see cref="User"/>.
    /// </summary>
    /// <returns>List of <see cref="User"/>.</returns>
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
            yield return CacheEntity(item);
        }
    }

    // FUTURE: Move 'Role' cannot be changed here.
    /// <summary>
    ///     Update <see cref="User"/>.
    /// </summary>
    /// <param name="entity">Entity object.</param>
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

    /// <summary>
    ///     Update user password.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="passwordHash">New password hash.</param>
    public async Task SetPasswordHashAsync(Guid id, string passwordHash)
    {
        var sql = @"
            UPDATE  application.user
            SET     password_hash = @password_hash
            WHERE   id = @id";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await connection.ExecuteAsync(sql, new { id, password_hash = passwordHash });
    }

    /// <summary>
    ///    Create a new password reset key.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    public async Task<Guid> CreateResetKeyAsync(Guid id)
    {
        var sql = @"
            INSERT INTO application.reset_key(user_id)
            VALUES (@id)
            RETURNING key";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.ExecuteScalarAsync<Guid>(sql, new { id });
    }

    /// <summary>
    ///     Increase signin failure count.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    public async Task BumpAccessFailed(Guid id)
    {
        var sql = @"
            UPDATE  application.user
            SET     access_failed_count = access_failed_count + 1
            WHERE   id = @id";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await connection.ExecuteAsync(sql, new { id });
    }

    /// <summary>
    ///     Reset signin failure count.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    public async Task ResetAccessFailed(Guid id)
    {
        var sql = @"
            UPDATE  application.user
            SET     access_failed_count = 0
            WHERE   id = @id";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await connection.ExecuteAsync(sql, new { id });
    }

    /// <summary>
    ///     Reset password reset key.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    public async Task ResetResetKey(Guid id)
    {
        var sql = @"
            DELETE FROM application.reset_key
            WHERE   user_id = @id";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await connection.ExecuteAsync(sql, new { id });
    }

    /// <summary>
    ///     Register a new user login.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    public async Task RegisterAccess(Guid id)
    {
        var sql = @"SELECT application.log_access(@id)";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await connection.ExecuteScalarAsync(sql, new { id });
    }
}
