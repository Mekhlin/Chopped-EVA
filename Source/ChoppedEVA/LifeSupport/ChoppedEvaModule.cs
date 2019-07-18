using System;
using ChoppedEVA.Handlers;

namespace ChoppedEVA.LifeSupport
{
    public class ChoppedEvaModule : PartModule
    {
        public bool EnableLifeSupport { get; set; }
        public bool ReportMissing { get; set; }
        public ResourceDef ResourceInfo { get; set; }
        public ResourceDef OutputResourceInfo { get; set; }

        public override void OnStart(StartState state)
        {
            try
            {
                if (!vessel.loaded || vessel.isEVA == false) return;
                SettingsHandler.ApplySettings(this);
                Logging.Log($"{vessel.name} is on EVA");
                UpdateResourceAmount();
            }
            catch (Exception ex)
            {
                Logging.Error("Failed to start module", ex);
            }
        }

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

        public void UpdateResourceAmount()
        {
            try
            {
                if (vessel == null || !vessel.loaded) return;

                var seconds = (int)TimeSpan.FromSeconds(vessel.missionTime).TotalSeconds;
                if (seconds == 0) return;

                var resource = part.Resources[ResourceInfo.Name];
                if (resource.amount.Equals(resource.maxAmount - seconds * ResourceInfo.Multiplier)) return;
                if (resource.maxAmount - seconds * ResourceInfo.Multiplier <= 0)
                {
                    Chop();
                }
                else
                {
                    resource.amount = resource.maxAmount - ResourceInfo.Multiplier * seconds;
                    if (OutputResourceInfo is object)
                    {
                        var output = part.Resources[OutputResourceInfo.Name];
                        output.amount = OutputResourceInfo.Multiplier * seconds;
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.Error("Failed to update resource amount", ex);
            }
        }

        private void Chop()
        {
            try
            {
                if (part == null || vessel == null || !vessel.loaded) return;

                var crewMembers = vessel.GetVesselCrew().ToArray();
                if (crewMembers.Length != 1) return;

                var doomed = crewMembers[0];
                ScreenMessages.PostScreenMessage($"{doomed.name} has run out of Life Support", 5.0f, ScreenMessageStyle.UPPER_CENTER);
                doomed.rosterStatus = ReportMissing ? ProtoCrewMember.RosterStatus.Missing : ProtoCrewMember.RosterStatus.Dead;
                part?.Die();
                Logging.Log($"{vessel.name} has run out of Life Support");
            }
            catch (Exception ex)
            {
                Logging.Error("Failed to chop kerbal", ex);
            }
        }
    }
}