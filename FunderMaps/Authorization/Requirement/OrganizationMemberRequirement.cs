using Microsoft.AspNetCore.Authorization;

namespace FunderMaps.Authorization.Requirement
{
    public class OrganizationMemberRequirement : IAuthorizationRequirement
    {
        public bool AllowAdministratorAlways { get; set; } = true;
    }
}
