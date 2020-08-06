namespace Ocetya.PeopleCounter.ReportGenerator
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Ocetya.PeopleCounter.ReportGenerator.Config;
    using Ocetya.PeopleCounter.ReportGenerator.Interfaces;

    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        private readonly IWin32 _env;

        public Worker(ILogger<Worker> logger, IConfiguration configuration, IWin32 env)
        {
            _logger = logger;
            _configuration = configuration;
            _env = env;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                //SetCursorPos(0, 0);
                await _env.PressDownArrow();
                await _env.PressDownArrow();
                await _env.PressDownArrow();
                await _env.PressDownArrow();
                await _env.PressDownArrow();
                await _env.PressDownArrow();
                await _env.PressDownArrow();

                await Task.Delay(TimeSpan.FromSeconds(_configuration.GetValue<int>(ConfigurationKeys.WORKER_DELAY)), stoppingToken);
            }
        }
    }
}