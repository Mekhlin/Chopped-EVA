using System.Reflection;
using ChoppedEVA.LifeSupport;

namespace ChoppedEVA.Settings
{
    public class ChoppingProperties : GameParameters.CustomParameterNode
    {
        public const string ResourceName = "EvaLS";
        public override string Title { get; } = "Chopping Rules";
        public override string DisplaySection { get; } = "EVA chopping";
        public override string Section { get; } = "choppedEva";
        public override int SectionOrder { get; } = 3;
        public override GameParameters.GameMode GameMode { get; } = GameParameters.GameMode.ANY;
        public override bool HasPresets { get; } = false;

        [GameParameters.CustomParameterUI("Enable EVA chopping", toolTip = "Can kerbals be chopped during EVA", autoPersistance = true, gameMode = GameParameters.GameMode.ANY)]
        public bool EnableChopping = false;

        [GameParameters.CustomIntParameterUI("Max EVA time (minutes)", maxValue = 15, minValue = 1, stepSize = 1, toolTip = "How many minutes can a kerbal be on EVA, before being chopped", autoPersistance = true)]
        public int MaxEvaTime = 10;

        [GameParameters.CustomParameterUI("Missing kerbals", toolTip = "Kerbals will be marked as missing, instead of dead", autoPersistance = true, gameMode = GameParameters.GameMode.ANY)]
        public bool ReportMissing = false;

        public override bool Enabled(MemberInfo member, GameParameters parameters)
        {
            return member.Name == nameof(EnableChopping) || EnableChopping;
        }

        public static void ApplySettings(ChoppedEvaModule evaModule)
        {
            var properties = HighLogic.CurrentGame.Parameters.CustomParams<ChoppingProperties>();
            if (properties.EnableChopping)
            {
                evaModule.EnableChopping = properties.EnableChopping;
                evaModule.ReportMissing = properties.ReportMissing;

                // Add resource
                var node = new ConfigNode("RESOURCE");
                node.AddValue("name", ResourceName);
                node.AddValue("amount", properties.MaxEvaTime);
                node.AddValue("maxAmount", properties.MaxEvaTime);

                evaModule.part.AddResource(node);
            }
            else
            {
                evaModule.part.RemoveResource(ResourceName);
            }
        }
    }
}