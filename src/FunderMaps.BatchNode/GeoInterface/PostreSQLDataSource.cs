using FunderMaps.BatchNode.Command;
using Npgsql;

namespace FunderMaps.BatchNode.GeoInterface
{
    internal class PostreSQLDataSource : DataSource
    {
        // FUTURE: Replace with non npgsql version
        private NpgsqlConnectionStringBuilder connectionStringBuilder;

        public PostreSQLDataSource(string dbConnection)
        {
            connectionStringBuilder = new NpgsqlConnectionStringBuilder(dbConnection);
        }

        public override string Read(CommandInfo commandInfo)
        {
            commandInfo.Environment.Add("PGHOST", connectionStringBuilder.Host);
            commandInfo.Environment.Add("PGUSER", connectionStringBuilder.Username);
            commandInfo.Environment.Add("PGPASSWORD", connectionStringBuilder.Password);

            return $"PG:dbname={connectionStringBuilder.Database}";
        }

        public override string Write(CommandInfo commandInfo)
        {
            commandInfo.Environment.Add("PGHOST", connectionStringBuilder.Host);
            commandInfo.Environment.Add("PGUSER", connectionStringBuilder.Username);
            commandInfo.Environment.Add("PGPASSWORD", connectionStringBuilder.Password);

            return $"PG:dbname={connectionStringBuilder.Database}";
        }

        public static PostreSQLDataSource FromConnectionString(string connectionString)
            => new PostreSQLDataSource(connectionString);
    }
}
