using System;
using ChoppedEVA.LifeSupport;
using ChoppedEVA.Providers;

namespace ChoppedEVA.Modules
{
    // ReSharper disable once InconsistentNaming
    public class ModuleChoppedEVA : PartModule
    {
        #region Properties

        public bool EnableLifeSupport { get; set; }
        public bool ReportMissing { get; set; }
        public ResourceDef ResourceInfo { get; set; }
        public ResourceDef OutputResourceInfo { get; set; }

        #endregion

        public override void OnStart(StartState state)
        {
            try
            {
                if (!vessel.loaded || vessel.isEVA == false) return;
                SettingsProvider.ApplySettings(this);
                Logging.Log($"{vessel.name} is on EVA");
                UpdateResourceAmount();
            }
            catch (Exception ex)
            {
                Logging.Error("Failed to start module", ex);
            }
        }

        #region Overrides

        public override void OnLoad(ConfigNode node)
        {
            base.OnLoad(node);
            UpdateResourceAmount();
        }

        public override void OnAwake()
        {
            base.OnAwake();
            UpdateResourceAmount();
        }

        public override void OnUpdate()
        {
            try
            {
                if (EnableLifeSupport == false) return;
                UpdateResourceAmount();
            }
            catch (Exception ex)
            {
                Logging.Error("Failed to update module", ex);
            }
        }

        #endregion

        private int _prevSeconds;

        private void UpdateResourceAmount()
        {
            try
            {
                if (vessel == null || !vessel.loaded) return;

                var seconds = (int)TimeSpan.FromSeconds(vessel.missionTime).TotalSeconds;
                if (seconds == 0) return;

                var resource = part.Resources[ResourceInfo.Name];
                if (_prevSeconds == seconds - 1)
                {
                    if (HasHelmetOn())
                    {
                        if (resource is object)
                            resource.amount -= ResourceInfo.Multiplier;

                        if (OutputResourceInfo is object)
                        {
                            var outputRes = part.Resources[OutputResourceInfo.Name];
                            if (outputRes is object)
                                outputRes.amount += OutputResourceInfo.Multiplier;
                        }
                    }
                    _prevSeconds = seconds;
                }

                if (resource is object && resource.amount <= 0)
                {
                    CrewChopper.Chop(this);
                }
            }
            catch (Exception ex)
            {
                Logging.Error("Failed to update resource amount", ex);
            }
        }

        private bool HasHelmetOn()
        {
            if (vessel == null || !vessel.loaded) return true;

            var crewMembers = vessel.GetVesselCrew().ToArray();
            if (crewMembers.Length != 1) return true;

            var crewMember = crewMembers[0];
            return crewMember.hasHelmetOn;
        }
    }
}