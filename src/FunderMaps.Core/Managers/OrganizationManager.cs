using FunderMaps.Core.Interfaces.Repositories;
using System;

namespace FunderMaps.Core.Managers
{
    /// <summary>
    ///     Organization manager.
    /// </summary>
    public class OrganizationManager
    {
        private readonly UserManager _userManager;
        //private readonly IOrganizationRepository _organizationRepository;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public OrganizationManager(UserManager userManager/*, IOrganizationRepository organizationRepository*/)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            //_organizationRepository = organizationRepository ?? throw new ArgumentNullException(nameof(organizationRepository));
        }
    }
}
