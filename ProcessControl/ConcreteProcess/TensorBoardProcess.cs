
namespace UnityControls.ProcessControl
{
    class TensorBoardProcess : EmbeddedProcess
    {

        public TensorBoardProcess()
        {
            CoreProcess = EmbeddedProcessFactory.CreateTensorboardProcess();
        }

        public override void Start()
        {
            CoreProcess.Start();
            this.AddToActiveList();
        }

        public override void Kill()
        {
            CoreProcess.Kill();
            this.RemoveFromActiveList();
        }
    }
}
