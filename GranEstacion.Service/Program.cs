namespace GranEstacion.Service
{
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

                    services
                        .AddHostedService<Worker>()
                        .Configure<EventLogSettings>(config =>
                        {
                            config.LogName = "People Counter Service";
                            config.SourceName = "People Counter";
                        });

                    services.AddEntityFrameworkNpgsql().AddDbContext<DbContext>(options =>
                        options.UseNpgsql(_configuration.GetConnectionString("DB")));

                    //Dependency Injection
                    services
                        .AddTransient<IReporter, Reporter>();
                }).UseWindowsService();
    }
}