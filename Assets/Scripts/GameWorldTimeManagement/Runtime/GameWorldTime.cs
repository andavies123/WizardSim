using UnityEngine;

namespace AndysTools.GameWorldTimeManagement.Runtime
{
	/// <summary>
	/// Keeps track of a game world time system by converting real world time
	/// into game time and incrementing internal values to calculate the day/minutes/seconds
	/// </summary>
	public class GameWorldTime : IGameWorldTime
	{
		private readonly float _toWorldTimeConversionFactor; // Precalculated conversion value
		private readonly Object _debugLogContext; // Value only used for debugging
		
		private int _hours = 0; // Property backing value for "Hours"
		private int _minutes = 0; // Property backing value for "Minutes"
		private float _seconds = 0; // Property backing value for "Seconds"
		
		public GameWorldTime(float realWorldSecondsPerWorldDay, Object debugLogContext = null)
		{
			_toWorldTimeConversionFactor = RealWorldTimeConstants.SECONDS_PER_DAY / realWorldSecondsPerWorldDay;
			_debugLogContext = debugLogContext;
		}

		public float TimeMultiplier { get; set; } = 1.0f;
		
		public float DeltaTime { get; private set; }
		public float TotalSeconds { get; private set; }
		
		public int Days { get; private set; }

		public int Hours
		{
			get => _hours;
			private set
			{
				_hours = value;
				if (_hours >= RealWorldTimeConstants.HOURS_PER_DAY)
				{
					Days += _hours / RealWorldTimeConstants.HOURS_PER_DAY;
					_hours %= RealWorldTimeConstants.HOURS_PER_DAY;
				}
			}
		}

		public int Minutes
		{
			get => _minutes;
			private set
			{
				_minutes = value;
				if (_minutes >= RealWorldTimeConstants.MINUTES_PER_HOUR)
				{
					Hours += _minutes / RealWorldTimeConstants.MINUTES_PER_HOUR;
					_minutes %= RealWorldTimeConstants.MINUTES_PER_HOUR;
				}
			}
		}

		public float Seconds
		{
			get => _seconds;
			private set
			{
				_seconds = value;
				if (_seconds >= RealWorldTimeConstants.SECONDS_PER_MINUTE)
				{
					Minutes += (int)(_seconds / RealWorldTimeConstants.SECONDS_PER_MINUTE);
					_seconds %= RealWorldTimeConstants.SECONDS_PER_MINUTE;
				}
			}
		}

		public void SetCurrentTime(int day, int hour, int minute, float second)
		{
			if (Debug.isDebugBuild)
			{
				if (day < 0) Debug.LogWarning($"Parameter {nameof(day)} has a value of {day}. Value should be >= 0. Clamping to 0...", _debugLogContext);
				if (hour < 0) Debug.LogWarning($"Parameter {nameof(hour)} has a value of {hour}. Value should be >= 0. Clamping to 0...", _debugLogContext);
				if (minute < 0) Debug.LogWarning($"Parameter {nameof(minute)} has a value of {minute}. Value should be >= 0. Clamping to 0...", _debugLogContext);
				if (second < 0) Debug.LogWarning($"Parameter {nameof(second)} has a value of {second}. Value should be >= 0. Clamping to 0...", _debugLogContext);
			}
			
			Days = Mathf.Max(day, 0);
			Hours = Mathf.Max(hour, 0);
			Minutes = Mathf.Max(minute, 0);
			Seconds = Mathf.Max(second, 0);
		}

		public void SetCurrentTime(float totalGameWorldSeconds)
		{
			SetCurrentTime(0, 0, 0, totalGameWorldSeconds);
		}
		
		public void AdvanceTime(float elapsedRealWorldSeconds)
		{
			if (elapsedRealWorldSeconds < 0)
			{
				if (Debug.isDebugBuild)
					Debug.LogWarning($"Parameter {nameof(elapsedRealWorldSeconds)} has a value of {elapsedRealWorldSeconds}. Value should be >= 0. Unable to advance time.", _debugLogContext);

				return;
			}
			
			DeltaTime = ConvertToWorldTime(elapsedRealWorldSeconds);
			TotalSeconds += DeltaTime;
			Seconds += DeltaTime;
		}

		private float ConvertToWorldTime(float realWorldSeconds)
		{
			return realWorldSeconds * _toWorldTimeConversionFactor * TimeMultiplier;
		}
	}
}