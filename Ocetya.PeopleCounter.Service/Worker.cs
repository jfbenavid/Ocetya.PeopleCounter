namespace Ocetya.PeopleCounter.Service
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Ocetya.PeopleCounter.Service.Config;
    using Ocetya.PeopleCounter.Service.Interfaces;

    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IReporter _reporter;
        private readonly IConfiguration _configuration;
        private readonly IReportBuilder _reportBuilder;

        public Worker(ILogger<Worker> logger, IConfiguration configuration, IReporter reporter, IReportBuilder reportBuilder)
        {
            _logger = logger;
            _configuration = configuration;
            _reporter = reporter;
            _reportBuilder = reportBuilder;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int delay = _configuration.GetValue<int>(ConfigurationKeys.WORKER_SECONDS_DELAY);
            string uploaderPath = _configuration[ConfigurationKeys.UPLOAD_REPORT_PATH];

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                await _reporter.GetAndSaveNewDataAsync();

                if (DateTime.Now.Hour == 23 && DateTime.Now.Minute == 30)
                {
                    await _reporter.SendMailAsync(await _reportBuilder.BuildDailyReportAsync());
                    await _reporter.SendMailAsync(await _reportBuilder.BuildDayComparisonReport());
                }

                if (Directory.Exists(uploaderPath))
                {
                    await _reporter.UploadReportFromDirectoryAsync(uploaderPath);
                }

                await Task.Delay(TimeSpan.FromSeconds(delay), stoppingToken);
            }
        }
    }
}