using System;
using AndysTools.GameWorldTimeManagement.Runtime;
using Game.Events;
using UnityEngine;
using Utilities.Attributes;

namespace Game
{
	public class GameWorldTimeEventManager : MonoBehaviour
	{
		private const int DAYTIME_START_HOUR = 6;
		private const int NIGHTTIME_START_HOUR = 18;
        
		[SerializeField, Required] private GameWorldTimeBehaviour gameWorldTime;
		
		private int _previousDay;
		private int _previousHour;
		private int _previousMinute;
		private int _previousSecond;

		private void Awake()
		{
			GameEvents.TimeEvents.ChangeGameSpeed.Requested += OnChangeGameSpeedRequested;
		}

		private void OnDestroy()
		{
			GameEvents.TimeEvents.ChangeGameSpeed.Requested -= OnChangeGameSpeedRequested;
		}

		private void Update()
		{
			if (_previousDay != gameWorldTime.Days)
			{
				GameEvents.TimeEvents.NewGameDayStarted.Raise(this);
				_previousDay = gameWorldTime.Days;
			}

			if (_previousHour != gameWorldTime.Hours)
			{
				if (_previousHour == DAYTIME_START_HOUR - 1 && gameWorldTime.Hours == DAYTIME_START_HOUR)
				{
					GameEvents.TimeEvents.DaytimeStarted.Raise(this);
				}
				else if (_previousHour == NIGHTTIME_START_HOUR - 1 && gameWorldTime.Hours == NIGHTTIME_START_HOUR)
				{
					GameEvents.TimeEvents.NighttimeStarted.Raise(this);
				}
				
				_previousHour = gameWorldTime.Hours;
			}
		}

		private void OnChangeGameSpeedRequested(object sender, GameSpeedEventArgs args)
		{
			float previousTimeMultiplier = gameWorldTime.TimeMultiplier;
			
			gameWorldTime.TimeMultiplier = args.GameSpeed switch
			{
				GameSpeed.Paused => 0f,
				GameSpeed.Regular => 1f,
				GameSpeed.Double => 2f,
				GameSpeed.Quadruple => 4f,
				_ => throw new ArgumentOutOfRangeException()
			};
			
			Time.timeScale = gameWorldTime.TimeMultiplier;

			if (Math.Abs(gameWorldTime.TimeMultiplier - previousTimeMultiplier) > .00001f)
			{
				GameEvents.TimeEvents.GameSpeedChanged.Raise(this, args);
			}
		}
	}
}