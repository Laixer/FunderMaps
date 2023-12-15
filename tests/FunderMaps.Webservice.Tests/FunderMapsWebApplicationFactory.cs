using FunderMaps.Core;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Services;
using FunderMaps.Core.Types;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit.Sdk;

namespace FunderMaps.Webservice.Tests;

internal class MemoryDatabase<TEntity, TEntityPrimaryKey>
    where TEntity : IEntityIdentifier<TEntityPrimaryKey>
{
    private readonly Dictionary<TEntityPrimaryKey, TEntity> memory = [];

    public async Task<TEntityPrimaryKey> AddAsync(TEntity entity)
    {
        await Task.CompletedTask;

        memory.Add(entity.Identifier, entity);

        return entity.Identifier;
    }

    public async Task<long> CountAsync()
    {
        await Task.CompletedTask;

        return memory.Count;
    }

    public async Task DeleteAsync(TEntityPrimaryKey id)
    {
        await Task.CompletedTask;

        memory.Remove(id);
    }

    public async Task<TEntity> GetByIdAsync(TEntityPrimaryKey id)
    {
        await Task.CompletedTask;

        return memory[id];
    }
}

// TODO: Maybe this should be a type or a model.
/// <summary>
///     User entity.
/// </summary>
public sealed class UserExtended : User
{
    public string? PasswordHash { get; set; }

    public int AccessFailedCount { get; set; }

    public int LoginCount { get; set; }

    /// <summary>
    ///     User role in organization.
    /// </summary>
    // [Required]
    public OrganizationRole OrganizationRole { get; set; }
}


internal class MemoryUserRepository(PasswordHasher passwordHasher) : IUserRepository
{
    private readonly Dictionary<Guid, UserExtended> memory = new()
    {
        [Guid.Parse("07a86d13-1c02-46ab-a2a8-c2342f829872")] = new()
        {
            Id = Guid.Parse("07a86d13-1c02-46ab-a2a8-c2342f829872"),
            GivenName = "Lester",
            LastName = "Bednar",
            Email = "lester@contoso.com",
            JobTitle = "Developer",
            PhoneNumber = "+31612345678",
            Role = ApplicationRole.User,
            PasswordHash = passwordHasher.HashPassword("fundermaps"),
            AccessFailedCount = 0,
            LoginCount = 0,
            OrganizationRole = OrganizationRole.Reader,
        },
    };

    /// <summary>
    ///     Create new <see cref="User"/>.
    /// </summary>
    /// <param name="entity">Entity object.</param>
    /// <returns>Created <see cref="User"/>.</returns>
    public async Task<Guid> AddAsync(User entity)
    {
        await Task.CompletedTask;

        memory.Add(entity.Id, new()
        {
            Id = entity.Id,
            GivenName = entity.GivenName,
            LastName = entity.LastName,
            Email = entity.Email,
            Avatar = entity.Avatar,
            JobTitle = entity.JobTitle,
            PhoneNumber = entity.PhoneNumber,
            Role = entity.Role,
            PasswordHash = string.Empty,
            AccessFailedCount = 0,
            LoginCount = 0,
            OrganizationRole = OrganizationRole.Reader,
        });

        return entity.Id;

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
    }

    /// <summary>
    ///     Retrieve number of entities.
    /// </summary>
    /// <returns>Number of entities.</returns>
    public async Task<long> CountAsync()
    {
        await Task.CompletedTask;

        return memory.Count;
    }

    // FUTURE: If user is in use it violates foreign key constraint, returning
    //         a ReferenceNotFoundException, which is invalid.
    /// <summary>
    ///     Delete <see cref="User"/>.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    public async Task DeleteAsync(Guid id)
    {
        await Task.CompletedTask;

        memory.Remove(id);
    }

    /// <summary>
    ///     Retrieve <see cref="User"/> by id.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <returns><see cref="User"/>.</returns>
    public async Task<User> GetByIdAsync(Guid id)
    {
        await Task.CompletedTask;

        return memory[id];
    }

    /// <summary>
    ///     Retrieve <see cref="User"/> by email and password hash.
    /// </summary>
    /// <param name="email">Unique identifier.</param>
    /// <returns><see cref="User"/>.</returns>
    public async Task<User> GetByEmailAsync(string email)
    {
        await Task.CompletedTask;

        return memory.Values.FirstOrDefault(x => x.Email == email) ?? throw new EntityNotFoundException(nameof(User));
    }

    /// <summary>
    ///     Retrieve <see cref="User"/> by authentication key.
    /// </summary>
    /// <param name="key">Authentication key.</param>
    /// <returns><see cref="User"/>.</returns>
    public async Task<User> GetByAuthKeyAsync(string key)
    {
        await Task.CompletedTask;

        // TODO: Implement

        return memory.Values.FirstOrDefault(x => x.Email == key) ?? throw new EntityNotFoundException(nameof(User));
    }

    /// <summary>
    ///     Get password hash.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <returns>Password hash as string.</returns>
    public async Task<string?> GetPasswordHashAsync(Guid id)
    {
        await Task.CompletedTask;

        return memory[id].PasswordHash;

        // return passwordHasher.HashPassword("fundermaps");

        // var sql = @"
        //     SELECT  u.password_hash
        //     FROM    application.user AS u
        //     WHERE   u.id = @id
        //     LIMIT   1";
    }

    /// <summary>
    ///     Get access failed count.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <returns>Failed access count.</returns>
    public async Task<int> GetAccessFailedCount(Guid id)
    {
        await Task.CompletedTask;

        return memory[id].AccessFailedCount;
    }

    /// <summary>
    ///     Retrieve all <see cref="User"/>.
    /// </summary>
    /// <returns>List of <see cref="User"/>.</returns>
    public IAsyncEnumerable<User> ListAllAsync(Navigation navigation)
    {
        return memory.Values.ToAsyncEnumerable();
    }

    /// <summary>
    ///     Update <see cref="User"/>.
    /// </summary>
    /// <param name="entity">Entity object.</param>
    public async Task UpdateAsync(User entity)
    {
        await Task.CompletedTask;

        memory[entity.Id] = new()
        {
            Id = entity.Id,
            GivenName = entity.GivenName,
            LastName = entity.LastName,
            Email = entity.Email,
            Avatar = entity.Avatar,
            JobTitle = entity.JobTitle,
            PhoneNumber = entity.PhoneNumber,
            Role = entity.Role,
            PasswordHash = string.Empty,
            AccessFailedCount = 0,
            LoginCount = 0,
            OrganizationRole = OrganizationRole.Reader,
        };
    }

    /// <summary>
    ///     Update user password.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="passwordHash">New password hash.</param>
    public async Task SetPasswordHashAsync(Guid id, string passwordHash)
    {
        await Task.CompletedTask;

        memory[id].PasswordHash = passwordHash;

        // var sql = @"
        //     UPDATE  application.user
        //     SET     password_hash = @password_hash
        //     WHERE   id = @id";
    }

    /// <summary>
    ///     Increase signin failure count.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    public async Task BumpAccessFailed(Guid id)
    {
        await Task.CompletedTask;

        memory[id].AccessFailedCount++;

        // var sql = @"
        //     UPDATE  application.user
        //     SET     access_failed_count = access_failed_count + 1
        //     WHERE   id = @id";
    }

    /// <summary>
    ///     Reset signin failure count.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    public async Task ResetAccessFailed(Guid id)
    {
        await Task.CompletedTask;

        memory[id].AccessFailedCount = 0;

        // var sql = @"
        //     UPDATE  application.user
        //     SET     access_failed_count = 0
        //     WHERE   id = @id";
    }

    /// <summary>
    ///     Register a new user login.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    public async Task RegisterAccess(Guid id)
    {
        await Task.CompletedTask;

        // var sql = @"SELECT application.log_access(@id)";

        memory[id].LoginCount++;
    }
}

/// <summary>
///     Organization user repository.
/// </summary>
internal class MemoryOrganizationUserRepository : IOrganizationUserRepository
{
    private readonly Dictionary<Guid, User> memory = [];

    public async Task AddAsync(Guid organizationId, Guid userId, OrganizationRole role)
    {
        await Task.CompletedTask;

        // var sql = @"
        //     INSERT INTO application.organization_user(
        //         user_id,
        //         organization_id,
        //         role)
        //     VALUES (
        //         @user_id,
        //         @organization_id,
        //         @role)";
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

/// <summary>
///     Construct new instance.
/// </summary>
public class MemoryKeystoreXmlRepository : Microsoft.AspNetCore.DataProtection.Repositories.IXmlRepository
{
    private readonly Dictionary<string, System.Xml.Linq.XElement> memory = [];

    public IReadOnlyCollection<System.Xml.Linq.XElement> GetAllElements()
    {
        return memory.Values.ToList().AsReadOnly();
    }

    public void StoreElement(System.Xml.Linq.XElement element, string friendlyName)
    {
        memory.Add(friendlyName, element);
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
            // services.AddScoped<IMapsetRepository, MapsetRepository>();
            // services.AddScoped<INeighborhoodRepository, NeighborhoodRepository>();
            // services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            // services.AddScoped<IRecoveryRepository, RecoveryRepository>();
            // services.AddScoped<IRecoverySampleRepository, RecoverySampleRepository>();
            // services.AddScoped<IStatisticsRepository, StatisticsRepository>();
            // services.AddScoped<ITestRepository, TestRepository>();
            // services.AddScoped<ITelemetryRepository, TelemetryRepository>();

            // services.Replace(ServiceDescriptor.Scoped<IKeystoreRepository, MemoryKeystoreRepository>());
            services.Replace(ServiceDescriptor.Scoped<IOrganizationUserRepository, MemoryOrganizationUserRepository>());
            services.Replace(ServiceDescriptor.Scoped<IUserRepository, MemoryUserRepository>());

            services.Configure<Microsoft.AspNetCore.DataProtection.KeyManagement.KeyManagementOptions>(options =>
            {
                options.XmlRepository = new MemoryKeystoreXmlRepository();
            });
        });
    }
}
