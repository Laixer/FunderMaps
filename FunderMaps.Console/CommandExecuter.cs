using FunderMaps.Core.Exceptions;
using System.Diagnostics;
using System.Text;

namespace FunderMaps.Console
{
    /// <summary>
    ///     Class to execute commands for us.
    /// </summary>
    internal static class CommandExecuter
    {
        /*
        * TODO https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.process.start?view=netcore-3.1
        * A new Process that is associated with the process resource, 
        * or null if no process resource is started. Note that a new 
        * process that's started alongside already running instances 
        * of the same process will be independent from the others. In 
        * addition, Start may return a non-null Process with its HasExited 
        * property already set to true. In this case, the started process 
        * may have activated an existing instance of itself and then exited.
        */

        // TODO This is very dangerous and should be made as bulletproof as possible!
        /// <summary>
        ///     Executes a command through bash -c.
        /// </summary>
        /// <remarks>
        ///     This escapes any " characters to \\\" meaning they will come
        ///     out as " after the bash -c evaluation.
        /// </remarks>
        /// <param name="commandText">The command text to execute.</param>
        internal static void ExecuteCommand(string commandText)
        {
            // Escape characters so bash -c can run.
            var parsedCommandText = commandText.Replace("\"", "\\\"");

            // TODO This is hardcoded for linux.
            var processInfo = new ProcessStartInfo("/bin/bash", $"-c \"{parsedCommandText}\"")
            {
                CreateNoWindow = true,
                UseShellExecute = false,

                RedirectStandardError = true,
                RedirectStandardOutput = true
            };

            // TODO Remove
            System.Console.WriteLine("processInfo.Arguments: " + processInfo.Arguments);

            using var process = Process.Start(processInfo);
            if (process == null)
            {
                throw new BundleExportException($"Could not start process for {parsedCommandText}");
            }

            // Append logging.
            var sbOutput = new StringBuilder();
            process.OutputDataReceived += (sender, args) => sbOutput.AppendLine(args.Data);

            var sbError = new StringBuilder();
            process.ErrorDataReceived += (sender, args) => sbError.AppendLine(args.Data);

            process.BeginOutputReadLine();
            process.WaitForExit();

            System.Console.WriteLine(sbOutput.ToString());
            System.Console.WriteLine("Process exit code = " + process.ExitCode);

            if (process.ExitCode != 0)
            {
                System.Console.WriteLine("Process failed, error log:");
                System.Console.WriteLine(sbError.ToString());
                System.Console.WriteLine("End of error log");
                throw new BundleExportException($"Process failed for {commandText}");
            }
        }
    }
}
