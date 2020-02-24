using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace FunderMaps.Identity
{
    /// <summary>
    /// Provides an abstraction for a store which manages organization accounts.
    /// </summary>
    /// <typeparam name="TOrganization">The type encapsulating an organization.</typeparam>
    public interface IOrganizationStore<TOrganization>
        where TOrganization : class
    {
        /// <summary>
        /// Gets the organization identifier for the specified <paramref name="organization"/>.
        /// </summary>
        /// <param name="organization">The organization whose identifier should be retrieved.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the identifier for the specified <paramref name="organization"/>.</returns>
        Task<string> GetOrganizationIdAsync(TOrganization organization, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the user name for the specified <paramref name="organization"/>.
        /// </summary>
        /// <param name="organization">The user whose name should be retrieved.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the name for the specified <paramref name="organization"/>.</returns>
        Task<string> GetNameAsync(TOrganization organization, CancellationToken cancellationToken);

        /// <summary>
        /// Sets the given <paramref name="name" /> for the specified <paramref name="organization"/>.
        /// </summary>
        /// <param name="organization">The user whose name should be set.</param>
        /// <param name="name">The user name to set.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        Task SetNameAsync(TOrganization organization, string name, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the normalized user name for the specified <paramref name="organization"/>.
        /// </summary>
        /// <param name="organization">The user whose normalized name should be retrieved.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the normalized user name for the specified <paramref name="organization"/>.</returns>
        Task<string> GetNormalizedNameAsync(TOrganization organization, CancellationToken cancellationToken);

        /// <summary>
        /// Sets the given normalized name for the specified <paramref name="organization"/>.
        /// </summary>
        /// <param name="organization">The user whose name should be set.</param>
        /// <param name="normalizedName">The normalized name to set.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        Task SetNormalizedNameAsync(TOrganization organization, string normalizedName, CancellationToken cancellationToken);

        /// <summary>
        /// Creates the specified <paramref name="organization"/> in the user store.
        /// </summary>
        /// <param name="organization">The user to create.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the creation operation.</returns>
        Task<IdentityResult> CreateAsync(TOrganization organization, CancellationToken cancellationToken);

        /// <summary>
        /// Updates the specified <paramref name="organization"/> in the user store.
        /// </summary>
        /// <param name="organization">The user to update.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the update operation.</returns>
        Task<IdentityResult> UpdateAsync(TOrganization organization, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes the specified <paramref name="organization"/> from the organization store.
        /// </summary>
        /// <param name="organization">The user to delete.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the update operation.</returns>
        Task<IdentityResult> DeleteAsync(TOrganization organization, CancellationToken cancellationToken);

        /// <summary>
        /// Finds and returns an organization, if any, who has the specified <paramref name="organizationId"/>.
        /// </summary>
        /// <param name="organizationId">The organization ID to search for.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the user matching the specified <paramref name="organizationId"/> if it exists.
        /// </returns>
        Task<TOrganization> FindByIdAsync(string organizationId, CancellationToken cancellationToken);

        /// <summary>
        /// Finds and returns an organization, if any, who has the specified normalized name.
        /// </summary>
        /// <param name="normalizedName">The normalized name to search for.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the user matching the specified <paramref name="normalizedName"/> if it exists.
        /// </returns>
        Task<TOrganization> FindByNameAsync(string normalizedName, CancellationToken cancellationToken);
    }
}
