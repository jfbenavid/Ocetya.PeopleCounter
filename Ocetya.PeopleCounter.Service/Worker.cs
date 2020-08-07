namespace Ocetya.PeopleCounter.Service
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Ocetya.PeopleCounter.Service.Config;
    using Ocetya.PeopleCounter.Service.Interfaces;

    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> logger;
        private readonly IReporter reporter;
        private readonly IConfiguration configuration;
        private readonly IReportBuilder reportBuilder;

        public Worker(ILogger<Worker> logger, IConfiguration configuration, IReporter reporter, IReportBuilder reportBuilder)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.reporter = reporter;
            this.reportBuilder = reportBuilder;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int delay = configuration.GetValue<int>(ConfigurationKeys.WORKER_SECONDS_DELAY);
            string uploaderPath = configuration[ConfigurationKeys.UPLOAD_REPORT_PATH];

            while (!stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                await reporter.GetAndSaveNewDataAsync();

                if (DateTime.Now.Hour == 23 && DateTime.Now.Minute == 30)
                {
                    await reporter.SendMailAsync(await reportBuilder.BuildDailyReportAsync());
                    await reporter.SendMailAsync(await reportBuilder.BuildDayComparisonReport());
                }

                if (Directory.Exists(uploaderPath) && Directory.EnumerateFileSystemEntries(uploaderPath).Any())
                {
                    await reporter.UploadReportFromDirectoryAsync(uploaderPath);
                }

                await Task.Delay(TimeSpan.FromSeconds(delay), stoppingToken);
            }
        }
    }
}