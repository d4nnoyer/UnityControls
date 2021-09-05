using System;
using System.ComponentModel;
using System.Windows.Forms;
using UnityControls.Logging;
using UnityControls.ProcessControl;


namespace UnityControls
{

    public partial class UnityControls : Form
    {

        private readonly TensorBoardProcess _tensorBoard;
        private readonly UnityProcess _iLearningTrainspotProcess;
        private readonly UnityProcess _testSpotProcess;
        private readonly ConsoleProcess _terminal;
        private readonly CEF _chromium;


        public UnityControls()
        {
            InitializeComponent();

            var backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += backgroundWorker_DoWork;


            Logger.Log += delegate (string text)
            {
                lock (LogTextBox)
                    LogTextBox.AppendText(Environment.NewLine + text);
            };

            _chromium = new CEF();
            _terminal = new ConsoleProcess("sh", UnityAndCmdContainer.Panel2);
            _tensorBoard = new TensorBoardProcess();
            _iLearningTrainspotProcess =
                new UnityProcess("MLagentsTrainspot.exe", @"\exeIL", EmbeddingPanel);
            _testSpotProcess =
                new UnityProcess("MLagentsTrainspot.exe", @"\exeTEST", EmbeddingPanel);

            _chromium.Embed(TensorboardTab);
            _terminal.Start();
            backgroundWorker.RunWorkerAsync();


        }

        #region Events

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
            => NamedPipeUnitySession.Start(this);

        private void UnityControls_Activated(object sender, EventArgs e)
            => UnityProcess.ActivateWindow();

        private void UnityControls_Deactivate(object sender, EventArgs e)
            => UnityProcess.DeactivateWindow();

        private void UnityControls_FormClosed(object sender, FormClosedEventArgs e)
        {
            CEF.Dispose();
            EmbeddedProcessFactory.DisposeAll();
            NamedPipeUnitySession.Close();

        }

        #endregion


        #region Buttons

        private void ActivateButton_Click(object sender, EventArgs e)
            => Logger.PrintUsage();


        private void StopTraining_Click(object sender, EventArgs e)
            => UnityProcess.QuitTraining();
        

        private void BornILprocess_Click(object sender, EventArgs e)
            => _iLearningTrainspotProcess.Start();


        private void KillILprocess_Click(object sender, EventArgs e)
            => _iLearningTrainspotProcess.Kill();


        private void BornTestProcess_Click(object sender, EventArgs e)
            => _testSpotProcess.Start();


        private void KillTestProcess_Click(object sender, EventArgs e)
            => _testSpotProcess.Kill();


        private void OverrideNNmodelButton_Click(object sender, EventArgs e)
            => NamedPipeUnitySession.Message = $"overrideNNmodel?{OverrideNNmodelPathBox.Text}\\{OverrideNNmodelNameBox.Text}";



        private void ToggleLoggingButton_Click(object sender, EventArgs e)
        {
            NamedPipeUnitySession.Message = "toggleLogging";
            Logger.ToggleLogging();
        }

        private void CLeanButton(object sender, EventArgs e)
        {
            NamedPipeUnitySession.Message = "cleanLogTextBox";
            lock (LogTextBox)
                LogTextBox.Clear();
        }


        private void StartTensorBoardButton_Click(object sender, EventArgs e)
            => _tensorBoard.Start();

        private void OpenTensorBoardButton_Click_1(object sender, EventArgs e)
            => _chromium.Load("http://localhost:6006");
        


        private void ApplyConfigButton_Click(object sender, EventArgs e) 
            => YamlConfigWriter.UpdateConfigFile();

        #endregion

    }
}



