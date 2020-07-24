using System;
using System.Linq;
// ReSharper disable UnusedMember.Global
namespace ChoppedEVA
{
    [KSPScenario(ScenarioCreationOptions.AddToAllGames, GameScenes.FLIGHT, GameScenes.SPACECENTER, GameScenes.TRACKSTATION)]
    public class EvaScenario : ScenarioModule
    {
        #region Housekeeping

        private const double SecondsPerCycle = 1.0;
        private double _cycleStartTime;

        #endregion

        private bool _enableLifeSupport;
        private double _dischargePerSec;
        private bool _respawn;
        private static bool _notifyDeath;

        public override void OnLoad(ConfigNode node)
        {
            var settings = HighLogic.CurrentGame.Parameters.CustomParams<ChoppedEVASettings>();
            _enableLifeSupport = settings.EnableLifeSupport;
            _dischargePerSec = Convert.ToDouble(settings.DischargePerSec);
            _respawn = settings.Respawn;
            _notifyDeath = settings.NotifyDeath;
        }

        public void FixedUpdate()
        {
            try
            {
                if (_enableLifeSupport == false) return;
                if (_cycleStartTime.Equals(0))
                {
                    _cycleStartTime = Planetarium.GetUniversalTime();
                    return;
                }
                var elapsedTime = Planetarium.GetUniversalTime() - _cycleStartTime;

                if (elapsedTime < SecondsPerCycle)
                {
                    return;
                }

                _cycleStartTime = Planetarium.GetUniversalTime();

                foreach (var vessel in FlightGlobals.Vessels)
                {
                    if (vessel.isEVA == false) continue;
                    if (IsHelmetOn(vessel))
                        ConsumeCharge(vessel);
                }
            }
            catch (Exception ex)
            {
                Logging.Error($"{nameof(EvaScenario)}.{nameof(FixedUpdate)} - Failed to update scenario", ex);
            }
        }

        private void ConsumeCharge(Vessel vessel)
        {
            if (!(vessel is object)) return;
            var part = vessel.Parts.FirstOrDefault();
            if (!(part is object)) return;

            var ec = part.Resources[ResourceProvider.ElectricCharge];
            if (ec == null) return;
            if (ec.amount <= _dischargePerSec)
                Chop(vessel, _respawn);
            else
            {
                ec.amount -= _dischargePerSec;
            }
        }

        private bool IsHelmetOn(Vessel vessel)
        {
            var crewMembers = vessel.GetVesselCrew().ToArray();
            if (crewMembers.Length != 1) return true;

            var crewMember = crewMembers[0];
            return crewMember.hasHelmetOn;
        }

        public static void Chop(Vessel vessel, bool respawn)
        {
            try
            {
                if (!(vessel is object)) return;
                var part = vessel.Parts.FirstOrDefault();
                if (!(part is object)) return;

                var crewMembers = vessel.GetVesselCrew().ToArray();
                if (crewMembers.Length != 1) return;

                var doomed = crewMembers[0];

                // Kill kerbal
                doomed.rosterStatus = respawn ? ProtoCrewMember.RosterStatus.Missing : ProtoCrewMember.RosterStatus.Dead;
                part.Die();
                Logging.Log($"{vessel.name} has run out of Life Support");

                ScreenMessages.PostScreenMessage($"{doomed.name} has run out of Life Support", 5.0f, ScreenMessageStyle.UPPER_CENTER);
                if (_notifyDeath)
                {
                    ChoppedMessenger.NotifyDeath(doomed);
                }
            }
            catch (Exception ex)
            {
                Logging.Error("Failed to chop kerbal", ex);
            }
        }
    }
}
