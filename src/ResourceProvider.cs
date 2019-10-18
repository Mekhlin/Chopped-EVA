namespace ChoppedEVA
{
    public static class ResourceProvider
    {
        public const string ElectricCharge = "ElectricCharge";

        public static ConfigNode CreateResource(string name, int amount, int? maxAmount = null)
        {
            var node = new ConfigNode("RESOURCE");
            node.AddValue("name", name);
            node.AddValue("amount", amount);
            node.AddValue("maxAmount", maxAmount ?? amount);
            return node;
        }
    }
}
