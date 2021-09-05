using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace UnityControls.ProcessControl
{
    class ConsoleProcess : EmbeddedProcess
    {
        private readonly Panel _panel;
          
        private const int GwlStyle = -16;
        private const int WsVisible = 0x10000000;

        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, int wFlags);

        public ConsoleProcess(string shell, Panel panel)
        {
            this.CoreProcess = EmbeddedProcessFactory.CreateConsoleProcess(shell);
            this._panel = panel;
        }

        public override void Start()
        {
            CoreProcess.Start();

            while (CoreProcess.MainWindowHandle == (IntPtr)(0)) Thread.Sleep(5);

            SetParent(CoreProcess.MainWindowHandle, _panel.Handle);
            SetWindowLong(CoreProcess.MainWindowHandle, GwlStyle, WsVisible);
            //MoveWindow(_terminal.MainWindowHandle, 0, 0, UnityAndCmdContainer.Panel2.Width, UnityAndCmdContainer.Panel2.Height, false);
            SetWindowPos(CoreProcess.MainWindowHandle, (IntPtr)(1), 0, 0, _panel.Width, _panel.Height, 0x0040);
            this.AddToActiveList();

        }

        public override void Kill()
        {
            CoreProcess.CloseMainWindow();

            while (CoreProcess.HasExited == false) 
                CoreProcess.Kill();
        }
    }
}
