using FunderMaps.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace FunderMaps.WebApi.Managers
{
    public class FunderMapsOrganizationManager
    {
        public FunderMapsOrganizationManager(
            IUserStore<FunderMapsUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            UserManager<FunderMapsUser> userManager,
            IEnumerable<IUserValidator<FunderMapsUser>> userValidators,
            IEnumerable<IPasswordValidator<FunderMapsUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<FunderMapsOrganizationManager> logger)
            //: base(store, optionsAccessor, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }
    }
}
