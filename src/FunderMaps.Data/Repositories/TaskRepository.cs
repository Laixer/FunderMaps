using Dapper;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data.Abstractions;

namespace FunderMaps.Data.Repositories;

/// <summary>
///     Various data operations.
/// </summary>
internal sealed class TaskRepository : DbServiceBase, ITaskRepository
{
    /// <summary>
    ///     Log the run time of a task.
    /// </summary>
    public async Task LogRunTimeAsync(string id, TimeSpan runtime)
    {
        var sql = @"
            INSERT INTO application.task(name, runtime)
            VALUES (@id, @runtime);";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await connection.ExecuteAsync(sql, new { id, runtime });
    }
}
