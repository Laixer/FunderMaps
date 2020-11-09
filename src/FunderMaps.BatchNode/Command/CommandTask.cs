using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FunderMaps.Core.Threading;
using Microsoft.Extensions.Logging;

namespace FunderMaps.BatchNode.Command
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

        protected readonly ILogger _logger;

        /// <summary>
        ///     Command context.
        /// </summary>
        protected CommandTaskContext Context { get; private set; }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public CommandTask(ILogger logger)
            => _logger = logger;

        /// <summary>
        ///     Run command in workspace.
        /// </summary>
        /// <param name="fileName">Command filename.</param>
        /// <param name="arguments">Optional command arguments.</param>
        public Task<int> RunCommandAsync(string fileName, string arguments = null)
            => Task.Run(() => RunCommand(fileName, arguments));

        /// <summary>
        ///     Run command in workspace.
        /// </summary>
        /// <param name="fileName">Command filename.</param>
        /// <param name="arguments">Optional command arguments.</param>
        public int RunCommand(string fileName, string arguments = null)
        {
            var command = new CommandInfo(fileName);
            if (arguments != null)
            {
                foreach (var argument in arguments.Split(" "))
                {
                    command.ArgumentList.Add(argument.Trim());
                }
            }
            return RunCommand(command);
        }

        /// <summary>
        ///     Run command in workspace.
        /// </summary>
        /// <param name="commandInfo">Command descriptor.</param>
        public Task<int> RunCommandAsync(CommandInfo commandInfo)
            => Task.Run(() => RunCommand(commandInfo));

        /// <summary>
        ///     Run command in workspace.
        /// </summary>
        /// <param name="commandInfo">Command descriptor.</param>
        protected int RunCommand(CommandInfo commandInfo)
        {
            var processInfo = new ProcessStartInfo
            {
                FileName = commandInfo.FileName,
                WorkingDirectory = Context.Workspace,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
            };

            processInfo.ArgumentList.Clear();
            foreach (var argument in commandInfo.ArgumentList.Where(s => !string.IsNullOrEmpty(s)).Select(s => s.Trim()))
            {
                processInfo.ArgumentList.Add(argument);
            }

            processInfo.Environment.Clear();
            processInfo.Environment[TaskIdName] = Context.Id.ToString();
            processInfo.Environment["PATH"] = System.Environment.GetEnvironmentVariable("PATH");
            processInfo.Environment["LANG"] = System.Environment.GetEnvironmentVariable("LANG");
            processInfo.Environment["LANGUAGE"] = System.Environment.GetEnvironmentVariable("LANGUAGE");
            processInfo.Environment["TERM"] = System.Environment.GetEnvironmentVariable("TERM");
            foreach (var environmentVariable in commandInfo.Environment)
            {
                processInfo.Environment.Add(environmentVariable);
            }

            _logger.LogDebug("Start system process");

            using var process = System.Diagnostics.Process.Start(processInfo);
            if (process == null)
            {
                throw new Exception($"Could not start process for {processInfo.FileName}"); // TODO:
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

            _logger.LogDebug("Wait for process to exit");

            process.WaitForExit();

            stdoutWriter.Flush();
            stderrWriter.Flush();

            _logger.LogDebug($"Process exit with return code: {process.ExitCode}");

            return process.ExitCode;
        }

        /// <summary>
        ///     Create a directory inside the workspace.
        /// </summary>
        public string CreateDirectory(string name = null)
        {
            var path = Path.Combine(Context.Workspace, name ?? Path.GetRandomFileName());
            Directory.CreateDirectory(path);
            return path;
        }

        /// <summary>
        ///     Setup command workspace.
        /// </summary>
        /// <param name="context">Command task execution context.</param>
        public virtual async Task SetupAsync(CommandTaskContext context)
        {
            _logger.LogDebug("Setup workspace for job");

            context.Workspace = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(context.Workspace);

            // These files are created as a placeholder. Some operating systems
            // may cleanup unused temporary directories when disk space is sparse.
            await File.Create($"{context.Workspace}/.lock").DisposeAsync();
            await File.WriteAllTextAsync($"{context.Workspace}/{TaskIdName}", Context.Id.ToString());

            _logger.LogTrace($"Workspace: {context.Workspace}");
        }

        /// <summary>
        ///     Cleanup workspace.
        /// </summary>
        /// <param name="context">Command task execution context.</param>
        public virtual Task TeardownAsync(CommandTaskContext context)
        {
            _logger.LogDebug("Teardown workspace for job");

            if (!context.KeepWorkspace)
            {
                _logger.LogTrace("Workspace directory is not kept");

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
                QueuedAt = context.QueuedAt,
                StartedAt = context.StartedAt,
                FinishedAt = context.FinishedAt,
                Delay = context.Delay,
                RetryCount = context.RetryCount,
                DispatchManager = context.DispatchManager,
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
