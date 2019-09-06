namespace Laixer.Identity.Dapper
{
    /// <summary>
    /// Interface for custom query properties.
    /// </summary>
    public interface ICustomQueryRepository
    {
        /// <summary>
        /// Configure the repository properties.
        /// </summary>
        /// <param name="databaseQuery"></param>
        void Configure(IQueryRepository queryRepository);
    }
}
