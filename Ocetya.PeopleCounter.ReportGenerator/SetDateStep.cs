namespace Ocetya.PeopleCounter.ReportGenerator
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Ocetya.PeopleCounter.ReportGenerator.Extensions;
    using Ocetya.PeopleCounter.ReportGenerator.Interfaces;
    using Ocetya.PeopleCounter.ReportGenerator.Models;
    using Ocetya.PeopleCounter.ReportGenerator.StepPattern;

    public class SetDateStep : IStep
    {
        private readonly ILogger<Worker> logger;
        private readonly IWin32 env;
        private readonly Point point;

        public SetDateStep(ILogger<Worker> logger, IWin32 env, IOptions<Point> configuration)
        {
            this.logger = logger;
            this.env = env;
            point = configuration.Value;
        }

        public async Task<StepResponse> Run()
        {
            try
            {
                var endingDate = DateTime.Now.RoundDown(TimeSpan.FromMinutes(15));
                var startingDate = endingDate.AddMinutes(-15);

                env.SetCursorPosition(point.X, point.Y);
                env.MouseClick();
                SetDate(startingDate);
                env.PressTab();
                SetDate(endingDate);

                logger.LogInformation("Entered dates and times at {time}", DateTime.Now);

                return await Task.FromResult(new StepResponse { ExecutedSuccessfully = true });
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new StepResponse
                {
                    ExecutedSuccessfully = false,
                    Message = $"There was an error in SetDateStep: {ex.Message} "
                });
            }
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