namespace Ocetya.PeopleCounter.ReportGenerator
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Ocetya.PeopleCounter.ReportGenerator.Interfaces;
    using Ocetya.PeopleCounter.ReportGenerator.Models;
    using Ocetya.PeopleCounter.ReportGenerator.StepPattern;

    public class CreateReportStep : IStep
    {
        private readonly ILogger<Worker> logger;
        private readonly ButtonPoint point;
        private readonly IWin32 env;

        public CreateReportStep(ILogger<Worker> logger, IWin32 env, IOptions<ButtonPoint> options)
        {
            this.logger = logger;
            this.env = env;
            point = options.Value;
        }

        public async Task<StepResponse> Run()
        {
            try
            {
                env.PressTab(3)
                    .PressUpArrow()
                    .PressTab(3)
                    .SetCursorPosition(point.X, point.Y)
                    .MouseClick()
                    .PressEnter();

                logger.LogInformation("Generated report at {time}", DateTime.Now);

                return await Task.FromResult(new StepResponse { ExecutedSuccessfully = true });
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new StepResponse
                {
                    ExecutedSuccessfully = false,
                    Message = $"There was an error in CreateReportStep: {ex.Message} "
                });
            }
        }
    }
}