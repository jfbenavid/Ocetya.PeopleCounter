namespace Ocetya.PeopleCounter.ReportGenerator.Interfaces
{
    using System.Threading.Tasks;
    using Ocetya.PeopleCounter.ReportGenerator.StepPattern;

    public interface IStepManager
    {
        Task<StepManager> AddStep(IStep step);

        Task Execute();
    }
}