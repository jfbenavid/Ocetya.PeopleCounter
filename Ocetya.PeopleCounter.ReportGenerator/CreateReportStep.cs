namespace Ocetya.PeopleCounter.ReportGenerator
{
    using System.Threading.Tasks;
    using Ocetya.PeopleCounter.ReportGenerator.Interfaces;
    using Ocetya.PeopleCounter.ReportGenerator.StepPattern;

    public class CreateReportStep : IStep
    {
        private readonly IWin32 env;

        public CreateReportStep(IWin32 env)
        {
            this.env = env;
        }

        public async Task<StepResponse> Run()
        {
            env.PressTab(3)
                .PressUpArrow()
                .PressTab(3)
                .PressEnter(2);

            return await Task.FromResult(new StepResponse { ExecutedSuccessfully = true });
        }
    }
}