namespace Ocetya.PeopleCounter.ReportGenerator.Interfaces
{
    using System.Threading.Tasks;

    public interface IWin32
    {
        Task MouseClick(int x, int y);

        Task PressUpArrow();

        Task PressDownArrow();

        Task PressLeftArrow();

        Task PressRightArrow();

        Task InsertText(string text);
    }
}