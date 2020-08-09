namespace Ocetya.PeopleCounter.ReportGenerator.StepPattern
{
    using System.Threading.Tasks;

    public interface IStep
    {
        Task<StepResponse> Run();
    }
}