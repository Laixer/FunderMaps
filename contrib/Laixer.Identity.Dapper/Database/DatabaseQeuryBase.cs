using System.Data;

namespace Laixer.Identity.Dapper.Database
{
    internal abstract class DatabaseQeuryBase : IDatabaseDriver
    {
        public abstract IDbConnection GetDbConnection();

        public string CreateAsync { get; protected set; }
        public string FindByEmailAsync { get; protected set; }
        public string GetPasswordHashAsync { get; protected set; }
    }
}
