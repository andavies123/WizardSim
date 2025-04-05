using System;
using System.ComponentModel;
using System.IO;
using AndysTools.GameWorldTimeManagement.Runtime;
using Game.Common;
using Game.Events;
using Game.Values;
using UnityEngine;
using Utilities.Attributes;

namespace Game
{
	[DisallowMultipleComponent]
	public class GameWorldTimeEventManager : MonoBehaviour
	{
		private const int DAYTIME_START_HOUR = 6;
		private const int NIGHTTIME_START_HOUR = 18;
        
		[SerializeField, Required] private GameWorldTimeBehaviour gameWorldTime;
		
		private int _previousDay;
		private int _previousHour;

		private static int CurrentDay => GameValues.Time.Day;
		private static int CurrentHour => GameValues.Time.Hour;

		private void Awake()
		{
			GameEvents.Time.ChangeGameSpeed.Requested += OnChangeGameSpeedRequested;
			GameValues.Time.PropertyChanged += OnGameTimeValuesChanged;
			GameValues.Time.PropertyChanging += OnGameTimeValuesChanging;
		}

		private void Start()
		{
			GameValues.Time.Day = gameWorldTime.Days;
			GameValues.Time.Hour = gameWorldTime.Hours;
			GameValues.Time.Minute = gameWorldTime.Minutes;
			GameValues.Time.Second = gameWorldTime.Seconds;
			GameValues.Time.GameSpeed = TimeMultiplierToGameSpeed(gameWorldTime.TimeMultiplier);
		}

		private void OnDestroy()
		{
			GameEvents.Time.ChangeGameSpeed.Requested -= OnChangeGameSpeedRequested;
			GameValues.Time.PropertyChanged -= OnGameTimeValuesChanged;
			GameValues.Time.PropertyChanging -= OnGameTimeValuesChanging;
		}

		private void Update()
		{
			// Update the global values
			GameValues.Time.Day = gameWorldTime.Days;
			GameValues.Time.Hour = gameWorldTime.Hours;
			GameValues.Time.Minute = gameWorldTime.Minutes;
			GameValues.Time.Second = gameWorldTime.Seconds;
		}

		private void OnChangeGameSpeedRequested(object sender, GameSpeedEventArgs args)
		{
			gameWorldTime.TimeMultiplier = GameSpeedToTimeMultiplier(args.GameSpeed);
			Time.timeScale = gameWorldTime.TimeMultiplier;
			GameValues.Time.GameSpeed = args.GameSpeed;
		}

		private void OnDayValueChanged()
		{
			if (CurrentDay - _previousDay == 1)
			{
				GameEvents.Time.NewGameDayStarted.Raise(this);
			}
		}

		private void OnHourValueChanged()
		{
			if (CurrentHour == DAYTIME_START_HOUR && CurrentHour - _previousHour == 1)
			{
				GameEvents.Time.DaytimeStarted.Raise(this);
			}
			else if (CurrentHour == NIGHTTIME_START_HOUR && CurrentHour - _previousHour == 1)
			{
				GameEvents.Time.NighttimeStarted.Raise(this);
			}
		}

		private void OnGameTimeValuesChanging(object sender, PropertyChangingEventArgs args)
		{
			switch (args.PropertyName)
			{
				case nameof(TimeValues.Day): _previousDay = CurrentDay; break;
				case nameof(TimeValues.Hour): _previousHour = CurrentHour; break;
			}
		}

		private void OnGameTimeValuesChanged(object sender, PropertyChangedEventArgs args)
		{
			switch (args.PropertyName)
			{
				case nameof(TimeValues.Day): OnDayValueChanged(); break;
				case nameof(TimeValues.Hour): OnHourValueChanged(); break;
			}
		}

		private static GameSpeed TimeMultiplierToGameSpeed(float timeMultiplier)
		{
			return timeMultiplier switch
			{
				0f => GameSpeed.Paused,
				1f => GameSpeed.Regular,
				2f => GameSpeed.Double,
				4f => GameSpeed.Quadruple,
				_ => throw new InvalidDataException("Unable to convert time multiplier to game speed enum")
			};
		}

		private static float GameSpeedToTimeMultiplier(GameSpeed gameSpeed)
		{
			return gameSpeed switch
			{
				GameSpeed.Paused => 0f,
				GameSpeed.Regular => 1f,
				GameSpeed.Double => 2f,
				GameSpeed.Quadruple => 4f,
				_ => throw new ArgumentOutOfRangeException()
			};
		}
	}
}