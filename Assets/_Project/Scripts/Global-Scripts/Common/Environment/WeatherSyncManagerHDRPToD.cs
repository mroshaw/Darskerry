#if HDRPTIMEOFDAY_
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using Enviro;

namespace DaftAppleGames.Common.Environment
{
    [ExecuteInEditMode]
    public class WeatherSyncManagerHDRPToD : WeatherSyncManagerBase, IWeatherSyncManager
    {
         private HDRPTimeOfDayHelper _hdrpTodHelper;

        private void Start()
        {

            _hdrpTodHelper = FindObjectOfType<HDRPTimeOfDayHelper>();
            if (_hdrpTodHelper)
            {
                if(manageSnow)
                {
                    _hdrpTodHelper.OnStartSnowing.AddListener(StartSnow);
                    _hdrpTodHelper.OnStopSnowing.AddListener(StopSnow);

                }

                if(manageWetness)
                {
                    _hdrpTodHelper.OnStartRaining.AddListener(StartRain);
                    _hdrpTodHelper.OnStopRaining.AddListener(StopRain);
                }
            }
            base.Start();
        }

        public float GetSnowLevel()
        {

        }

        public float GetWetLevel()
        {
        }

    }
}
#endif