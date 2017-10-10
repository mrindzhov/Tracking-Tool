namespace TrackingTool.Wpf.Domain
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;
    public class ActiveWindow
    {
        public delegate void ActiveWindowChangedHandler(object sender, String windowHeader, IntPtr hwnd);

        public event ActiveWindowChangedHandler OnActiveWindowChanged;
        public event ActiveWindowChangedHandler OnWindowMinimized;
        public event ActiveWindowChangedHandler OnWindowRestored;

        delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType,
            IntPtr hwnd, int idObject, int idChild, uint dwEventThread,
            uint dwmsEventTime);

        const uint EVENT_SYSTEM_MINIMIZESTART = 0x16;
        const uint EVENT_SYSTEM_MINIMIZEEND = 0x17;
        const uint WINEVENT_OUTOFCONTEXT = 0;
        const uint EVENT_SYSTEM_FOREGROUND = 3;

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll")]
        static extern IntPtr GetKeyboardLayout(uint thread);

        [DllImport("user32.dll")]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        static extern bool UnhookWinEvent(IntPtr hWinEventHook);

        [DllImport("user32.dll")]
        static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax,
            IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc,
            uint idProcess, uint idThread, uint dwFlags);

        IntPtr m_hhook = IntPtr.Zero;
        private WinEventDelegate _winEventProc;

        public ActiveWindow()
        {
            _winEventProc = new WinEventDelegate(WinEventProc);
        }

        public ActiveWindow(bool start) : this()
        {
            if (start)
                Start();
        }

        public void Start()
        {
            if (m_hhook == IntPtr.Zero)
            {
                m_hhook = SetWinEventHook(EVENT_SYSTEM_FOREGROUND,
                    EVENT_SYSTEM_MINIMIZEEND, IntPtr.Zero, _winEventProc,
                    0, 0, WINEVENT_OUTOFCONTEXT);
            }
        }

        public void Stop()
        {
            if (m_hhook != IntPtr.Zero)
            {
                UnhookWinEvent(m_hhook);
                m_hhook = IntPtr.Zero;
            }
        }

        void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd,
            int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            if (eventType == EVENT_SYSTEM_FOREGROUND)
            {
                OnActiveWindowChanged?.Invoke(this, GetActiveWindowTitle(hwnd), hwnd);
            }
            else if (eventType == EVENT_SYSTEM_MINIMIZEEND)
            {
                OnWindowRestored?.Invoke(this, GetActiveWindowTitle(hwnd), hwnd);
            }
            else if (eventType == EVENT_SYSTEM_MINIMIZESTART)
            {
                OnWindowMinimized?.Invoke(this, GetActiveWindowTitle(hwnd), hwnd);
            }
        }

        private string GetActiveWindowTitle(IntPtr hwnd)
        {
            StringBuilder Buff = new StringBuilder(500);
            GetWindowText(hwnd, Buff, Buff.Capacity);
            return Buff.ToString();
        }

        ~ActiveWindow()
        {
            UnhookWinEvent(m_hhook);
        }
    }
}