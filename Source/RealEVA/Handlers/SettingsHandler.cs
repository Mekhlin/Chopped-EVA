using System;
using RealEVA.LifeSupport;
using RealEVA.Settings;

namespace RealEVA.Handlers
{
    public static class SettingsHandler
    {
        public static void ApplySettings(RealEvaModule evaModule)
        {
            try
            {
                if (evaModule == null || evaModule.part == null || !evaModule.vessel.loaded) return;
                var settings = HighLogic.CurrentGame.Parameters.CustomParams<RealEvaSettings>();
                if (!settings.EnableLifeSupport) return;
                evaModule.EnableLifeSupport = settings.EnableLifeSupport;
                evaModule.ReportMissing = settings.ReportMissing;

                // Add resources
                if (settings.RealResources == false)
                {
                    evaModule.ResourceInfo = new ResourceDef(ResourceNames.EvaLs, 1.0);
                    var amount = settings.MaxEvaTime * 60;
                    ResourceHandler.Add(evaModule, ResourceNames.EvaLs, amount);
                }
                else
                {
                    var oxygenPerSec = Convert.ToDouble(settings.OxygenPerSec);
                    var carbonDioxidePerSec = Convert.ToDouble(settings.CarbonDioxidePerSec);
                    evaModule.ResourceInfo = new ResourceDef(ResourceNames.Oxygen, oxygenPerSec);
                    ResourceHandler.Add(evaModule, ResourceNames.Oxygen, settings.OxygenAmount);
                    evaModule.OutputResourceInfo = new ResourceDef(ResourceNames.CarbonDioxide, carbonDioxidePerSec);
                    ResourceHandler.Add(evaModule, ResourceNames.CarbonDioxide, 0, settings.OxygenAmount * carbonDioxidePerSec);
                }
            }
            catch (Exception ex)
            {
                Logging.Error("Failed to apply settings", ex);
            }
        }
    }

    public static class ResourceNames
    {
        public const string EvaLs = "EvaLS"; // EVA LS
        public const string Oxygen = "EvaOxygen"; // Oxygen
        public const string CarbonDioxide = "EvaCO2"; // Carbon dioxide
    }
}