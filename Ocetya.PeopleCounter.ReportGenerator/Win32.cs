namespace Ocetya.PeopleCounter.ReportGenerator
{
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using Ocetya.PeopleCounter.ReportGenerator.Interfaces;
    using WindowsInput;
    using WindowsInput.Native;

    public class Win32 : IWin32
    {
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private readonly IInputSimulator _sim;

        public Win32(IInputSimulator sim)
        {
            _sim = sim;
        }

        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        private async Task PressKey(VirtualKeyCode key)
        {
            await Task.Run(() =>
            {
                _sim.Keyboard
                    .KeyPress(key);
            });
        }

        public async Task MouseClick(int x, int y)
        {
            await Task.Run(() =>
            {
                mouse_event(MOUSEEVENTF_LEFTDOWN, x, y, 0, 0);
                mouse_event(MOUSEEVENTF_LEFTUP, x, y, 0, 0);
            });
        }

        public async Task PressDownArrow()
        {
            await PressKey(VirtualKeyCode.DOWN);
        }

        public async Task PressUpArrow()
        {
            await PressKey(VirtualKeyCode.UP);
        }

        public async Task PressLeftArrow()
        {
            await PressKey(VirtualKeyCode.LEFT);
        }

        public async Task PressRightArrow()
        {
            await PressKey(VirtualKeyCode.RIGHT);
        }

        public async Task InsertText(string text)
        {
            await Task.Run(() =>
            {
                var sim = new InputSimulator();
                sim.Keyboard
                    .TextEntry(text);
            });
        }
    }
}