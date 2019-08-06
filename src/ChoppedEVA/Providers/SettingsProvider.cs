using System;
using ChoppedEVA.LifeSupport;
using ChoppedEVA.Modules;
using ChoppedEVA.Settings;

namespace ChoppedEVA.Providers
{
    public static class SettingsProvider
    {
        public static void ApplySettings(ModuleChoppedEVA evaModule)
        {
            try
            {
                if (evaModule == null || evaModule.part == null || !evaModule.vessel.loaded) return;
                var settings = HighLogic.CurrentGame.Parameters.CustomParams<ChoppedEVASettings>();
                if (!settings.EnableLifeSupport) return;
                evaModule.EnableLifeSupport = settings.EnableLifeSupport;
                evaModule.ReportMissing = settings.ReportMissing;

                // Add resources
                var oxygenPerSec = Convert.ToDouble(settings.OxygenPerSec);
                var carbonDioxidePerSec = Convert.ToDouble(settings.CarbonDioxidePerSec);
                evaModule.ResourceInfo = new ResourceDef(ResourceNames.Oxygen, oxygenPerSec);
                ResourceProvider.Add(evaModule, ResourceNames.Oxygen, settings.OxygenAmount);
                evaModule.OutputResourceInfo = new ResourceDef(ResourceNames.CarbonDioxide, carbonDioxidePerSec);
                ResourceProvider.Add(evaModule, ResourceNames.CarbonDioxide, 0, settings.OxygenAmount * carbonDioxidePerSec);
            }
            catch (Exception ex)
            {
                Logging.Error("Failed to apply settings to module", ex);
            }
        }
    }

    public static class ResourceNames
    {
        public const string Oxygen = "Oxygen"; // Oxygen
        public const string CarbonDioxide = "CarbonDioxide"; // Carbon dioxide
    }
}