namespace Ocetya.PeopleCounter.ReportGenerator.StepPattern
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Ocetya.PeopleCounter.ReportGenerator.Interfaces;

    public class StepManager : IStepManager
    {
        private readonly ILogger<Worker> _logger;
        private readonly Queue<IStep> _steps;

        public StepManager(ILogger<Worker> logger)
        {
            _logger = logger;
            _steps = new Queue<IStep>();
        }

        public async Task<StepManager> AddStep(IStep step)
        {
            return await Task.Run(() =>
            {
                _steps.Enqueue(step);
                return this;
            });
        }

        public async Task Execute()
        {
            foreach (var step in _steps)
            {
                var response = await step.Run();

                if (!response.ExecutedSuccessfully)
                {
                    _logger.LogError(response.Message);
                    break;
                }
            }
        }
    }
}