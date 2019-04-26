using System;
using Chopped.Settings;

namespace Chopped.LifeSupport
{
    public class ChoppedEvaModule : PartModule
    {
        private int _minutes;
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
                }
            }
            catch (Exception ex)
            {
                Log("Failed to start module. Exception = " + ex.Message);
            }
        }

        public override void OnUpdate()
        {
            try
            {
                if (_enabled == false) return;
                if (vessel == null || part == null || !vessel.loaded || vessel.vesselType != VesselType.EVA) return;
                if (TimeSpan.FromSeconds(vessel.missionTime).Minutes != _minutes + 1)
                {
                    CheckHealth();
                    return;
                }

                _minutes++;
                Log($"{vessel.name} has been on EVA in {_minutes} minutes");
                var units = GetResource(part, 1);
                Log($"{units} EVA LS units supplied"); 

                CheckHealth();
            }
            catch (Exception ex)
            {
                Log("Failed to update module. Exception = " + ex.Message);
            }
        }

        public double GetResource(Part fromPart, double demand)
        {
            try
            {
                if (!_enabled || part == null || vessel == null || !vessel.loaded) return 0;
                var resource = fromPart.Resources[ChoppingProperties.ResourceName];
                double supplied = 0;

                if (resource.flowState == false)
                    return supplied;

                if (resource.amount >= demand)
                {
                    resource.amount -= demand;
                    supplied += demand;
                }

                else
                {
                    supplied += resource.amount;
                    resource.amount = 0;
                }

                return supplied;
            }
            catch (Exception ex)
            {
                Log("Failed to take resource from vessel. Exception = " + ex.Message);
                return 0;
            }
        }

        private void CheckHealth()
        {
            if (!_enabled || part == null || vessel == null || !vessel.loaded) return;
            var resource = part.Resources[ChoppingProperties.ResourceName];
            if (resource != null && resource.amount >= 1) return;

            var crewMembers = vessel.GetVesselCrew().ToArray();
            if (crewMembers.Length != 1) return;
            ScreenMessages.PostScreenMessage($"{vessel.name} has run out of Life Support", 5.0f, ScreenMessageStyle.UPPER_CENTER);
            var doomed = crewMembers[0];
            part?.Die();
            vessel?.Die();
            doomed.rosterStatus = ProtoCrewMember.RosterStatus.Dead;
        }

        private static void Log(string message)
        {
            print($"[{nameof(Chopped)}] {nameof(ChoppedEvaModule)} - {message}");
        }
    }
}