using FunderMaps.Core.Abstractions;
using FunderMaps.Core.Components;
using Microsoft.Extensions.Caching.Memory;

namespace FunderMaps.Data.Abstractions;

/// <summary>
///     Data service base.
/// </summary>
/// <remarks>
///     <para>
///         The data service base should be the base class to
///         all data services in the application.
///     </para>
///     <para>
///         This class is modelled after <seealso cref="AppServiceBase"/>.
///     </para>
/// </remarks>
internal abstract class DbServiceBase : AppServiceBase
{
    /// <summary>
    ///     Memory cache.
    /// </summary>
    public IMemoryCache Cache { get; set; }

    /// <summary>
    ///     Data context factory.
    /// </summary>
    public DbContextFactory DbContextFactory { get; set; }
}
