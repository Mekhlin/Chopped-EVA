using System;
using UniLinq;

namespace ChoppedEVA
{
    public class ResourceProvider
    {
        public static void SetupDefaultResources(ChoppedModule module)
        {
            try
            {
                var settings = HighLogic.CurrentGame.Parameters.CustomParams<ChoppedEVASettings>();

                var carbonDioxidePerSec = Convert.ToDouble(settings.CarbonDioxidePerSec);
                AddResource(module.vessel, Resource.Oxygen, settings.OxygenAmount);
                AddResource(module.vessel, Resource.CarbonDioxide, 0, settings.OxygenAmount * carbonDioxidePerSec);
            }
            catch (Exception ex)
            {
                Logging.Error($"{nameof(ResourceProvider)}.{nameof(SetupDefaultResources)} - Failed to add Life Support resources to vessel (kerbal)", ex);
            }
        }

        private static void AddResource(Vessel vessel, string name, double amount, double? maxAmount = null)
        {
            try
            {
                if (!vessel.loaded) return;
                var part = vessel.parts.First();
                var node = new ConfigNode("RESOURCE");
                node.AddValue("name", name);
                node.AddValue("amount", amount);
                node.AddValue("maxAmount", maxAmount ?? amount);

                part.AddResource(node);
            }
            catch (Exception ex)
            {
                Logging.Error($"Failed to add resource to module - Resource:{name ?? string.Empty}", ex);
            }
        }

        public static class Resource
        {
            public const string Oxygen = "Oxygen";
            public const string CarbonDioxide = "CarbonDioxide";
        }
    }
}