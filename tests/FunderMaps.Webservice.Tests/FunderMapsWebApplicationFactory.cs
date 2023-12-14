using FunderMaps.Core;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace FunderMaps.Webservice.Tests;

internal class MemoryUserRepository(IPasswordHasher passwordHasher) : IUserRepository
{
    /// <summary>
    ///     Create new <see cref="User"/>.
    /// </summary>
    /// <param name="entity">Entity object.</param>
    /// <returns>Created <see cref="User"/>.</returns>
    public async Task<Guid> AddAsync(User entity)
    {
        await Task.CompletedTask;

        // var entityName = EntityTable("application");

        // var sql = @$"
        //     INSERT INTO {entityName} (
        //         given_name,
        //         last_name,
        //         email,
        //         avatar,
        //         job_title,
        //         phone_number,
        //         role)
        //     VALUES (
        //         @given_name,
        //         @last_name,
        //         @email,
        //         @avatar,
        //         NULLIF(trim(@job_title), ''),
        //         REGEXP_REPLACE(@phone_number,'\D','','g'),
        //         @role)
        //     RETURNING id";

        // await using var context = await DbContextFactory.CreateAsync(sql);

        // MapToWriter(context, entity);

        // await using var reader = await context.ReaderAsync();

        // return reader.GetGuid(0);
        throw new NotImplementedException();
    }

    /// <summary>
    ///     Retrieve number of entities.
    /// </summary>
    /// <returns>Number of entities.</returns>
    public async Task<long> CountAsync()
    {
        // var sql = @"
        //     SELECT  COUNT(*)
        //     FROM    application.user";

        // await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        // return await connection.ExecuteScalarAsync<long>(sql);
        throw new NotImplementedException();
    }

    // FUTURE: If user is in use it violates foreign key constraint, returning
    //         a ReferenceNotFoundException, which is invalid.
    /// <summary>
    ///     Delete <see cref="User"/>.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    public async Task DeleteAsync(Guid id)
    {
        // ResetCacheEntity(id);

        // var sql = @"
        //     DELETE
        //     FROM    application.user
        //     WHERE   id = @id";

        // await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        // await connection.ExecuteAsync(sql, new { id });
        throw new NotImplementedException();
    }

    /// <summary>
    ///     Retrieve <see cref="User"/> by id.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <returns><see cref="User"/>.</returns>
    public async Task<User> GetByIdAsync(Guid id)
    {
        // if (TryGetEntity(id, out User? entity))
        // {
        //     return entity ?? throw new InvalidOperationException();
        // }

        // var sql = @"
        //     SELECT  -- User
        //             u.id,
        //             u.given_name,
        //             u.last_name,
        //             u.email,
        //             u.avatar,
        //             u.job_title,
        //             u.phone_number,
        //             u.role
        //     FROM    application.user AS u
        //     WHERE   u.id = @id
        //     LIMIT   1";

        // await using var context = await DbContextFactory.CreateAsync(sql);

        // context.AddParameterWithValue("id", id);

        // await using var reader = await context.ReaderAsync();

        // return CacheEntity(MapFromReader(reader));

        throw new NotImplementedException();
    }

    /// <summary>
    ///     Retrieve <see cref="User"/> by email and password hash.
    /// </summary>
    /// <param name="email">Unique identifier.</param>
    /// <returns><see cref="User"/>.</returns>
    public async Task<User> GetByEmailAsync(string email)
    {
        await Task.CompletedTask;

        return new()
        {
            Id = Guid.NewGuid(),
            GivenName = "John",
            LastName = "Doe",
            Email = "javier40@yahoo.com",
            Avatar = "https://www.gravatar.com/avatar/205e460b479e2e5b48aec07710c08d50",
            JobTitle = "Developer",
            PhoneNumber = "+31612345678",
            Role = ApplicationRole.Administrator,
        };
    }

    /// <summary>
    ///     Retrieve <see cref="User"/> by authentication key.
    /// </summary>
    /// <param name="key">Authentication key.</param>
    /// <returns><see cref="User"/>.</returns>
    public async Task<User> GetByAuthKeyAsync(string key)
    {
        // var sql = @"
        //     SELECT  -- User
        //             u.id,
        //             u.given_name,
        //             u.last_name,
        //             u.email,
        //             u.avatar,
        //             u.job_title,
        //             u.phone_number,
        //             u.role
        //     FROM    application.user AS u
        //     JOIN    application.auth_key ak ON ak.user_id = u.id
        //     WHERE   ak.key = @key
        //     LIMIT   1";

        // await using var context = await DbContextFactory.CreateAsync(sql);

        // context.AddParameterWithValue("key", key);

        // await using var reader = await context.ReaderAsync();

        // return CacheEntity(MapFromReader(reader));
        throw new NotImplementedException();
    }

    /// <summary>
    ///     Get password hash.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <returns>Password hash as string.</returns>
    public async Task<string?> GetPasswordHashAsync(Guid id)
    {
        await Task.CompletedTask;

        var pwd = passwordHasher.HashPassword("fundermaps");
        return pwd;

        // var sql = @"
        //     SELECT  u.password_hash
        //     FROM    application.user AS u
        //     WHERE   u.id = @id
        //     LIMIT   1";

        // await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        // return await connection.ExecuteScalarAsync<string>(sql, new { id });
        // throw new NotImplementedException();
    }

    /// <summary>
    ///     Get access failed count.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <returns>Failed access count.</returns>
    public async Task<int> GetAccessFailedCount(Guid id)
    {
        await Task.CompletedTask;

        return 0;
    }

    /// <summary>
    ///     Retrieve all <see cref="User"/>.
    /// </summary>
    /// <returns>List of <see cref="User"/>.</returns>
    public async IAsyncEnumerable<User> ListAllAsync(Navigation navigation)
    {
        yield break;

        // var sql = @"
        //     SELECT  -- User
        //             u.id,
        //             u.given_name,
        //             u.last_name,
        //             u.email,
        //             u.avatar,
        //             u.job_title,
        //             u.phone_number,
        //             u.role
        //     FROM    application.user AS u";

        // await using var context = await DbContextFactory.CreateAsync(sql);

        // await foreach (var reader in context.EnumerableReaderAsync())
        // {
        //     yield return CacheEntity(MapFromReader(reader));
        // }
        throw new NotImplementedException();
    }

    /// <summary>
    ///     Update <see cref="User"/>.
    /// </summary>
    /// <param name="entity">Entity object.</param>
    public async Task UpdateAsync(User entity)
    {
        // ResetCacheEntity(entity);

        // var sql = @"
        //     UPDATE  application.user
        //     SET     given_name = @given_name,
        //             last_name = @last_name,
        //             avatar = @avatar,
        //             job_title = NULLIF(trim(@job_title), ''),
        //             phone_number = REGEXP_REPLACE(@phone_number,'\D','','g'),
        //             role = @role
        //     WHERE   id = @id";

        // await using var context = await DbContextFactory.CreateAsync(sql);

        // context.AddParameterWithValue("id", entity.Id);

        // MapToWriter(context, entity);

        // await context.NonQueryAsync();
        throw new NotImplementedException();
    }

    /// <summary>
    ///     Update user password.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="passwordHash">New password hash.</param>
    public async Task SetPasswordHashAsync(Guid id, string passwordHash)
    {
        // var sql = @"
        //     UPDATE  application.user
        //     SET     password_hash = @password_hash
        //     WHERE   id = @id";

        // await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        // await connection.ExecuteAsync(sql, new { id, password_hash = passwordHash });

        throw new NotImplementedException();
    }

    /// <summary>
    ///     Increase signin failure count.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    public async Task BumpAccessFailed(Guid id)
    {
        // var sql = @"
        //     UPDATE  application.user
        //     SET     access_failed_count = access_failed_count + 1
        //     WHERE   id = @id";

        // await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        // await connection.ExecuteAsync(sql, new { id });

        throw new NotImplementedException();
    }

    /// <summary>
    ///     Reset signin failure count.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    public async Task ResetAccessFailed(Guid id)
    {
        await Task.CompletedTask;

        // var sql = @"
        //     UPDATE  application.user
        //     SET     access_failed_count = 0
        //     WHERE   id = @id";

        // await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        // await connection.ExecuteAsync(sql, new { id });

        // throw new NotImplementedException();
    }

    /// <summary>
    ///     Register a new user login.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    public async Task RegisterAccess(Guid id)
    {
        await Task.CompletedTask;

        // var sql = @"SELECT application.log_access(@id)";

        // await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        // await connection.ExecuteScalarAsync(sql, new { id });

        // throw new NotImplementedException();
    }
}

/// <summary>
///     Organization user repository.
/// </summary>
internal class MemoryOrganizationUserRepository : IOrganizationUserRepository
{
    public async Task AddAsync(Guid organizationId, Guid userId, OrganizationRole role)
    {
        // var sql = @"
        //     INSERT INTO application.organization_user(
        //         user_id,
        //         organization_id,
        //         role)
        //     VALUES (
        //         @user_id,
        //         @organization_id,
        //         @role)";

        // await using var context = await DbContextFactory.CreateAsync(sql);

        // context.AddParameterWithValue("user_id", userId);
        // context.AddParameterWithValue("organization_id", organizationId);
        // context.AddParameterWithValue("role", role);

        // await context.NonQueryAsync();

        await Task.CompletedTask;
    }

    /// <summary>
    ///     Retrieve all users by organization.
    /// </summary>
    /// <returns>List of user identifiers.</returns>
    public async IAsyncEnumerable<OrganizationUser> ListAllAsync(Guid organizationId, Navigation navigation)
    {
        // var sql = @"
        //     SELECT
        //             u.id,
        //             u.given_name,
        //             u.last_name,
        //             u.email,
        //             u.avatar,
        //             u.job_title,
        //             u.phone_number,
        //             u.role,
        //             ou.role AS organization_role
        //     FROM   application.user u
        //     JOIN   application.organization_user ou ON ou.user_id = u.id
        //     WHERE  ou.organization_id = @organization_id";

        // await using var context = await DbContextFactory.CreateAsync(sql);

        // context.AddParameterWithValue("organization_id", organizationId);

        // await foreach (var reader in context.EnumerableReaderAsync())
        // {
        //     yield return new()
        //     {
        //         Id = reader.GetGuid(0),
        //         GivenName = reader.GetSafeString(1),
        //         LastName = reader.GetSafeString(2),
        //         Email = reader.GetString(3),
        //         Avatar = reader.GetSafeString(4),
        //         JobTitle = reader.GetSafeString(5),
        //         PhoneNumber = reader.GetSafeString(6),
        //         Role = reader.GetFieldValue<ApplicationRole>(7),
        //         OrganizationRole = reader.GetFieldValue<OrganizationRole>(8),
        //     };
        // }

        yield return new()
        {
            Id = Guid.NewGuid(),
            GivenName = "John",
            LastName = "Doe",
            Email = "Javier40@yahoo.com"
        };
    }

    public async IAsyncEnumerable<Guid> ListAllByRoleAsync(Guid organizationId, OrganizationRole[] role, Navigation navigation)
    {
        // var sql = @"
        //     SELECT  user_id
        //     FROM    application.organization_user
        //     WHERE   organization_id = @organization_id
        //     AND     role = ANY(@role)";

        // await using var context = await DbContextFactory.CreateAsync(sql);

        // context.AddParameterWithValue("organization_id", organizationId);
        // context.AddParameterWithValue("role", role);

        // await foreach (var reader in context.EnumerableReaderAsync())
        // {
        //     yield return reader.GetGuid(0);
        // }

        // await Task.CompletedTask;

        yield return Guid.NewGuid();
    }

    public async Task<bool> IsUserInOrganization(Guid organizationId, Guid userId)
    {
        // var sql = @"
        //     SELECT EXISTS (
        //         SELECT  1
        //         FROM    application.organization_user
        //         WHERE   user_id = @user_id
        //         AND     organization_id = @organization_id
        //         LIMIT   1
        //     )";

        // await using var context = await DbContextFactory.CreateAsync(sql);

        // context.AddParameterWithValue("user_id", userId);
        // context.AddParameterWithValue("organization_id", organizationId);

        // return await context.ScalarAsync<bool>();

        await Task.CompletedTask;

        return true;
    }

    public IAsyncEnumerable<Guid> ListAllOrganizationIdByUserIdAsync(Guid userId)
    {
        // var sql = @"
        //     SELECT  organization_id
        //     FROM    application.organization_user
        //     WHERE   user_id = @user_id";

        // await using var context = await DbContextFactory.CreateAsync(sql);

        // context.AddParameterWithValue("user_id", userId);

        // await foreach (var reader in context.EnumerableReaderAsync())
        // {
        //     yield return reader.GetGuid(0);
        // }

        Guid[] staticArray = [Guid.NewGuid()];

        return staticArray.ToAsyncEnumerable();

        // yield return Guid.NewGuid();
    }

    public async Task<OrganizationRole?> GetOrganizationRoleByUserIdAsync(Guid userId, Guid organizationId)
    {
        // var sql = @"
        //     SELECT  role
        //     FROM    application.organization_user
        //     WHERE   user_id = @user_id
        //     AND     organization_id = @organization_id";

        // await using var context = await DbContextFactory.CreateAsync(sql);

        // context.AddParameterWithValue("user_id", userId);
        // context.AddParameterWithValue("organization_id", organizationId);

        // await using var reader = await context.ReaderAsync();

        // return reader.GetSafeStructValue<OrganizationRole>(0);

        await Task.CompletedTask;

        return OrganizationRole.Reader;
    }

    // TODO: Also request the organization for which this role must be set.
    public async Task SetOrganizationRoleByUserIdAsync(Guid userId, OrganizationRole role)
    {
        // var sql = @"
        //     UPDATE  application.organization_user
        //     SET     role = @role
        //     WHERE   user_id = @user_id";

        // await using var context = await DbContextFactory.CreateAsync(sql);

        // context.AddParameterWithValue("user_id", userId);
        // context.AddParameterWithValue("role", role);

        // await context.NonQueryAsync();

        await Task.CompletedTask;
    }
}

public class FunderMapsWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // services.AddScoped<IAddressRepository, AddressRepository>();
            // services.AddScoped<IAnalysisRepository, AnalysisRepository>();
            // services.AddScoped<IBuildingRepository, BuildingRepository>();
            // services.AddScoped<IBundleRepository, BundleRepository>();
            // services.AddScoped<IContractorRepository, ContractorRepository>();
            // services.AddScoped<IIncidentRepository, IncidentRepository>();
            // services.AddScoped<IInquiryRepository, InquiryRepository>();
            // services.AddScoped<IInquirySampleRepository, InquirySampleRepository>();
            // services.AddScoped<IKeystoreRepository, KeystoreRepository>();
            // services.AddScoped<IMapsetRepository, MapsetRepository>();
            // services.AddScoped<INeighborhoodRepository, NeighborhoodRepository>();
            // services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            // services.AddScoped<IOrganizationUserRepository, OrganizationUserRepository>();
            // services.AddScoped<IRecoveryRepository, RecoveryRepository>();
            // services.AddScoped<IRecoverySampleRepository, RecoverySampleRepository>();
            // services.AddScoped<IStatisticsRepository, StatisticsRepository>();
            // services.AddScoped<ITestRepository, TestRepository>();
            // services.AddScoped<ITelemetryRepository, TelemetryRepository>();

            services.Replace(ServiceDescriptor.Scoped<IOrganizationUserRepository, MemoryOrganizationUserRepository>());
            services.Replace(ServiceDescriptor.Scoped<IUserRepository, MemoryUserRepository>());
        });

        // builder.UseEnvironment("Development");
    }
}