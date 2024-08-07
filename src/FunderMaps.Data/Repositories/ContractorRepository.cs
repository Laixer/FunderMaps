﻿using Dapper;
using FunderMaps.Core;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;

namespace FunderMaps.Data.Repositories;

internal class ContractorRepository : RepositoryBase<Contractor, int>, IContractorRepository
{
    public override async IAsyncEnumerable<Contractor> ListAllAsync(Navigation navigation)
    {
        var sql = "SELECT id, name FROM application.contractor ORDER BY id";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await foreach (var item in connection.QueryUnbufferedAsync<Contractor>(sql))
        {
            yield return item;
        }
    }
}
