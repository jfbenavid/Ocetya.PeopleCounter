namespace Ocetya.PeopleCounter.ReportGenerator
{
    using System;
    using System.Drawing;
    using System.Threading.Tasks;
    using Ocetya.PeopleCounter.ReportGenerator.Interfaces;
    using Ocetya.PeopleCounter.ReportGenerator.StepPattern;

    public class SetDateStep : IStep
    {
        private readonly IWin32 _env;
        private const int X_FIRST = 130;
        private const int Y_STARTING = 150;
        private const int Y_ENDING = 180;
        private Point point = new Point(X_FIRST, Y_STARTING);

        public SetDateStep(IWin32 env)
        {
            _env = env;
        }

        public async Task<StepResponse> Run()
        {
            await SetDate();
            await Task.Delay(TimeSpan.FromSeconds(10));
            await SetDate();
            return await Task.FromResult(new StepResponse { ExecutedSuccessfully = true });
        }

        private async Task SetDate()
        {
            await _env.MouseClick(point.X, point.Y);
        }
    }
}