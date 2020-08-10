namespace Ocetya.PeopleCounter.ReportGenerator.Interfaces
{
    public interface IWin32
    {
        Win32 SetCursorPosition(int x, int y);

        Win32 MouseClick();

        Win32 PressUpArrow(int times = 1);

        Win32 PressDownArrow(int times = 1);

        Win32 PressLeftArrow(int times = 1);

        Win32 PressRightArrow(int times = 1);

        Win32 PressTab(int times = 1);

        Win32 PressEnter(int times = 1);

        Win32 InsertText(string text);
    }
}