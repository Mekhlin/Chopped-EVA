namespace Chopped.Settings
{
    public class ChoppingProperties : GameParameters.CustomParameterNode
    {
        public const string ResourceName = "EvaLS";

        [GameParameters.CustomParameterUI("Enable chopping", toolTip = "Can kerbals be chopped during EVA", autoPersistance = true, gameMode = GameParameters.GameMode.ANY)]
        public bool EnableChopping = true;

        [GameParameters.CustomIntParameterUI("Max EVA time", maxValue = 10, minValue = 1, stepSize = 1, toolTip = "How many minutes can a kerbal be on EVA, before being chopped", autoPersistance = true)]
        public int MaxEvaTime = 10;

        public override string Title { get; } = "Chopping Rules";
        public override string DisplaySection { get; } = "EVA chopping";
        public override string Section { get; } = "choppedEva";
        public override int SectionOrder { get; } = 3;
        public override GameParameters.GameMode GameMode { get; } = GameParameters.GameMode.ANY;
        public override bool HasPresets { get; } = false;

        public static void ApplySettings(PartModule partModule)
        {
            var properties = HighLogic.CurrentGame.Parameters.CustomParams<ChoppingProperties>();
            if (properties.EnableChopping)
            {
                var node = new ConfigNode("RESOURCE");
                node.AddValue("name", ResourceName);
                node.AddValue("amount", properties.MaxEvaTime);
                node.AddValue("maxAmount", properties.MaxEvaTime);

                partModule.part.AddResource(node);
            }
            else
            {
                partModule.part.RemoveResource(ResourceName);
            }
        }
    }
}