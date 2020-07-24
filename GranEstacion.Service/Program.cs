namespace GranEstacion.Service
{
    using GranEstacion.Repository;
    using GranEstacion.Service.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.EventLog;
    using System.IO;

    public class Program
    {
        public static IConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
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
                    optionsBuilder.UseNpgsql(_configuration.GetConnectionString("Mig"));
                    services.AddScoped(s => new GranEstacionContext(optionsBuilder.Options));

                    services
                        .AddHostedService<Worker>()
                        .Configure<EventLogSettings>(config =>
                        {
                            config.LogName = "People Counter Service";
                            config.SourceName = "People Counter";
                        });

                    //Dependency Injection
                    services
                        .AddTransient<IReporter, Reporter>();
                }).UseWindowsService();
    }
}