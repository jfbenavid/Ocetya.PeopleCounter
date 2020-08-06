namespace Ocetya.PeopleCounter.ReportGenerator
{
    using System;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.EventLog;
    using Ocetya.PeopleCounter.ReportGenerator.Interfaces;
    using Ocetya.PeopleCounter.ReportGenerator.StepPattern;
    using WindowsInput;

    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            return builder.Build();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(options =>
                    options.AddFilter<EventLogLoggerProvider>(level => level >= LogLevel.Information))
                .ConfigureServices((hostContext, services) =>
                {
                    var _configuration = LoadConfiguration();

                    services
                        .AddHostedService<Worker>()
                        .Configure<EventLogSettings>(config =>
                        {
                            config.LogName = "People Counter Service";
                            config.SourceName = "People Counter";
                        });

                    services
                        .AddTransient<IInputSimulator, InputSimulator>()
                        .AddTransient<IStepManager, StepManager>()
                        .AddTransient<IWin32, Win32>()
                        .AddTransient<IRunner, Runner>();
                });
    }
}