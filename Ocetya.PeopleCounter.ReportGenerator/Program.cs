namespace Ocetya.PeopleCounter.ReportGenerator
{
    using System;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.EventLog;
    using Ocetya.PeopleCounter.ReportGenerator.Config;
    using Ocetya.PeopleCounter.ReportGenerator.Interfaces;
    using Ocetya.PeopleCounter.ReportGenerator.Models;
    using Ocetya.PeopleCounter.ReportGenerator.StepPattern;
    using WindowsInput;

    public class Program
    {
        public static IConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            return builder.Build();
        }

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(options =>
                    options.AddFilter<EventLogLoggerProvider>(level => level >= LogLevel.Information))
                .ConfigureServices((hostContext, services) =>
                {
                    var configuration = LoadConfiguration();

                    services
                        .AddHostedService<Worker>()
                        .Configure<EventLogSettings>(config =>
                        {
                            config.LogName = "Report Generator Service";
                            config.SourceName = "Report Generator";
                        });

                    services.Configure<Point>(configuration.GetSection(ConfigurationKeys.MOUSE_STARTING_POINT));
                    services.Configure<ButtonPoint>(configuration.GetSection(ConfigurationKeys.MOUSE_BUTTON_POINT));

                    services
                        .AddTransient<IInputSimulator, InputSimulator>()
                        .AddTransient<IStepManager, StepManager>()
                        .AddTransient<IWin32, Win32>()
                        .AddTransient<IRunner, Runner>();
                }).UseWindowsService();
    }
}