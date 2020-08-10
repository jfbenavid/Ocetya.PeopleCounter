namespace Ocetya.PeopleCounter.ReportGenerator
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Ocetya.PeopleCounter.ReportGenerator.Interfaces;
    using Ocetya.PeopleCounter.ReportGenerator.Models;

    public class Runner : IRunner
    {
        private readonly ILogger<Worker> logger;
        private readonly IStepManager step;
        private readonly IWin32 env;
        private readonly IOptions<Point> startingPointOptions;
        private readonly IOptions<ButtonPoint> buttonOptions;

        public Runner(ILogger<Worker> logger, IStepManager step, IWin32 env, IOptions<Point> startingPointOptions, IOptions<ButtonPoint> buttonOptions)
        {
            this.logger = logger;
            this.step = step;
            this.env = env;
            this.startingPointOptions = startingPointOptions;
            this.buttonOptions = buttonOptions;
        }

        public async Task RunFlow()
        {
            var x = await step
                .AddStep(new SetDateStep(logger, env, startingPointOptions)).Result
                .AddStep(new CreateReportStep(logger, env, buttonOptions));

            await x.Execute();
        }
    }
}