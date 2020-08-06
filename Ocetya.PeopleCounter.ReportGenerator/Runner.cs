namespace Ocetya.PeopleCounter.ReportGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Ocetya.PeopleCounter.ReportGenerator.Interfaces;
    using Ocetya.PeopleCounter.ReportGenerator.StepPattern;

    public class Runner : IRunner
    {
        private readonly ILogger<Worker> _logger;
        private readonly IStepManager _step;
        private readonly IWin32 _env;

        public Runner(ILogger<Worker> logger, IStepManager step, IWin32 env)
        {
            _logger = logger;
            _step = step;
            _env = env;
        }

        public async Task RunFlow()
        {
            var x = await _step
                .AddStep(new SetDateStep(_env));

            await x.Execute();
        }
    }
}