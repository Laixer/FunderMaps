namespace FunderMaps.Core.Interfaces.Repositories;

/// <summary>
///     Represents a repository for managing user data.
/// </summary>
public interface IUserdataRepository
{
    /// <summary>
    ///     Retrieves user data asynchronously based on the specified user ID and application ID.
    /// </summary>
    /// <param name="user_id">The unique identifier of the user.</param>
    /// <param name="application_id">The unique identifier of the application.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the retrieved user data.</returns>
    Task<object> GetAsync(Guid user_id, string application_id);

    /// <summary>
    ///     Updates user data asynchronously based on the specified user ID, application ID, and metadata.
    /// </summary>
    /// <param name="user_id">The unique identifier of the user.</param>
    /// <param name="application_id">The unique identifier of the application.</param>
    /// <param name="metadata">The metadata to be updated.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task UpdateAsync(Guid user_id, string application_id, object metadata);
}
