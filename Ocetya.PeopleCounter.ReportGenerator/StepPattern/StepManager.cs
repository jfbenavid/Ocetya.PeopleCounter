namespace Ocetya.PeopleCounter.ReportGenerator.StepPattern
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Ocetya.PeopleCounter.ReportGenerator.Interfaces;

    public class StepManager : IStepManager
    {
        private readonly ILogger<Worker> logger;
        private readonly Queue<IStep> steps;

        public StepManager(ILogger<Worker> logger)
        {
            this.logger = logger;
            steps = new Queue<IStep>();
        }

        public async Task<StepManager> AddStep(IStep step)
        {
            return await Task.Run(() =>
            {
                steps.Enqueue(step);
                return this;
            });
        }

        public async Task Execute()
        {
            while (steps.Count > 0)
            {
                var step = steps.Dequeue();
                var response = await step.Run();

                if (!response.ExecutedSuccessfully)
                {
                    logger.LogError(response.Message);
                    break;
                }
            }
        }
    }
}