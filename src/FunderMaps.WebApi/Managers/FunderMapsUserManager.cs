using FunderMaps.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace FunderMaps.WebApi.Managers
{
    public class FunderMapsUserManager : UserManager<FunderMapsUser>
    {
        public FunderMapsUserManager(
            IUserStore<FunderMapsUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<FunderMapsUser> passwordHasher,
            IEnumerable<IUserValidator<FunderMapsUser>> userValidators,
            IEnumerable<IPasswordValidator<FunderMapsUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<FunderMapsUser>> logger)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }
    }
}
