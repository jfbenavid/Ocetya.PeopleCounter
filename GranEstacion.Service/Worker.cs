namespace GranEstacion.Service
{
    using GranEstacion.Service.Config;
    using GranEstacion.Service.Interfaces;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IReporter _reporter;
        private readonly IConfiguration _configuration;

        public Worker(ILogger<Worker> logger, IConfiguration configuration, IReporter reporter)
        {
            _logger = logger;
            _configuration = configuration;
            _reporter = reporter;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int delay = _configuration.GetValue<int>(ConfigurationKeys.WORKER_SECONDS_DELAY);

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                await _reporter.GetAttachedFileAsync();

                await Task.Delay(TimeSpan.FromSeconds(delay), stoppingToken);
            }
        }
    }
}