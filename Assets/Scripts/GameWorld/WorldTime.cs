using UnityEngine;
using Utilities;

namespace GameWorld
{
	[DisallowMultipleComponent]
	public class WorldTime : MonoBehaviour
	{
		[SerializeField] private float secondsPerWorldDay = 600f;
		
		private float _toWorldTimeConversionFactor;
		private float _currentSeconds;
		private int _currentMinutes;
		private int _currentHours;
		
		public float WorldDeltaTime { get; private set; }

		public float CurrentSeconds
		{
			get => _currentSeconds;
			set
			{
				_currentSeconds = value;
				if (_currentSeconds >= TimeConstants.SECONDS_PER_MINUTE)
				{
					CurrentMinutes += (int)(_currentSeconds / TimeConstants.SECONDS_PER_MINUTE);
					_currentSeconds %= TimeConstants.SECONDS_PER_MINUTE;
				}
			}
		}

		public int CurrentMinutes
		{
			get => _currentMinutes;
			set
			{
				_currentMinutes = value;
				if (_currentMinutes >= TimeConstants.MINUTES_PER_HOUR)
				{
					CurrentHours += _currentMinutes / TimeConstants.MINUTES_PER_HOUR;
					_currentMinutes %= TimeConstants.MINUTES_PER_HOUR;
				}
			}
		}

		public int CurrentHours
		{
			get => _currentHours;
			set
			{
				_currentHours = value;
				if (_currentHours >= TimeConstants.HOURS_PER_DAY)
				{
					CurrentDays += _currentHours / TimeConstants.HOURS_PER_DAY;
					_currentHours %= TimeConstants.HOURS_PER_DAY;
				}
			}
		}

		public int CurrentDays { get; set; }

		private void Awake()
		{
			_toWorldTimeConversionFactor = TimeConstants.SECONDS_PER_DAY / secondsPerWorldDay;
		}

		private void Update()
		{
			WorldDeltaTime = ConvertToWorldTime(Time.deltaTime);
			CurrentSeconds += WorldDeltaTime;
		}

		private float ConvertToWorldTime(float realWorldSeconds)
		{
			return realWorldSeconds * _toWorldTimeConversionFactor;
		}
	}
}