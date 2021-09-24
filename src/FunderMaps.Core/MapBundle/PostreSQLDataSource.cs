using FunderMaps.Core.Threading.Command;

namespace FunderMaps.Core.MapBundle
{
    internal class PostreSQLDataSource : DataSource
    {
        private string _connectionString;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public PostreSQLDataSource(string connectionString) => _connectionString = connectionString;

        private string ParseCommandString(CommandInfo commandInfo)
        {
            string dbname = null;
            foreach (var item in _connectionString.Split(';'))
            {
                string[] keyValue = item.Split('=');
                switch (keyValue[0].ToLower())
                {
                    case "server":
                        commandInfo.Environment.Add("PGHOST", keyValue[1]);
                        break;

                    case "port":
                        commandInfo.Environment.Add("PGPORT", keyValue[1]);
                        break;

                    case "user id":
                        commandInfo.Environment.Add("PGUSER", keyValue[1]);
                        break;

                    case "password":
                        commandInfo.Environment.Add("PGPASSWORD", keyValue[1]);
                        break;

                    case "database":
                        dbname = keyValue[1];
                        break;
                }
            }

            return $"PG:dbname={dbname}";
        }

        public override string Read(CommandInfo commandInfo) => ParseCommandString(commandInfo);

        public override string Write(CommandInfo commandInfo) => ParseCommandString(commandInfo);
    }
}
