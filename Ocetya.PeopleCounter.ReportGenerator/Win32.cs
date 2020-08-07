namespace Ocetya.PeopleCounter.ReportGenerator
{
    using System.Runtime.InteropServices;
    using System.Threading;
    using Ocetya.PeopleCounter.ReportGenerator.Interfaces;
    using WindowsInput;
    using WindowsInput.Native;

    public class Win32 : IWin32
    {
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private readonly IInputSimulator sim;

        public Win32(IInputSimulator sim)
        {
            this.sim = sim;
        }

        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        private void PressKey(VirtualKeyCode key, int times)
        {
            for (int i = 0; i < times; i++)
            {
                sim.Keyboard.KeyPress(key);
                Thread.Sleep(50);
            }
        }

        public Win32 MouseClick()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            return this;
        }

        public Win32 PressDownArrow(int times = 1)
        {
            PressKey(VirtualKeyCode.DOWN, times);
            return this;
        }

        public Win32 PressTab(int times = 1)
        {
            PressKey(VirtualKeyCode.TAB, times);
            return this;
        }

        public Win32 PressUpArrow(int times = 1)
        {
            PressKey(VirtualKeyCode.UP, times);
            return this;
        }

        public Win32 PressLeftArrow(int times = 1)
        {
            PressKey(VirtualKeyCode.LEFT, times);
            return this;
        }

        public Win32 PressRightArrow(int times = 1)
        {
            PressKey(VirtualKeyCode.RIGHT, times);
            return this;
        }

        public Win32 InsertText(string text)
        {
            sim.Keyboard.TextEntry(text);
            return this;
        }

        public Win32 PressEnter(int times = 1)
        {
            PressKey(VirtualKeyCode.RETURN, times);
            return this;
        }
    }
}