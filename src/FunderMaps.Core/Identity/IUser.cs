using System;
using FunderMaps.Core.Types;

namespace FunderMaps.Core.Identity;

/// <summary>
///     User identity.
/// </summary>
public interface IUser
{
    /// <summary>
    ///     Unique user identifier.
    /// </summary>
    Guid Id { get; set; }

    /// <summary>
    ///     User role.
    /// </summary>
    ApplicationRole Role { get; set; }
}
