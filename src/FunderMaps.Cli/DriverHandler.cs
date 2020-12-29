using System;
using System.CommandLine.Invocation;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace FunderMaps.Cli
{
    /// <summary>
    ///     Driver command handler.
    /// </summary>
    public static class DriverHandler
    {
        /// <summary>
        ///     Create delegate with default parametes.
        /// </summary>
        /// <param name="action">Handler action.</param>
        /// <returns>See <see cref="ICommandHandler"/>.</returns>
        public static ICommandHandler InstantiateDriver(Func<FileInfo, IHost, Task> action)
            => CommandHandler.Create<FileInfo, IHost>(action);

        /// <summary>
        ///     Create delegate with default parametes.
        /// </summary>
        /// <param name="action">Handler action.</param>
        /// <returns>See <see cref="ICommandHandler"/>.</returns>
        public static ICommandHandler InstantiateDriver<T0>(Func<FileInfo, IHost, T0, Task> action)
            => CommandHandler.Create<FileInfo, IHost, T0>(action);

        /// <summary>
        ///     Create delegate with default parametes.
        /// </summary>
        /// <param name="action">Handler action.</param>
        /// <returns>See <see cref="ICommandHandler"/>.</returns>
        public static ICommandHandler InstantiateDriver<T0, T1>(Func<FileInfo, IHost, T0, T1, Task> action)
            => CommandHandler.Create<FileInfo, IHost, T0, T1>(action);
    }
}
