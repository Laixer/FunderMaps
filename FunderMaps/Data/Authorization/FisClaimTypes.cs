namespace FunderMaps.Data.Authorization
{
    /// <summary>
    /// FIS attestation claims.
    /// </summary>
    public static class FisClaimTypes
    {
        /// <summary>
        /// FIS user identifier.
        /// </summary>
        public static readonly string UserAttestationIdentifier = "http://fundermaps.com/2019/identity/claims/fisuseridentifier";
        
        /// <summary>
        /// FIS role.
        /// </summary>
        public static readonly string OrganizationUserRole = "http://fundermaps.com/2019/identity/claims/fisuserrole";
        
        /// <summary>
        /// FIS organization identifier.
        /// </summary>
        public static readonly string OrganizationAttestationIdentifier = "http://fundermaps.com/2019/identity/claims/fisorgidentifier";
    }
}
