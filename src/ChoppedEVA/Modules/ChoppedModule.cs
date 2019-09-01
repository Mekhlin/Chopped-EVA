using System;
using ChoppedEVA.Providers;

namespace ChoppedEVA.Modules
{
    public class ChoppedModule : PartModule
    {
        public override void OnStart(StartState state)
        {
            try
            {
                if (!vessel.loaded || vessel.isEVA == false) return;
                ResourceProvider.SetupDefaultResources(this);
                Logging.Log($"{vessel.name} is on EVA");
            }
            catch (Exception ex)
            {
                Logging.Error("Failed to start module", ex);
            }
        }
    }
}