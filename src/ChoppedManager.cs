// ReSharper disable UnusedMember.Global
namespace ChoppedEVA
{
    [KSPAddon(KSPAddon.Startup.AllGameScenes, false)]
    class ChoppedManager : BlankMonoBehaviour
    {
        public void Awake()
        {
            GameEvents.onCrewOnEva.Remove(OnEvaStart);
            GameEvents.onCrewOnEva.Add(OnEvaStart);
        }

        public void OnDestroy()
        {
            GameEvents.onCrewOnEva.Remove(OnEvaStart);
        }

        private void OnEvaStart(GameEvents.FromToAction<Part, Part> data)
        {
            var settings = HighLogic.CurrentGame.Parameters.CustomParams<ChoppedEVASettings>();
            if (data.to.isVesselEVA == false || settings.EnableLifeSupport == false) return;

            var electricCharge = ResourceProvider.CreateResource(ResourceProvider.ElectricCharge, settings.ElectricCharge);
            data.to.AddResource(electricCharge);
        }
    }
}
