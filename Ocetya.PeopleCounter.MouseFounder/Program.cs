namespace Ocetya.PeopleCounter.MouseFounder
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Timers;

    internal class Program
    {
        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(ref Point lpPoint);

        private static void Main(string[] args)
        {
            Timer timer = new Timer(TimeSpan.FromSeconds(5).TotalMilliseconds);
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;

            Console.WriteLine("\nPress the Enter key to exit the application...\n");
            Console.WriteLine("The application started at {0:HH:mm:ss.fff}", DateTime.Now);
            Console.ReadLine();
            timer.Stop();
            timer.Dispose();
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            var pt = new Point();
            GetCursorPos(ref pt);

            Console.WriteLine("X={0}, Y={1}", pt.X, pt.Y);
        }
    }
}