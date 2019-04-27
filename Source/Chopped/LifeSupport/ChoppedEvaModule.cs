using System;
using Chopped.Settings;

namespace Chopped.LifeSupport
{
    public class ChoppedEvaModule : PartModule
    {
        //private int _minutes;
        private bool _enabled;

        public override void OnStart(StartState state)
        {
            try
            {
                if (vessel.loaded && vessel.vesselType == VesselType.EVA)
                {
                    _enabled = HighLogic.CurrentGame.Parameters.CustomParams<ChoppingProperties>().EnableChopping;
                    ChoppingProperties.ApplySettings(this);
                    Log($"{vessel.name} is on EVA");
                    UpdateResourceAmount();
                }
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
                if (_enabled == false) return;
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
                    KillCrewMember();
                    return;
                }
                resource.amount = resource.maxAmount - unitsUsed;
            }
            catch (Exception ex)
            {
                Log("Failed to update resource amount - " + ex.Message);
            }
        }

        private void KillCrewMember()
        {
            try
            {
                var crewMembers = vessel.GetVesselCrew().ToArray();
                if (crewMembers.Length != 1) return;
                ScreenMessages.PostScreenMessage($"{vessel.name} has run out of Life Support", 5.0f, ScreenMessageStyle.UPPER_CENTER);
                var doomed = crewMembers[0];
                doomed.rosterStatus = ProtoCrewMember.RosterStatus.Dead;
                part?.Die();
                vessel?.Die();
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