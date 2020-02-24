using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FunderMaps.Identity
{
    /// <summary>
    /// Provides the APIs for managing organizations in a persistence store.
    /// </summary>
    /// <typeparam name="TOrganization">The type encapsulating an organization.</typeparam>
    public class OrganizationManager<TOrganization>
        where TOrganization : class
    {
        /// <summary>
        /// The cancellation token used to cancel operations.
        /// </summary>
        protected virtual CancellationToken CancellationToken => CancellationToken.None;

        /// <summary>
        /// Gets or sets the persistence store the manager operates over.
        /// </summary>
        /// <value>The persistence store the manager operates over.</value>
        protected internal IOrganizationStore<TOrganization> Store { get; set; }

        /// <summary>
        /// The <see cref="ILogger"/> used to log messages from the manager.
        /// </summary>
        /// <value>
        /// The <see cref="ILogger"/> used to log messages from the manager.
        /// </value>
        public virtual ILogger Logger { get; set; }

        /// <summary>
        /// The <see cref="ILookupNormalizer"/> used to normalize things like user and role names.
        /// </summary>
        public ILookupNormalizer KeyNormalizer { get; set; }

        /// <summary>
        /// The <see cref="IdentityErrorDescriber"/> used to generate error messages.
        /// </summary>
        public IdentityErrorDescriber ErrorDescriber { get; set; }

        /// <summary>
        /// The <see cref="IdentityOptions"/> used to configure Identity.
        /// </summary>
        public IdentityOptions Options { get; set; }

        /// <summary>
        /// Constructs a new instance of <see cref="OrganizationManager{TOrganization}"/>.
        /// </summary>
        /// <param name="store">The persistence store the manager will operate over.</param>
        /// <param name="optionsAccessor">The accessor used to access the <see cref="IdentityOptions"/>.</param>
        /// <param name="keyNormalizer">The <see cref="ILookupNormalizer"/> to use when generating index keys for organizations.</param>
        /// <param name="errors">The <see cref="IdentityErrorDescriber"/> used to provider error messages.</param>
        /// <param name="logger">The logger used to log messages, warnings and errors.</param>
        public OrganizationManager(IOrganizationStore<TOrganization> store,
            IOptions<IdentityOptions> optionsAccessor,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            ILogger<OrganizationManager<TOrganization>> logger)
        {
            Store = store ?? throw new ArgumentNullException(nameof(store));
            Options = optionsAccessor?.Value ?? new IdentityOptions();
            KeyNormalizer = keyNormalizer;
            ErrorDescriber = errors;
            Logger = logger;
        }

        /// <summary>
        /// Finds and returns an organization, if any, who has the specified <paramref name="organizationId"/>.
        /// </summary>
        /// <param name="organizationId">The user ID to search for.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the user matching the specified <paramref name="organizationId"/> if it exists.
        /// </returns>
        public virtual Task<TOrganization> FindByIdAsync(string organizationId)
            => Store.FindByIdAsync(organizationId, CancellationToken);
    }
}
