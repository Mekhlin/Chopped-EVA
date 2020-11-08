// ReSharper disable UnusedMember.Global
namespace ChoppedEVA
{
    [KSPAddon(KSPAddon.Startup.AllGameScenes, false)]
    internal class EvaEventHandler : BlankMonoBehaviour
    {
        public void Awake()
        {
            GameEvents.onCrewOnEva.Remove(CrewOnEva);
            GameEvents.onCrewOnEva.Add(CrewOnEva);
        }

        public void OnDestroy()
        {
            GameEvents.onCrewOnEva.Remove(CrewOnEva);
        }

        private void CrewOnEva(GameEvents.FromToAction<Part, Part> data)
        {
            var settings = HighLogic.CurrentGame.Parameters.CustomParams<ChoppedEVASettings>();
            if (data.to.isVesselEVA == false || settings.EnableLifeSupport == false)
            {
                return;
            }

            var electricCharge = ResourceProvider.CreateResource(ResourceProvider.ElectricCharge, settings.ElectricCharge);
            data.to.AddResource(electricCharge);
        }
    }
}
