using Microsoft.AspNetCore.Authorization;

namespace FunderMaps.Authorization.Requirement
{
    public class OrganizationRoleRequirement : IAuthorizationRequirement
    {
        public bool AllowAdministratorAlways { get; set; } = true;
        public string Role { get; set; }

        public OrganizationRoleRequirement(string roleName)
        {
            Role = roleName;
        }
    }
}
