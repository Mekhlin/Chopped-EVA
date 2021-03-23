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

        private static void CrewOnEva(GameEvents.FromToAction<Part, Part> data)
        {
            var settings = HighLogic.CurrentGame.Parameters.CustomParams<ChoppedEVASettings>();
            if (data.to.isVesselEVA == false || settings.EnableLifeSupport == false)
            {
                return;
            }

            var lifeSupport = ResourceProvider.CreateResource(ResourceProvider.LifeSupport, settings.LifeSupport);
            data.to.AddResource(lifeSupport);
        }
    }
}
