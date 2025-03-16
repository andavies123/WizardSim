using UnityEngine;

namespace AndysTools.GameWorldTimeManagement.Runtime
{
	/// <summary>
	/// Unity MonoBehaviour wrapper class for the <see cref="GameWorldTime"/> class
	/// to handle time management for the game world
	///
	/// If a MonoBehaviour is not wanted, the <see cref="GameWorldTime"/> logic was created
	/// outside a MonoBehaviour class if preferred. This class could be used as an example of
	/// how to add it into an existing MonoBehaviour.
	///
	/// An interface has also been created/attached to <see cref="GameWorldTime"/>
	/// for easier substituting for Unit Tests
	/// </summary>
	public class GameWorldTimeBehaviour : MonoBehaviour, IGameWorldTime
	{
		[Tooltip("How many real world seconds it takes for one in game day to occur. Ex: A value of 60 would take 60 real world seconds for one in-game day")]
		[SerializeField] private float realLifeSecondsPerGameWorldDay = 600;
		
		private IGameWorldTime _gameWorldTime;
		
		public float DeltaTime => _gameWorldTime.DeltaTime;
		public float TotalSeconds => _gameWorldTime.TotalSeconds;
		
		public int Days => _gameWorldTime.Days;
		public int Hours => _gameWorldTime.Hours;
		public int Minutes => _gameWorldTime.Minutes;
		public float Seconds => _gameWorldTime.Seconds;

		public float TimeMultiplier
		{
			get => _gameWorldTime.TimeMultiplier;
			set => _gameWorldTime.TimeMultiplier = value;
		}

		public void SetCurrentTime(int day, int hour, int minute, float second)
		{
			_gameWorldTime.SetCurrentTime(day, hour, minute, second);
		}

		public void SetCurrentTime(float totalGameWorldSeconds)
		{
			_gameWorldTime.SetCurrentTime(totalGameWorldSeconds);
		}

		public void AdvanceTime(float elapsedRealWorldSeconds)
		{
			_gameWorldTime.AdvanceTime(elapsedRealWorldSeconds);
		}

		protected virtual void Awake()
		{
			// Since we are using a serialized field for the conversion, we need to create the object here in awake
			_gameWorldTime = new GameWorldTime(realLifeSecondsPerGameWorldDay, this);
		}

		protected virtual void Update()
		{
			// Since _gameWorldTime is not a MonoBehaviour, we need to update the time here in the update method
			if (Time.timeScale == 0)
				_gameWorldTime.AdvanceTime(0);
			else
				_gameWorldTime.AdvanceTime(Time.unscaledDeltaTime);
		}
	}
}