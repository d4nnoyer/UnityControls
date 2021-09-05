using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace UnityControls.ProcessControl
{
    class UnityProcess : EmbeddedProcess
    { 
        private readonly int _pnlHandle;

        private delegate int WindowEnumProc(IntPtr hwnd, IntPtr lparam);

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        private static extern bool EnumChildWindows(IntPtr hwnd, WindowEnumProc func, IntPtr lParam);



        //private const int WmNclbuttondown = 0xA1;
        private const int WmActivate = 0x0006;
        //private readonly IntPtr _htCaption = (IntPtr)0x2;
        private static readonly IntPtr WaActive = new IntPtr(1);
        private static readonly IntPtr WaInactive = new IntPtr(0);
        public static IntPtr UnityHwnd { get; private set; } = IntPtr.Zero;


        public UnityProcess(string exeName, string exePath, Control panel)
        {
            this._pnlHandle = (int)panel.Handle;
            this.CoreProcess = EmbeddedProcessFactory.CreateUnityProcess(exeName, exePath, _pnlHandle);
        }

        private static int WindowEnum(IntPtr hwnd, IntPtr lparam)
        {
            UnityHwnd = hwnd;
            return 0;
        }
        public  bool IsUnityEmbeddingPanelBusy()
        {
            return (EmbeddedProcessFactory.EmbeddedProcessActiveList.Any(p
                => (p is UnityProcess process
                    && (p as UnityProcess)._pnlHandle == this._pnlHandle)));

        }
        

        public override  void Start()
        {

            if (IsUnityEmbeddingPanelBusy())
                return;

            try
            {
                NamedPipeUnitySession.Server.WaitForConnectionAsync();

                CoreProcess.Start();
                CoreProcess.WaitForInputIdle();

                EnumChildWindows((IntPtr)_pnlHandle, WindowEnum, IntPtr.Zero);
            }
            catch (Exception ex)
            {
                MessageBox.Show( CoreProcess.StartInfo.FileName + " failed to start: \n\n" + ex.Message + " application." +
                                 $"\nCheck if it is in {CoreProcess.StartInfo.WorkingDirectory}");
            }

            this.AddToActiveList();
        }


        public override void Kill()
        {
            
            if (!this.IsActive()) 
                return;

            CoreProcess.CloseMainWindow();
            Thread.Sleep(50);

            while (CoreProcess.HasExited == false) 
                CoreProcess.Kill();

            this.RemoveFromActiveList();
            NamedPipeUnitySession.Server.Disconnect();
        }



        public static void QuitTraining()
        {
            foreach (var process in Process.GetProcessesByName("MLagentsTrainspot"))
            {
                while (!process.HasExited)
                    process.Kill();
            }
        }

        public static void ActivateWindow()
        {
            SendMessage(UnityHwnd, WmActivate, WaActive, IntPtr.Zero);
        }

        public static void DeactivateWindow()
        {
            SendMessage(UnityHwnd, WmActivate, WaInactive, IntPtr.Zero);
        }


    }
}
