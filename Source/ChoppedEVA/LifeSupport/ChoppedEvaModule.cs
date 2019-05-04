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
                ChoppingProperties.ApplySettings(this);
                Log($"{vessel.name} is on EVA");
                UpdateResourceAmount();
            }
            catch (Exception ex)
            {
                Log("Failed to start module. Exception = " + ex.Message);
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
                Log("Failed to update module. Exception = " + ex.Message);
            }
        }

        public void UpdateResourceAmount()
        {
            try
            {
                if (vessel == null || !vessel.loaded) return;

                var minutes = TimeSpan.FromSeconds(vessel.missionTime).Minutes;
                if (minutes == 0) return;

                var resource = part.Resources[ChoppingProperties.ResourceName];
                if (!resource.amount.Equals(resource.maxAmount - minutes))
                {
                    resource.amount = resource.maxAmount - minutes;
                }

                if (!resource.amount.Equals(0)) return;
                Chop();
            }
            catch (Exception ex)
            {
                Log("Failed to update resource amount - " + ex.Message);
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
            }
            catch (Exception ex)
            {
                Log("Failed to kill crew member - " + ex.Message);
            }
        }

        private static void Log(string message)
        {
            print($"[{nameof(ChoppedEVA)}] {nameof(ChoppedEvaModule)} - {message}");
        }
    }
}