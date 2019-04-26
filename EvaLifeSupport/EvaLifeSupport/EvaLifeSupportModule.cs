using System;

namespace EvaLifeSupport
{
    public class EvaLifeSupportModule : PartModule
    {
        private int _minutes;

        public override void OnStart(StartState state)
        {
            try
            {
                if (vessel.loaded && vessel.vesselType == VesselType.EVA)
                {
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
                if (vessel == null || part == null || vessel.loaded == false || vessel.vesselType != VesselType.EVA) return;
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
                if (part == null || vessel == null || vessel.loaded == false) return 0;
                var resource = fromPart.Resources["EvaLS"];
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
            if (part == null || vessel == null || vessel.loaded == false) return;
            var resource = part.Resources["EvaLS"];
            if (resource != null && resource.amount >= 1) return;

            var crewMembers = vessel.GetVesselCrew().ToArray();
            Log($"Number of crewmembers = {crewMembers.Length}");
            if (crewMembers.Length != 1) return;
            ScreenMessages.PostScreenMessage($"{vessel.name} has run out of Life Support", 5.0f, ScreenMessageStyle.UPPER_CENTER);
            var doomed = crewMembers[0];

            //vessel.RemoveCrew(doomed);
            //vessel.CrewListSetDirty();
            vessel.DestroyVesselComponents();
            vessel.Die();
            doomed.rosterStatus = ProtoCrewMember.RosterStatus.Dead;
        }

        private static void Log(string message)
        {
            print($"[EvaLifeSupport] {nameof(EvaLifeSupportModule)} {message}");
        }
    }
}