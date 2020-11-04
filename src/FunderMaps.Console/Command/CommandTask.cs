using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using FunderMaps.Core.Threading;

namespace FunderMaps.Console.Command
{
    /// <summary>
    ///     Base class to command tasks.
    /// </summary>
    /// <remarks>
    ///     This class can setup a temporary environment in which
    ///     the task can run safely without causing any side effects
    ///     on the system.
    /// </remarks>
    public abstract class CommandTask : BackgroundTask
    {
        private const string TaskIdName = "FM_TASK_ID";

        /// <summary>
        ///     Command context.
        /// </summary>
        protected CommandTaskContext Context { get; private set; }

        // FUTURE: Clean the environment
        /// <summary>
        ///     Run command in workspace.
        /// </summary>
        /// <param name="fileName">Command filename.</param>
        /// <param name="arguments">Optional command arguments</param>
        protected async Task RunCommand(string fileName, string arguments = null)
        {
            var processInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                WorkingDirectory = Context.Workspace,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
            };

            processInfo.Environment.Clear();
            processInfo.Environment[TaskIdName] = Context.Id.ToString();
            processInfo.Environment["PATH"] = System.Environment.GetEnvironmentVariable("PATH");
            processInfo.Environment["LANG"] = System.Environment.GetEnvironmentVariable("LANG");
            processInfo.Environment["LANGUAGE"] = System.Environment.GetEnvironmentVariable("LANGUAGE");
            processInfo.Environment["TERM"] = System.Environment.GetEnvironmentVariable("TERM");

            using var process = System.Diagnostics.Process.Start(processInfo);
            if (process == null)
            {
                // TODO Too specific
                throw new Exception($"Could not start process for {fileName}");
            }

            var stdoutWriter = File.CreateText($"{Context.Workspace}/{process.Id}.stdout");
            var stderrWriter = File.CreateText($"{Context.Workspace}/{process.Id}.stderr");

            process.OutputDataReceived += (sender, args) => stdoutWriter.WriteLine(args.Data);
            process.ErrorDataReceived += (sender, args) => stderrWriter.WriteLine(args.Data);

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            using var registration = Context.CancellationToken.Register(() =>
            {
                process.Kill(entireProcessTree: true);
            });

            process.WaitForExit();
            await registration.DisposeAsync();

            await stdoutWriter.FlushAsync();
            await stderrWriter.FlushAsync();
        }

        /// <summary>
        ///     Setup command workspace.
        /// </summary>
        /// <param name="context">Command task execution context.</param>
        public virtual Task SetupAsync(CommandTaskContext context)
        {
            context.Workspace = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(context.Workspace);

            // These files are created as a placeholder. Some operating systems
            // may cleanup unused temporary directories when disk space is limited.
            File.Create($"{context.Workspace}/.lock");
            File.WriteAllText($"{context.Workspace}/{TaskIdName}", Context.Id.ToString());

            return Task.CompletedTask;
        }

        /// <summary>
        ///     Cleanup workspace.
        /// </summary>
        /// <param name="context">Command task execution context.</param>
        public virtual Task TeardownAsync(CommandTaskContext context)
        {
            if (!context.KeepWorkspace)
            {
                Directory.Delete(context.Workspace, recursive: true);
            }

            return Task.CompletedTask;
        }

        /// <summary>
        ///     Run the background task body.
        /// </summary>
        /// <param name="context">Background task execution context.</param>
        public override async Task ExecuteAsync(BackgroundTaskContext context)
        {
            // We allways want to yield for the async state machine.
            await Task.Yield();

            Context = new CommandTaskContext(context.Id)
            {
                CancellationToken = context.CancellationToken,
                Value = context.Value,
                KeepWorkspace = false,
            };

            try
            {
                await SetupAsync(Context);

                Context.CancellationToken.ThrowIfCancellationRequested();

                await ExecuteCommandAsync(Context);
            }
            finally
            {
                await TeardownAsync(Context);
            }
        }

        /// <summary>
        ///     Run the background command.
        /// </summary>
        /// <param name="context">Command task execution context.</param>
        public abstract Task ExecuteCommandAsync(CommandTaskContext context);
    }
}
