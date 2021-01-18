using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using Microsoft.Extensions.Hosting;
using System.CommandLine.Parsing;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System;
using System.IO;
using FunderMaps.Cli.Drivers;
using Microsoft.Extensions.Logging;

namespace FunderMaps.Cli
{
    /// <summary>
    ///     Application entry.
    /// </summary>
    public static class Program
    {
        /// <summary>
        ///     Application entry point.
        /// </summary>
        /// <param name="args">Commandline arguments.</param>
        public static Task Main(string[] args)
            => CreateHostBuilder(args).Build().InvokeAsync(args);

        /// <summary>
        ///     Build a host and run the application.
        /// </summary>
        /// <remarks>
        ///     The signature of this method should not be changed.
        ///     External tooling expects this function be present.
        /// </remarks>
        /// <param name="args">Commandline arguments.</param>
        /// <returns>See <see cref="IHostBuilder"/>.</returns>
        public static CommandLineBuilder CreateHostBuilder(string[] args)
            => BuildCommandLine()
                .AddLocalCommands()
                .UseHost(_ => Host.CreateDefaultBuilder(),
                    host =>
                    {
                        host.ConfigureLogging(logging =>
                        {
                            logging.ClearProviders();
                            logging.AddConsole();
                            logging.SetMinimumLevel(LogLevel.Error);
                        });
                        host.ConfigureServices(services =>
                        {
                            // Configure FunderMaps services.
                            services.AddFunderMapsCoreServices();
                            services.AddFunderMapsInfrastructureServices();

                            services.AddTransient<BatchDriver>();
                        });
                    })
                .UseDefaults();

        /// <summary>
        ///     Build commandline builder.
        /// </summary>
        /// <returns>See <see cref="CommandLineBuilder"/>.</returns>
        private static CommandLineBuilder BuildCommandLine()
        {
            RootCommand root = new();
            root.AddGlobalOption(new Option<FileInfo>(new[] { "-c", "--config" }, "An option whose argument is parsed as a FileInfo"));
            root.Description = "FunderMaps CommandLine Interface";

            return new(root);
        }

        /// <summary>
        ///     Register local commands.
        /// </summary>
        /// <returns>See <see cref="CommandLineBuilder"/>.</returns>
        private static CommandLineBuilder AddLocalCommands(this CommandLineBuilder rootBuilder)
        {
            Command command = new("batch", "Run batch operations");
            command.AddAlias("b");
            command.AddOption(new Option<string>(new[] { "-H", "--host" }, "The batch host to execute the command on"));
            command.AddOption(new Option<bool>("--ignore-cert", "Ignore TLS certificate checks"));

            {
                Command subcommand = new("build-bundle", "The maplayer bundle(s)");
                subcommand.AddArgument(new Argument<Guid[]>("bundleId", "(Optional) Bundle identifiers to build")
                {
                    Arity = ArgumentArity.OneOrMore,
                });
                subcommand.Handler = DriverHandler.InstantiateDriver<IEnumerable<Guid>>(BatchDriver.BuildBundleAsync);
                command.AddCommand(subcommand);
            }

            {
                Command subcommand = new("build-bundle-all", "The all maplayer bundles");
                subcommand.Handler = DriverHandler.InstantiateDriver(BatchDriver.BuildAllAsync);
                command.AddCommand(subcommand);
            }

            {
                Command subcommand = new("status", "Print the batch status reports");
                subcommand.Handler = DriverHandler.InstantiateDriver(BatchDriver.StatusAsync);
                command.AddCommand(subcommand);
            }

            rootBuilder.AddCommand(command);
            return rootBuilder;
        }
    }
}
