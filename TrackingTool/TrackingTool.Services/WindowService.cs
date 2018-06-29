namespace TrackingTool.Services
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Text;
    using HWND = System.IntPtr;


    public static class WindowService
    {
        private delegate bool EnumWindowsProc(HWND hWnd, int lParam);

        delegate void WinEventDelegate(HWND hWinEventHook, uint eventType, HWND hwnd, int idObject, int idChild,
            uint dwEventThread, uint dwmsEventTime);

        [DllImport("USER32.DLL")]
        private static extern bool EnumWindows(EnumWindowsProc enumFunc, int lParam);

        [DllImport("USER32.DLL")]
        private static extern int GetWindowTextLength(HWND hWnd);

        [DllImport("USER32.DLL")]
        private static extern bool IsWindowVisible(HWND hWnd);

        [DllImport("USER32.DLL")]
        private static extern HWND GetShellWindow();

        [DllImport("user32.dll")]
        static extern HWND GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(HWND hWnd, StringBuilder text, int count);

        [DllImport("user32.dll")]
        static extern HWND SetWinEventHook(uint eventMin, uint eventMax, HWND hmodWinEventProc,
            WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);


        public static string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            return GetWindowText(handle, Buff, nChars) > 0
                ? Buff.ToString()
                : null;
        }

        /// <summary>Returns a dictionary that contains the handle and title of all the open windows.</summary>
        /// <returns>A dictionary that contains the handle and title of all the open windows.</returns>
        public static IDictionary<HWND, string> GetOpenWindows()
        {
            HWND shellWindow = GetShellWindow();
            Dictionary<HWND, string> windows = new Dictionary<HWND, string>();

            EnumWindowsProc enumFunc = GenerateEnumFunction(shellWindow, windows);
            EnumWindows(enumFunc, 0);

            return windows;
        }

        private static EnumWindowsProc GenerateEnumFunction(HWND shellWindow, Dictionary<HWND, string> windows) =>
            delegate (HWND hWnd, int lParam)
                {
                    if (hWnd == shellWindow) return true;
                    if (!IsWindowVisible(hWnd)) return true;

                    int length = GetWindowTextLength(hWnd);
                    if (length == 0) return true;

                    StringBuilder builder = new StringBuilder(length);
                    GetWindowText(hWnd, builder, length + 1);

                    windows[hWnd] = builder.ToString();
                    return true;

                };
    }
}