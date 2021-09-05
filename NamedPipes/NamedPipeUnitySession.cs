using System;
using System.IO.Pipes;
using System.Threading;
using System.Windows.Forms;
using UnityControls.Logging;

namespace UnityControls
{
    public static class NamedPipeUnitySession
    { 
  
        public static string Message { get; internal set; }

        public static readonly NamedPipeServerStream Server;

        static NamedPipeUnitySession()
        {
            Server = new NamedPipeServerStream("UnityPipe", PipeDirection.InOut, 1);
        }

        public static void Start(Control control)
        {

            StreamString ss = new StreamString(Server);
            while (true)
            {
                try
                {
                    while (Server.IsConnected == false) Thread.Sleep(30);
                    control.Invoke(Logger.Log, "Server streamString initialized");
                    while (Server.IsConnected)
                    {
                        if (!string.IsNullOrEmpty(Message))
                        {
                            ss.WriteString(Message);
                            control.Invoke(Logger.Log, "Out message: " + Message);
                            Message = "";
                        }
                        Thread.Sleep(30);
                    }

                    control.Invoke(Logger.Log, "Server stream has disconnected");

                    Server.WaitForConnection();

                }
                catch (Exception ex)
                {
                    control.Invoke(Logger.Log, ex.Message);
                    Message = "";
                }
            }
        }


        public static void Close()
        {
            Server.Close();
        }
    }
}
