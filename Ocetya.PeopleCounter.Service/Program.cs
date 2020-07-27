namespace Ocetya.PeopleCounter.Service
{
    using Ocetya.PeopleCounter.Repository;
    using Ocetya.PeopleCounter.Service.Config;
    using Ocetya.PeopleCounter.Service.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.EventLog;
    using System.IO;
    using System;

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
                    var _configuration = LoadConfiguration();

                    var optionsBuilder = new DbContextOptionsBuilder<GranEstacionContext>();
                    optionsBuilder.UseNpgsql(_configuration.GetConnectionString(ConnectionStrings.MIGRATION));
                    services.AddScoped(s => new GranEstacionContext(optionsBuilder.Options));

                    services.Configure<MailConfiguration>(_configuration.GetSection(ConfigurationKeys.MAIL_CONFIGURATION));

                    services
                        .AddHostedService<Worker>()
                        .Configure<EventLogSettings>(config =>
                        {
                            config.LogName = "People Counter Service";
                            config.SourceName = "People Counter";
                        });

                    //Dependency Injection
                    services
                        .AddTransient<IReporter, Reporter>()
                        .AddTransient<IReportBuilder, ReportBuilder>();
                }).UseWindowsService();
    }
}