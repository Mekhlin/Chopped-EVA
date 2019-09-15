using System.Reflection;

namespace ChoppedEVA
{
    // ReSharper disable once InconsistentNaming
    public class ChoppedEVASettings : GameParameters.CustomParameterNode
    {
        #region Housekeeping

        public override string Title { get; } = "Chopped EVA settings";
        public override string Section { get; } = "Chopped EVA";
        public override string DisplaySection { get; } = "Chopped EVA";
        public override int SectionOrder { get; } = 3;
        public override GameParameters.GameMode GameMode { get; } = GameParameters.GameMode.ANY;
        public override bool HasPresets { get; } = false;

        #endregion

        #region Settings

        [GameParameters.CustomParameterUI("Enable EVA life support", toolTip = "Can kerbals be killed during EVA", autoPersistance = true, gameMode = GameParameters.GameMode.ANY)]
        public bool EnableLifeSupport = false;

        [GameParameters.CustomParameterUI("Respawn kerbals", toolTip = "Kerbals will be marked as missing, instead of dead", autoPersistance = true, gameMode = GameParameters.GameMode.ANY)]
        public bool Respawn = false;

        [GameParameters.CustomIntParameterUI("Oxygen", maxValue = 1000, minValue = 200, stepSize = 10, toolTip = "Amount of oxygen", autoPersistance = true)]
        public int OxygenAmount = 700;

        [GameParameters.CustomFloatParameterUI("Oxygen per second", toolTip = "Amount of oxygen consumed per second", minValue = 0.8f, maxValue = 1.2f, asPercentage = false, displayFormat = "0.0")]
        public float OxygenPerSec = 0.8f;

        [GameParameters.CustomFloatParameterUI("Carbon dioxide per second", toolTip = "Amount of carbon dioxide produced per second", minValue = 0.7f, maxValue = 0.9f, asPercentage = false, displayFormat = "0.0")]
        public float CarbonDioxidePerSec = 0.7f;

        #endregion

        public override bool Enabled(MemberInfo member, GameParameters parameters) => member.Name == nameof(EnableLifeSupport) || EnableLifeSupport;
    }
}