using System;
using static UnityControls.UnityControls;

namespace UnityControls.Logging
{
    static class Logger
    {
        private static Action<string> _log;

        public static Action<string> Log
        {
            get => !IsLoggingEnabled ? null : _log;

            internal set => _log = value;
        }

        public static bool IsLoggingEnabled { get; set; } = true;

        public static bool ToggleLogging()
            => IsLoggingEnabled = !IsLoggingEnabled;


        public static void PrintUsage()
        {
            Log("\nUse this command to activate pytorch-venv: \n" +
                       "source ./venv/scripts/activate\n" +
                       "\nUse this command to start Unity ML-environment:\n" +
                       "mlagents-learn config/MoveToGoal.yaml " +
                       "--env=\"exeRL/MLagentsTrainspot.exe\" " +
                       $"{EmbeddingPanel.Width} {EmbeddingPanel.Height} --run-id=ExeInsideWinForm " +
                       "--force --env-args " +
                       $"-parentHWND {EmbeddingPanel.Handle.ToInt32()}");
        }



    }
}
 