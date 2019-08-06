using System;
using ChoppedEVA.Modules;

namespace ChoppedEVA.LifeSupport
{
    public static class CrewChopper
    {
        public static void Chop(ModuleChoppedEVA evaModule)
        {
            try
            {
                var part = evaModule.part;
                var vessel = evaModule.vessel;
                if (part == null || vessel == null || !vessel.loaded) return;

                var crewMembers = vessel.GetVesselCrew().ToArray();
                if (crewMembers.Length != 1) return;

                var doomed = crewMembers[0];
                ScreenMessages.PostScreenMessage($"{doomed.name} has run out of Life Support", 5.0f, ScreenMessageStyle.UPPER_CENTER);
                doomed.rosterStatus = evaModule.ReportMissing ? ProtoCrewMember.RosterStatus.Missing : ProtoCrewMember.RosterStatus.Dead;
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