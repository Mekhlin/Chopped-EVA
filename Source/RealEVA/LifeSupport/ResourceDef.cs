namespace RealEVA.LifeSupport
{
    public class ResourceDef
    {
        public string Name { get; private set; }
        public double Multiplier { get; private set; }

        public ResourceDef(string name, double multiplier)
        {
            Name = name;
            Multiplier = multiplier;
        }
    }
}
