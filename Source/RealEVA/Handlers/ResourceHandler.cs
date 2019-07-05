namespace RealEVA.Handlers
{
    public class ResourceHandler
    {
        public static void Add(PartModule module, string name, double amount, double? maxAmount = null)
        {
            if (!module.vessel.loaded) return;
            var node = new ConfigNode("RESOURCE");
            node.AddValue("name", name);
            node.AddValue("amount", amount);
            node.AddValue("maxAmount", maxAmount ?? amount);

            module.part.AddResource(node);
        }
    }
}