using FunderMaps.Core.Abstractions;
using FunderMaps.Core.MapBundle.Jobs;
using FunderMaps.Core.Threading;

namespace FunderMaps.Core.MapBundle;

/// <summary>
///     Process bundles.
/// </summary>
internal class BundleHub : AppServiceBase, IBundleService
{
    private readonly BackgroundTaskDispatcher _backgroundTaskDispatcher;

    /// <summary>
    ///     Create new instance.
    /// </summary>
    public BundleHub(BackgroundTaskDispatcher backgroundTaskDispatcher)
        => _backgroundTaskDispatcher = backgroundTaskDispatcher ?? throw new ArgumentNullException(nameof(backgroundTaskDispatcher));

    /// <summary>
    ///     Send build candidates off to background worker.
    /// </summary>
    public async Task BuildAsync() => await _backgroundTaskDispatcher.EnqueueTaskAsync<ExportJob>();
}
