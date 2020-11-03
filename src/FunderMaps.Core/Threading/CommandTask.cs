using System;
using System.IO;
using System.Threading.Tasks;

namespace FunderMaps.Core.Types.BackgroundTasks
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
        private string currentDirectory;

        /// <summary>
        ///     Workspace directory.
        /// </summary>
        protected string workspaceDirectory;

        /// <summary>
        ///     Setup command workspace.
        /// </summary>
        /// <param name="context">Background task execution context.</param>
        public virtual Task SetupAsync(BackgroundTaskContext context)
        {
            currentDirectory = Directory.GetCurrentDirectory();
            workspaceDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(workspaceDirectory);
            Directory.SetCurrentDirectory(workspaceDirectory);

            // These files are created as a placeholder. Some operating systems
            // may cleanup unused temporary directories when disk space is limited.
            System.IO.File.Create(".lock");
            System.IO.File.WriteAllText("task_id", context.Id.ToString());

            // The environment settings can be used by external tools.
            Environment.SetEnvironmentVariable("TASK_ID", context.Id.ToString());

            return Task.CompletedTask;
        }

        /// <summary>
        ///     Cleanup workspace.
        /// </summary>
        /// <param name="context">Background task execution context.</param>
        public virtual Task TeardownAsync(BackgroundTaskContext context)
        {
            Environment.SetEnvironmentVariable("TASK_ID", null);

            Directory.SetCurrentDirectory(currentDirectory);
            Directory.Delete(workspaceDirectory, recursive: true);
            return Task.CompletedTask;
        }

        /// <summary>
        ///     Run the background task body.
        /// </summary>
        /// <param name="context">Background task execution context.</param>
        public override async Task ProcessAsync(BackgroundTaskContext context)
        {
            // We allways want to yield for the async state machine.
            await Task.Yield();

            try
            {
                await SetupAsync(context);
                await ExecuteAsync(context);
            }
            finally
            {
                await TeardownAsync(context);
            }
        }

        /// <summary>
        ///     Run the background command.
        /// </summary>
        /// <param name="context">Background task execution context.</param>
        public abstract Task ExecuteAsync(BackgroundTaskContext context);
    }
}
