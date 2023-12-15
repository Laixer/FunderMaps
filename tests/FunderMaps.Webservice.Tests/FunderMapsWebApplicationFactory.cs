using FunderMaps.Core;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Services;
using FunderMaps.Core.Types;
using FunderMaps.Core.Types.Products;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace FunderMaps.Webservice.Tests;

internal class MemoryRepositoryBase<TEntity, TEntityPrimaryKey> : IAsyncRepository<TEntity, TEntityPrimaryKey>
    where TEntity : IEntityIdentifier<TEntityPrimaryKey>
{
    protected readonly Dictionary<TEntityPrimaryKey, TEntity> memory = [];

    public Task<TEntityPrimaryKey> AddAsync(TEntity entity)
    {
        memory.Add(entity.Identifier, entity);

        return Task.FromResult(entity.Identifier);
    }

    public Task<long> CountAsync()
        => Task.FromResult((long)memory.Count);

    public Task DeleteAsync(TEntityPrimaryKey id)
    {
        memory.Remove(id);

        return Task.CompletedTask;
    }

    public Task<TEntity> GetByIdAsync(TEntityPrimaryKey id)
        => Task.FromResult(memory[id]);

    public IAsyncEnumerable<TEntity> ListAllAsync(Navigation navigation)
        => memory.Values.ToAsyncEnumerable();

    public Task UpdateAsync(TEntity entity)
    {
        memory[entity.Identifier] = entity;

        return Task.CompletedTask;
    }
}

public sealed class UserExtended : User
{
    public string? PasswordHash { get; set; }
    public string? AuthKey { get; set; }
    public int AccessFailedCount { get; set; } = 0;
    public int LoginCount { get; set; } = 0;
    public OrganizationRole OrganizationRole { get; set; } = OrganizationRole.Reader;
}

internal class MemoryUserRepository : MemoryRepositoryBase<UserExtended, Guid>, IUserRepository
{
    public MemoryUserRepository(PasswordHasher passwordHasher)
    {
        memory.Add(Guid.Parse("c85e80f3-0ba9-481a-9a69-eb43794e1894"), new()
        {
            Id = Guid.Parse("c85e80f3-0ba9-481a-9a69-eb43794e1894"),
            GivenName = "Administrator",
            Email = "admin@fundermaps.com",
            JobTitle = "Administrator",
            PhoneNumber = "+31612345678",
            Role = ApplicationRole.Administrator,
            PasswordHash = passwordHasher.HashPassword("fundermaps"),
        });
        memory.Add(Guid.Parse("648f3fa6-d74a-4b82-b981-c1f2d30f4077"), new()
        {
            Id = Guid.Parse("648f3fa6-d74a-4b82-b981-c1f2d30f4077"),
            Email = "javier40@yahoo.com",
            JobTitle = "Superuser",
            PhoneNumber = "+31612345678",
            Role = ApplicationRole.User,
            PasswordHash = passwordHasher.HashPassword("fundermaps"),
        });
        memory.Add(Guid.Parse("8b0b6d53-3418-41c4-bad2-908288b421c7"), new()
        {
            Id = Guid.Parse("8b0b6d53-3418-41c4-bad2-908288b421c7"),
            GivenName = "kihn",
            Email = "freda@contoso.com",
            JobTitle = "Reviewer",
            PhoneNumber = "+31612345678",
            Role = ApplicationRole.User,
            PasswordHash = passwordHasher.HashPassword("fundermaps"),
        });
        memory.Add(Guid.Parse("5415f7f7-72ec-4e3f-b2f9-68d7c9ee2868"), new()
        {
            Id = Guid.Parse("5415f7f7-72ec-4e3f-b2f9-68d7c9ee2868"),
            GivenName = "Patsy",
            LastName = "Brekke",
            Email = "patsy@contoso.com",
            JobTitle = "Writer",
            PhoneNumber = "+31612345678",
            Role = ApplicationRole.User,
            PasswordHash = passwordHasher.HashPassword("fundermaps"),
        });
        memory.Add(Guid.Parse("07a86d13-1c02-46ab-a2a8-c2342f829872"), new()
        {
            Id = Guid.Parse("07a86d13-1c02-46ab-a2a8-c2342f829872"),
            GivenName = "Lester",
            LastName = "Bednar",
            Email = "lester@contoso.com",
            JobTitle = "Reader",
            PhoneNumber = "+31612345678",
            Role = ApplicationRole.User,
            PasswordHash = passwordHasher.HashPassword("fundermaps"),
        });
        memory.Add(Guid.Parse("81209a70-08d9-42da-8ce7-1922fc63cbaf"), new()
        {
            Id = Guid.Parse("81209a70-08d9-42da-8ce7-1922fc63cbaf"),
            Email = "corene@contoso.com",
            JobTitle = "Reader",
            PhoneNumber = "+31612345678",
            Role = ApplicationRole.User,
            PasswordHash = passwordHasher.HashPassword("fundermaps"),
            AuthKey = "fmsk.a1LKIR7nUT8SPELGdCNnT2ngQV8RDQXI",
        });
    }

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

    public async Task<User> GetByEmailAsync(string email)
    {
        await Task.CompletedTask;

        return memory.Values.FirstOrDefault(x => x.Email == email) ?? throw new EntityNotFoundException(nameof(User));
    }

    public async Task<User> GetByAuthKeyAsync(string key)
    {
        await Task.CompletedTask;

        return memory.Values.FirstOrDefault(x => x.AuthKey == key) ?? throw new EntityNotFoundException(nameof(User));
    }

    public async Task<string?> GetPasswordHashAsync(Guid id)
    {
        await Task.CompletedTask;

        return memory[id].PasswordHash;
    }

    public async Task<int> GetAccessFailedCount(Guid id)
    {
        await Task.CompletedTask;

        return memory[id].AccessFailedCount;
    }

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
        };
    }

    public async Task SetPasswordHashAsync(Guid id, string passwordHash)
    {
        await Task.CompletedTask;

        memory[id].PasswordHash = passwordHash;
    }

    public async Task BumpAccessFailed(Guid id)
    {
        await Task.CompletedTask;

        memory[id].AccessFailedCount++;
    }

    public async Task ResetAccessFailed(Guid id)
    {
        await Task.CompletedTask;

        memory[id].AccessFailedCount = 0;
    }

    public async Task RegisterAccess(Guid id)
    {
        await Task.CompletedTask;

        memory[id].LoginCount++;
    }

    async Task<User> IAsyncRepository<User, Guid>.GetByIdAsync(Guid id)
    {
        return await GetByIdAsync(id);
    }

    IAsyncEnumerable<User> IAsyncRepository<User, Guid>.ListAllAsync(Navigation navigation)
    {
        return ListAllAsync(navigation);
    }
}

internal class MemoryOrganizationRepository : MemoryRepositoryBase<Organization, Guid>, IOrganizationRepository
{
    public MemoryOrganizationRepository()
    {
        memory.Add(Guid.Parse("a44aa6d6-714a-4d5e-a6c7-25c9a840d114"), new()
        {
            Id = Guid.Parse("a44aa6d6-714a-4d5e-a6c7-25c9a840d114"),
            Name = "FunderMaps",
            Email = "info@fundermaps.com",
            Area = new SpatialBox { },
            Center = new SpatialPoint { }
        });
        memory.Add(Guid.Parse("7b6f6e29-24b6-41ff-a433-6f2aaddf2d91"), new()
        {
            Id = Guid.Parse("7b6f6e29-24b6-41ff-a433-6f2aaddf2d91"),
            Name = "Contoso",
            Email = "info@contoso.com",
            Area = new SpatialBox { },
            Center = new SpatialPoint { }
        });
    }
}

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

        Guid[] staticArray = [Guid.Parse("a44aa6d6-714a-4d5e-a6c7-25c9a840d114")];

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

internal class MemoryAddressRepository : MemoryRepositoryBase<Address, string>, IAddressRepository
{
    public MemoryAddressRepository()
    {
        memory.Add("gfm-c36dcd7b53c84b5eb39ad880c0955fed", new()
        {
            Id = "gfm-c36dcd7b53c84b5eb39ad880c0955fed",
            BuildingNumber = "8b",
            PostalCode = "3023AM",
            Street = "Vierambachtsstraat",
            IsActive = true,
            ExternalId = "NL.IMBAG.NUMMERAANDUIDING.0599200000499204",
            City = "Rotterdam",
            BuildingId = "gfm-4f5e73d478ff452b86023a06e5b8d834",
        });
    }

    public async Task<Address> GetByExternalIdAsync(string id)
    {
        await Task.CompletedTask;

        return memory.Values.FirstOrDefault(x => x.ExternalId == id) ?? throw new EntityNotFoundException(nameof(Building));
    }
}

internal class MemoryBuildingRepository : MemoryRepositoryBase<Building, string>, IBuildingRepository
{
    private readonly Dictionary<string, Address> memoryAddress = new()
    {
        ["gfm-c36dcd7b53c84b5eb39ad880c0955fed"] = new()
        {
            Id = "gfm-c36dcd7b53c84b5eb39ad880c0955fed",
            BuildingNumber = "8b",
            PostalCode = "3023AM",
            Street = "Vierambachtsstraat",
            IsActive = true,
            ExternalId = "NL.IMBAG.NUMMERAANDUIDING.0599200000499204",
            City = "Rotterdam",
            BuildingId = "gfm-4f5e73d478ff452b86023a06e5b8d834",
        },
    };

    public MemoryBuildingRepository()
    {
        memory.Add("gfm-4f5e73d478ff452b86023a06e5b8d834", new()
        {
            Id = "gfm-4f5e73d478ff452b86023a06e5b8d834",
            BuiltYear = new DateTime(1908, 1, 1),
            IsActive = true,
            ExternalId = "NL.IMBAG.PAND.0599100000685769",
            NeighborhoodId = "gfm-7bc9bb6497984a13a2cc95ea1a284825",
        });
    }

    public async Task<Building> GetByExternalIdAsync(string id)
    {
        await Task.CompletedTask;

        return memory.Values.FirstOrDefault(x => x.ExternalId == id) ?? throw new EntityNotFoundException(nameof(Building));
    }

    public async Task<Building> GetByExternalAddressIdAsync(string id)
    {
        await Task.CompletedTask;

        var address = memoryAddress.Values.FirstOrDefault(x => x.ExternalId == id) ?? throw new EntityNotFoundException(nameof(Building));
        return await GetByIdAsync(address?.BuildingId ?? throw new EntityNotFoundException(nameof(Building)));
    }
}

internal class MemoryAnalysisRepository : IAnalysisRepository
{
    protected readonly Dictionary<string, AnalysisProduct> memory = [];

    public MemoryAnalysisRepository()
    {
        memory.Add("gfm-4f5e73d478ff452b86023a06e5b8d834", new()
        {
            BuildingId = "gfm-4f5e73d478ff452b86023a06e5b8d834",
            ExternalBuildingId = "NL.IMBAG.PAND.0599100000685769",
            NeighborhoodId = "gfm-7bc9bb6497984a13a2cc95ea1a284825",
            ConstructionYear = 1908,
            ConstructionYearReliability = Reliability.Indicative,
            RestorationCosts = 306_200,
            Height = 14.18,
            Velocity = -1.90,
            GroundWaterLevel = 1.50,
            GroundLevel = -1.63,
            Soil = "ni-zk",
            SurfaceArea = 157.05,
            InquiryType = InquiryType.ArchieveResearch,
            FoundationType = FoundationType.Wood,
            FoundationTypeReliability = Reliability.Cluster,
            Drystand = -0.001449942588809927,
            DrystandReliability = Reliability.Indicative,
            DrystandRisk = FoundationRisk.C,
            DewateringDepthReliability = Reliability.Indicative,
            BioInfectionReliability = Reliability.Indicative,
            BioInfectionRisk = FoundationRisk.B,
        });
    }

    public async Task<AnalysisProduct> GetAsync(string id)
    {
        await Task.CompletedTask;

        return memory.Values.FirstOrDefault(x => x.BuildingId == id) ?? throw new EntityNotFoundException(nameof(AnalysisProduct));
    }

    public async Task<AnalysisProduct> GetByExternalIdAsync(string id)
    {
        await Task.CompletedTask;

        return memory.Values.FirstOrDefault(x => x.ExternalBuildingId == id) ?? throw new EntityNotFoundException(nameof(AnalysisProduct));
    }

    public async Task<bool> GetRiskIndexAsync(string id)
    {
        await Task.CompletedTask;

        return true;
    }

    public async Task<bool> RegisterProductMatch(string buildingId, string id, string product, Guid tenantId)
    {
        await Task.CompletedTask;

        return true;
    }

    public async Task RegisterProductMismatch(string id, Guid tenantId)
    {
        await Task.CompletedTask;

        return;
    }
}

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
            services.Replace(ServiceDescriptor.Scoped<IAddressRepository, MemoryAddressRepository>());
            services.Replace(ServiceDescriptor.Scoped<IAnalysisRepository, MemoryAnalysisRepository>());
            services.Replace(ServiceDescriptor.Scoped<IBuildingRepository, MemoryBuildingRepository>());
            // services.AddScoped<IBundleRepository, BundleRepository>();
            // services.AddScoped<IContractorRepository, ContractorRepository>();
            // services.AddScoped<IIncidentRepository, IncidentRepository>();
            // services.AddScoped<IInquiryRepository, InquiryRepository>();
            // services.AddScoped<IInquirySampleRepository, InquirySampleRepository>();
            // services.AddScoped<IMapsetRepository, MapsetRepository>();
            // services.AddScoped<INeighborhoodRepository, NeighborhoodRepository>();
            services.Replace(ServiceDescriptor.Scoped<IOrganizationRepository, MemoryOrganizationRepository>());
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
