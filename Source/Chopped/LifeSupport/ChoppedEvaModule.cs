using System;
using Chopped.Settings;

namespace Chopped.LifeSupport
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

                var unitsUsed = TimeSpan.FromSeconds(vessel.missionTime).Minutes;
                if (unitsUsed == 0) return;

                var resource = part.Resources[ChoppingProperties.ResourceName];
                if (resource.amount.Equals(resource.maxAmount - unitsUsed)) return;
                if (resource.amount.Equals(0) || unitsUsed >= resource.amount)
                {
                    Chop();
                    return;
                }
                resource.amount = resource.maxAmount - unitsUsed;
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
                ScreenMessages.PostScreenMessage($"{vessel.name} has run out of Life Support", 5.0f, ScreenMessageStyle.UPPER_CENTER);

                var doomed = crewMembers[0];
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
            print($"[{nameof(Chopped)}] {nameof(ChoppedEvaModule)} - {message}");
        }
    }
}