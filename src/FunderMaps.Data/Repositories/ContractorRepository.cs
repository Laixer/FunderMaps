﻿using Dapper;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data.Abstractions;

namespace FunderMaps.Data.Repositories;

internal class ContractorRepository : DbServiceBase, IContractorRepository
{
    public async IAsyncEnumerable<Contractor> ListAllAsync()
    {
        var sql = "SELECT id, name FROM application.contractor ORDER BY id";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await foreach (var item in connection.QueryUnbufferedAsync<Contractor>(sql))
        {
            yield return item;
        }
    }
}
