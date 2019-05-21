using ChoppedEVA.LifeSupport;

namespace ChoppedEVA.Settings
{
    static class SettingsUtils
    {
        public const string ResourceName = "EvaLS";

        public static void ApplySettings(ChoppedEvaModule evaModule)
        {
            if (evaModule == null || evaModule.part == null || !evaModule.vessel.loaded) return;
            var properties = HighLogic.CurrentGame.Parameters.CustomParams<ChoppedEvaSettings>();
            if (properties.EnableChopping)
            {
                evaModule.EnableChopping = properties.EnableChopping;
                evaModule.ReportMissing = properties.ReportMissing;

                // Add resource
                var amount = properties.MaxEvaTime * 60;
                var node = new ConfigNode("RESOURCE");
                node.AddValue("name", ResourceName);
                node.AddValue("amount", amount);
                node.AddValue("maxAmount", amount);

                evaModule.part.AddResource(node);
            }
            else
            {
                evaModule.part.RemoveResource(ResourceName);
            }
        }
    }
}
