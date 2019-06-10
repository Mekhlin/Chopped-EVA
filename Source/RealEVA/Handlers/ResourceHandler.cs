namespace RealEVA.Handlers
{
    public class ResourceHandler
    {
        public static void Add(PartModule module, string name, int amount)
        {
            if (!module.vessel.loaded) return;
            var node = new ConfigNode("RESOURCE");
            node.AddValue("name", name);
            node.AddValue("amount", amount);
            node.AddValue("maxAmount", amount);

            module.part.AddResource(node);
        }
    }
}