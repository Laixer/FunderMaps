using DapperOrm = Dapper;

namespace Laixer.Identity.Dapper
{
    /// <summary>
    /// Generic helper class.
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// Configure the ORM to match on underscore records.
        /// </summary>
        /// <param name="match"></param>
        public static void OrmMatchWithUnderscore(bool match)
            => DapperOrm.DefaultTypeMap.MatchNamesWithUnderscores = match;
    }
}
