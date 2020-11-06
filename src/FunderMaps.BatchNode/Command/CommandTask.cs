using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FunderMaps.Core.Threading;

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

        /// <summary>
        ///     Command context.
        /// </summary>
        protected CommandTaskContext Context { get; private set; }

        public Task RunCommandAsync(string fileName, string arguments = null)
        {
            return Task.Run(() => RunCommand(fileName, arguments));
        }

        /// <summary>
        ///     Run command in workspace.
        /// </summary>
        /// <param name="fileName">Command filename.</param>
        /// <param name="arguments">Optional command arguments.</param>
        public void RunCommand(string fileName, string arguments = null)
        {
            var command = new CommandInfo(fileName);
            if (arguments != null)
            {
                foreach (var argument in arguments.Split(" "))
                {
                    command.ArgumentList.Add(argument.Trim());
                }
            }
            RunCommand(command);
        }

        public Task RunCommandAsync(CommandInfo commandInfo)
        {
            return Task.Run(() => RunCommand(commandInfo));
        }

        /// <summary>
        ///     Run command in workspace.
        /// </summary>
        /// <param name="commandInfo">Command descriptor.</param>
        protected void RunCommand(CommandInfo commandInfo)
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

            process.WaitForExit();

            stdoutWriter.Flush();
            stderrWriter.Flush();
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
