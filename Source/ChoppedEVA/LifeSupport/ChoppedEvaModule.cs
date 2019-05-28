using System;
using ChoppedEVA.Settings;

namespace ChoppedEVA.LifeSupport
{
    public class ChoppedEvaModule : PartModule
    {
        public bool EnableChopping { get; set; }
        public bool ReportMissing { get; set; }

        public override void OnStart(StartState state)
        {
            try
            {
                if (!vessel.loaded || vessel.vesselType != VesselType.EVA) return;
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
                if (EnableChopping == false) return;
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

                var resource = part.Resources[ChoppedEvaSettings.ResourceName];
                if (!resource.amount.Equals(resource.maxAmount - seconds))
                {
                    resource.amount = resource.maxAmount - seconds;
                }

                if (!resource.amount.Equals(0)) return;
                Chop();
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
                Logging.Error("Failed to kill crew member", ex);
            }
        }
    }
}