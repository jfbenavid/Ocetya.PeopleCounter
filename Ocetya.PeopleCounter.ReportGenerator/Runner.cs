namespace Ocetya.PeopleCounter.ReportGenerator
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using Ocetya.PeopleCounter.ReportGenerator.Interfaces;
    using Ocetya.PeopleCounter.ReportGenerator.Models;

    public class Runner : IRunner
    {
        private readonly IStepManager step;
        private readonly IWin32 env;
        private readonly IOptions<Point> options;

        public Runner(IStepManager step, IWin32 env, IOptions<Point> options)
        {
            this.step = step;
            this.env = env;
            this.options = options;
        }

        public async Task RunFlow()
        {
            var x = await step
                .AddStep(new SetDateStep(env, options)).Result
                .AddStep(new CreateReportStep(env));

            await x.Execute();
        }
    }
}