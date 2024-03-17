#if ASMDEF
#if HDRPTIMEOFDAY_
#if SOULLINK_SPAWNER
using Magique.SoulLink;
using ProceduralWorlds.HDRPTOD;

namespace DaftAppleGames.Common.Environment
{
    /// <summary>
    /// Provides an implementation of BaseGameTime for use with Procedural World's HDRP Time of Day asset
    /// </summary>
    public class HDRPToDGameTime : BaseGameTime
    {
        public override void GetComponents()
        {
            HDRPTimeOfDayAPI.GetAutoUpdateMultiplier(out bool autoUpdate, out float timeScale);
            SetDayLength((1f / timeScale) * 24f);
        } // GetComponents();

        public override bool GetSoulLinkControlsDate()
        {
            return true;
        } // GetSoulLinkControlsDate()

        public override float GetHour()
        {
            _currentHour = (int)HDRPTimeOfDayAPI.GetCurrentTime();
            return _currentHour;
        } // GetHour()

        override public void SetHour(float hour)
        {
            _currentHour = (int)hour;
            HDRPTimeOfDayAPI.SetCurrentTime(_currentHour);
        } // SetHour()

        override public void SetMinute(float minute)
        {
            float currentTime = HDRPTimeOfDayAPI.GetCurrentTime();
            float currHour = (int)currentTime;
            HDRPTimeOfDayAPI.SetCurrentTime(currHour + (60 / minute));
            _currentMinute = (int)minute;

        } // SetMinute()

        public override float GetMinute()
        {
            float currentTime = HDRPTimeOfDayAPI.GetCurrentTime();
            float currMinDec = currentTime % 1.0f;
            _currentMinute = (int)(60 * currMinDec);
            return _currentMinute;
        } // GetMinute()

        override public void Pause()
        {
            _paused = true;
            SetDayLength(0);
        } // Pause()

        override public void UnPause()
        {
            _paused = false;
            HDRPTimeOfDayAPI.GetAutoUpdateMultiplier(out bool autoUpdate, out float timeScale);
            SetDayLength((1f / timeScale) * 24f);
        } // Pause()

        override protected void FixedUpdate()
        {
            // Update the day, month, year since Gaia does not do this for itself
            UpdateDates();

            // Calculate the number of days from start of the year and update the season
            CalculateDays();
            UpdateSeason();

            UpdateSecondsAdded();
        } // FixedUpdate()
    } // class GaiaGameTime()
} // namespace Magique.SoulLink
#endif
#endif
#endif