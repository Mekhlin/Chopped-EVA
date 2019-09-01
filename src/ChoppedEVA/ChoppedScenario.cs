using System;
using System.Linq;
using ChoppedEVA.LifeSupport;
using ChoppedEVA.Providers;
using ChoppedEVA.Settings;

namespace ChoppedEVA
{
    [KSPScenario(ScenarioCreationOptions.AddToAllGames, GameScenes.FLIGHT, GameScenes.SPACECENTER, GameScenes.TRACKSTATION)]
    public class ChoppedScenario : ScenarioModule
    {
        #region Housekeeping

        private const double SecondsPerCycle = 1.0;
        private double _cycleStartTime;

        #endregion

        private bool _enableLifeSupport;
        private double _oxygenPerSec;
        private double _carbonDioxidePerSec;
        private bool _respawn;

        public override void OnLoad(ConfigNode node)
        {
            var settings = HighLogic.CurrentGame.Parameters.CustomParams<ChoppedEVASettings>();
            _enableLifeSupport = settings.EnableLifeSupport;
            _oxygenPerSec = Convert.ToDouble(settings.OxygenPerSec);
            _carbonDioxidePerSec = Convert.ToDouble(settings.CarbonDioxidePerSec);
            _respawn = settings.Respawn;
        }

        // ReSharper disable once UnusedMember.Global
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
                    return;
                _cycleStartTime = Planetarium.GetUniversalTime();

                foreach (var vessel in FlightGlobals.Vessels)
                {
                    if (vessel.isEVA == false) continue;
                    if (HasHelmetOn(vessel))
                        ConsumeOxygen(vessel);
                }
            }
            catch (Exception ex)
            {
                Logging.Error($"{nameof(ChoppedScenario)}.{nameof(FixedUpdate)} - Failed to update scenario", ex);
            }
        }

        private void ConsumeOxygen(Vessel vessel)
        {
            if (vessel.loaded == false) return;
            var part = vessel.Parts.FirstOrDefault();
            if (part == null) return;

            var oxygen = part.Resources[ResourceProvider.Resource.Oxygen];
            if (oxygen == null) return;
            if (oxygen.amount <= _oxygenPerSec)
                CrewChopper.Chop(vessel, _respawn);
            else
            {
                oxygen.amount -= _oxygenPerSec;
                var carbonDioxide = part.Resources[ResourceProvider.Resource.CarbonDioxide];
                if (carbonDioxide is object)
                    carbonDioxide.amount += _carbonDioxidePerSec;
            }
        }

        private bool HasHelmetOn(Vessel vessel)
        {
            var crewMembers = vessel.GetVesselCrew().ToArray();
            if (crewMembers.Length != 1) return true;

            var crewMember = crewMembers[0];
            return crewMember.hasHelmetOn;
        }
    }
}