using System;
using RealEVA.LifeSupport;
using RealEVA.Settings;

namespace RealEVA.Handlers
{
    public static class SettingsHandler
    {
        public const string ResourceName = "EvaLS";

        public static void ApplySettings(RealEvaModule evaModule)
        {
            try
            {
                if (evaModule == null || evaModule.part == null || !evaModule.vessel.loaded) return;
                var properties = HighLogic.CurrentGame.Parameters.CustomParams<RealEvaSettings>();
                if (!properties.EnableLifeSupport) return;
                evaModule.EnableLifeSupport = properties.EnableLifeSupport;
                evaModule.ReportMissing = properties.ReportMissing;

                // Add resource
                var amount = properties.MaxEvaTime * 60;
                ResourceHandler.Add(evaModule, ResourceName, amount);

            }
            catch (Exception ex)
            {
                Logging.Error("Failed to apply settings", ex);
            }
        }

        public static Kerbal GetKerbal(KerbalEVA kerbalEva)
        {
            try
            {
                var crewMembers = kerbalEva.vessel.GetVesselCrew().ToArray();
                if (crewMembers.Length != 1) return null;
                var crewMember = crewMembers[0];
                var kerbal = new Kerbal
                {
                    name = kerbalEva.vessel.name,
                    veteran = crewMember.veteran
                };
                return kerbal;
            }
            catch (Exception ex)
            {
                Logging.Error("Failed to get Kerbal info", ex);
                return null;
            }
        }
    }
}