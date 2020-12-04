using System.Reflection;

namespace ChoppedEVA
{
    // ReSharper disable once InconsistentNaming
    public class ChoppedEVASettings : GameParameters.CustomParameterNode
    {
        #region Housekeeping

        public override string Title { get; } = "EVA Life Support";
        public override string Section { get; } = "ChoppedEva";
        public override string DisplaySection { get; } = "Chopped EVA";
        public override int SectionOrder { get; } = 0;
        public override GameParameters.GameMode GameMode { get; } = GameParameters.GameMode.ANY;
        public override bool HasPresets { get; } = false;

        #endregion

        #region Settings

        [GameParameters.CustomParameterUI("Enable EVA life support", autoPersistance = true)]
        public bool EnableLifeSupport = false;

        [GameParameters.CustomParameterUI("Respawn kerbals", toolTip = "Kerbals will be marked as missing, instead of dead", autoPersistance = true)]
        public bool Respawn = false;

        [GameParameters.CustomIntParameterUI("Life Support", maxValue = 1000, minValue = 200, stepSize = 10, toolTip = "Amount of life support available on EVA", autoPersistance = true)]
        public int LifeSupport = 700;

        [GameParameters.CustomFloatParameterUI("Life Support per second", toolTip = "Life support consumed per second", minValue = 0.4f, maxValue = 1.0f, asPercentage = false, displayFormat = "0.0")]
        public float LifeSupportPerSec = 0.7f;
        
        [GameParameters.CustomParameterUI("Notify death", toolTip = "Send a message when a kerbal dies", autoPersistance = true)]
        public bool NotifyDeath = false;

        #endregion

        public override bool Enabled(MemberInfo member, GameParameters parameters) => member.Name == nameof(EnableLifeSupport) || EnableLifeSupport;
    }
}
