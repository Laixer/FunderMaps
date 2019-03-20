using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using FunderMaps.Helpers;
using FunderMaps.Authorization.Requirement;
using FunderMaps.Models.Fis;
using FunderMaps.Data.Authorization;
using FunderMaps.Extensions;

namespace FunderMaps.Authorization.Handler
{
    public class FisOperationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Report>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement,
            Report report)
        {
            // Administrator role can operate on anything
            if (context.User.IsInRole(Constants.AdministratorRole))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            // User must have organization claim on this resource
            if (!context.User.HasClaim(FisClaimTypes.OrganizationAttestationIdentifier, report.Owner.ToString()))
            {
                return Task.CompletedTask;
            }

            var organizationClaim = context.User.GetClaim(FisClaimTypes.OrganizationUserRole);
            if (organizationClaim == null)
            {
                return Task.CompletedTask;
            }

            // User must have certain organization role for operation
            switch (organizationClaim)
            {
                case Constants.SuperuserRole:
                    context.Succeed(requirement);
                    break;
                case Constants.WriterRole:
                    if (requirement.Name == OperationsRequirement.Read.Name
                        || requirement.Name == OperationsRequirement.Create.Name
                        || requirement.Name == OperationsRequirement.Update.Name)
                    {
                        context.Succeed(requirement);
                    }
                    break;
                case Constants.ReaderRole:
                    if (requirement.Name == OperationsRequirement.Read.Name)
                    {
                        context.Succeed(requirement);
                    }
                    break;
                case Constants.VerifierRole:
                    if (requirement.Name == OperationsRequirement.Read.Name
                        || requirement.Name == OperationsRequirement.Create.Name
                        || requirement.Name == OperationsRequirement.Update.Name
                        || requirement.Name == OperationsRequirement.Validate.Name)
                    {
                        context.Succeed(requirement);
                    }
                    break;
            }

            return Task.CompletedTask;
        }
    }
}
