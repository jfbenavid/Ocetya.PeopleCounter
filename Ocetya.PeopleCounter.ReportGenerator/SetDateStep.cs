namespace Ocetya.PeopleCounter.ReportGenerator
{
    using System;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using Ocetya.PeopleCounter.ReportGenerator.Extensions;
    using Ocetya.PeopleCounter.ReportGenerator.Interfaces;
    using Ocetya.PeopleCounter.ReportGenerator.Models;
    using Ocetya.PeopleCounter.ReportGenerator.StepPattern;

    public class SetDateStep : IStep
    {
        private readonly IWin32 env;
        private readonly Point point;

        [DllImport("user32.dll")]
        private static extern void SetCursorPos(int x, int y);

        public SetDateStep(IWin32 env, IOptions<Point> configuration)
        {
            this.env = env;
            point = configuration.Value;
        }

        public async Task<StepResponse> Run()
        {
            var endingDate = DateTime.Now.RoundDown(TimeSpan.FromMinutes(15));
            var startingDate = endingDate.AddMinutes(-15);

            SetCursorPos(point.X, point.Y);
            env.MouseClick();
            SetDate(startingDate);
            env.PressTab();
            SetDate(endingDate);

            return await Task.FromResult(new StepResponse { ExecutedSuccessfully = true });
        }

        private void SetDate(DateTime date)
        {
            env
                .InsertText(date.Day.ToString())
                .PressRightArrow()
                .InsertText(date.Month.ToString())
                .PressRightArrow()
                .InsertText(date.Year.ToString())
                .PressRightArrow()
                .PressTab() //hasta aqui va la fecha, ahora va la hora
                .InsertText(date.ToString("hh", CultureInfo.InvariantCulture))
                .PressRightArrow()
                .InsertText(date.Minute.ToString())
                .PressRightArrow()
                .InsertText(date.Second.ToString())
                .PressRightArrow()
                .InsertText(date.ToString("tt", CultureInfo.InvariantCulture))
                .PressRightArrow();
        }
    }
}