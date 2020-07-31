namespace FunderMaps.Authorization
{
    /// <summary>
    ///     Authorization claims.
    /// </summary>
    public static class ClaimTypes
    {
        /// <summary>
        ///     Organization user.
        /// </summary>
        public static readonly string OrganizationUser = "http://fundermaps.com/2019/identity/claims/orguser";

        /// <summary>
        ///     Organization role.
        /// </summary>
        public static readonly string OrganizationUserRole = "http://fundermaps.com/2019/identity/claims/orgrole";
    }
}
