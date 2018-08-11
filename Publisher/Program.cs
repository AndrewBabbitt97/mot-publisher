using System;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MOT.Publisher
{
    /// <summary>
    /// The main program
    /// </summary>
    class Program
    {
        [DllImport("user32.dll")]
        static extern bool EnumThreadWindows(int dwThreadId, EnumThreadWindowsDelegate lpfn, IntPtr lParam);
        delegate bool EnumThreadWindowsDelegate(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        /// <summary>
        /// The entry point
        /// </summary>
        /// <param name="args">The command line arguments</param>
        /// <returns>The exit code</returns>
        static int Main(string[] args)
        {
            Timer timer = new Timer(new TimerCallback(BringToFront), null, 10, 10);
            CommandLineParser parser = new CommandLineParser();
            return parser.Parse(args);
        }

        /// <summary>
        /// Brings any Publisher windows to the front
        /// </summary>
        /// <param name="state">The state</param>
        static void BringToFront(object state)
        {
            List<IntPtr> windows = new List<IntPtr>();

            foreach (ProcessThread thread in Process.GetCurrentProcess().Threads)
            {
                EnumThreadWindows(thread.Id, (hWnd, lParam) => { windows.Add(hWnd); return true; }, IntPtr.Zero);
            }

            foreach (IntPtr window in windows)
            {
                SetWindowPos(window, new IntPtr(-1), 0, 0, 0, 0, 43u);
            }
        }
    }
}
