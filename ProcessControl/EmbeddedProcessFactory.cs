using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;


namespace UnityControls.ProcessControl
{
    static class EmbeddedProcessFactory
    {

        public static List<EmbeddedProcess> EmbeddedProcessActiveList { get; }

        static EmbeddedProcessFactory()
        {
            EmbeddedProcessActiveList = new List<EmbeddedProcess>();
        }


        public static Process CreateUnityProcess(string exeName, string exePath, int panelHandle)
        {
            return new Process
            { 
                StartInfo =
                {
                    FileName = exeName,
                    Arguments =
                        "-parentHWND " + panelHandle + " " + Environment.CommandLine,
                    UseShellExecute = true,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Maximized,
                    WorkingDirectory = Directory.GetCurrentDirectory() + exePath
                }
            };
        }

        public static Process CreateConsoleProcess(string shell)
        {

            return new Process()
            {
                StartInfo =
                {
                    FileName = shell,
                    UseShellExecute = false,
                    CreateNoWindow = false,
                    RedirectStandardInput = false,
                    RedirectStandardOutput = false,
                    RedirectStandardError = false
                }
            };
        }

        public static Process CreateTensorboardProcess()
        {
            return new Process
            {
                StartInfo =
                {
                    FileName = "tensorboard.exe",
                    Arguments =
                        "--logdir " + Directory.GetCurrentDirectory() + @"\results --port 6006",
                    UseShellExecute = true,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    WorkingDirectory = Directory.GetCurrentDirectory() + @"\venv\Scripts"
                }
            };
        }


        public static void DisposeAll()
        {
            foreach (var process in EmbeddedProcessActiveList) 
            {

                process.CoreProcess.CloseMainWindow();

                while (process.CoreProcess.HasExited == false)
                    process.CoreProcess.Kill();
            }
        }

    }
}
