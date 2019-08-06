using System;

namespace ChoppedEVA.Providers
{
    public class ResourceProvider
    {
        public static void Add(PartModule module, string name, double amount, double? maxAmount = null)
        {
            try
            {
                if (!module.vessel.loaded) return;
                var node = new ConfigNode("RESOURCE");
                node.AddValue("name", name);
                node.AddValue("amount", amount);
                node.AddValue("maxAmount", maxAmount ?? amount);

                module.part.AddResource(node);
            }
            catch (Exception ex)
            {
                Logging.Error($"Failed to add resource to module - Resource:{name ?? string.Empty}", ex);
            }
        }
    }
}