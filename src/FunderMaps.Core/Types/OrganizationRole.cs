namespace FunderMaps.Core.Types;

/// <summary>
///     Organization user role.
/// </summary>
public enum OrganizationRole
{
    /// <summary>
    ///     Superuser.
    /// </summary>
    Superuser = 0,

    /// <summary>
    ///     Verifier.
    /// </summary>
    Verifier = 1,

    /// <summary>
    ///     Writer.
    /// </summary>
    Writer = 2,

    /// <summary>
    ///     Reader.
    /// </summary>
    Reader = 3,
}
