namespace Ocetya.PeopleCounter.ReportGenerator
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Ocetya.PeopleCounter.ReportGenerator.Config;
    using Ocetya.PeopleCounter.ReportGenerator.Extensions;
    using Ocetya.PeopleCounter.ReportGenerator.Interfaces;

    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> logger;
        private readonly IConfiguration configuration;
        private readonly IRunner runner;

        public Worker(ILogger<Worker> logger, IConfiguration configuration, IRunner runner)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.runner = runner;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var nextExecution = TimeSpan.FromSeconds(15);
            while (!stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(nextExecution, stoppingToken);

                await runner.RunFlow();

                nextExecution = TimeSpan
                    .FromMinutes(configuration.GetValue<int>(ConfigurationKeys.WORKER_DELAY))
                    .NextTimeSpan();
            }
        }
    }
}