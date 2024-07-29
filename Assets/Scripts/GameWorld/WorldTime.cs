using UnityEngine;
using Utilities;

namespace GameWorld
{
	[DisallowMultipleComponent]
	public class WorldTime : MonoBehaviour
	{
		[SerializeField] private float secondsPerWorldDay = 600f;
		
		private float _toWorldTimeConversionFactor;
		
		/// <summary>
		/// Delta Time converted into world time.
		/// This value is calculated once per frame to avoid extra calculations
		/// every time it is requested
		/// </summary>
		public static float DeltaTime { get; private set; }

		private void Awake()
		{
			_toWorldTimeConversionFactor = TimeConstants.SECONDS_PER_DAY / secondsPerWorldDay;
		}

		private void Update()
		{
			DeltaTime = ConvertToWorldTime(Time.deltaTime);
		}

		private float ConvertToWorldTime(float realWorldSeconds)
		{
			return realWorldSeconds * _toWorldTimeConversionFactor;
		}
	}
}