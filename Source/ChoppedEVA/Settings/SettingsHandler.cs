using System;
using ChoppedEVA.LifeSupport;

namespace ChoppedEVA.Settings
{
    public static class SettingsHandler
    {
        public const string ResourceName = "EvaLS";

        public static void ApplySettings(ChoppedEvaModule evaModule)
        {
            try
            {
                if (evaModule == null || evaModule.part == null || !evaModule.vessel.loaded) return;
                var properties = HighLogic.CurrentGame.Parameters.CustomParams<ChoppedEvaSettings>();
                if (!properties.EnableChopping) return;
                evaModule.EnableChopping = properties.EnableChopping;
                evaModule.ReportMissing = properties.ReportMissing;

                // Add resource
                var amount = properties.MaxEvaTime * 60;
                AddResource(evaModule, amount);

            }
            catch (Exception ex)
            {
                Logging.Error("Failed to apply settings to ChoppedEvaModule", ex);
            }
        }

        private static void AddResource(ChoppedEvaModule evaModule, int amount)
        {
            if (!evaModule.vessel.loaded) return;
            var node = new ConfigNode("RESOURCE");
            node.AddValue("name", ResourceName);
            node.AddValue("amount", amount);
            node.AddValue("maxAmount", amount);

            evaModule.part.AddResource(node);
        }
    }
}
