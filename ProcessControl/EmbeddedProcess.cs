using System.Diagnostics;


namespace UnityControls.ProcessControl
{
    abstract class EmbeddedProcess
    {
        public Process CoreProcess { get; protected set; }
         

        public  void AddToActiveList()
        {
            EmbeddedProcessFactory.EmbeddedProcessActiveList.Add(this);
        }

        public  void RemoveFromActiveList()
        {
            EmbeddedProcessFactory.EmbeddedProcessActiveList.Remove(this);
        }
          
        public  bool IsActive()
        {
            return EmbeddedProcessFactory.EmbeddedProcessActiveList.Contains(this);
        }


        public abstract void Start();

        public abstract void Kill();

    }
}
