namespace AndysTools.GameWorldTimeManagement.Runtime
{
	public interface IGameWorldTime
	{
		/// <summary>
		/// Game World delta time.
		/// Works just like Time.deltaTime except it is converted into game world time
		/// </summary>
		float DeltaTime { get; }
		
		/// <summary>
		/// The total number of seconds that has been passed since the game time started
		/// </summary>
		public float TotalSeconds { get; }
		
		/// <summary>
		/// The current game world day count.
		/// This value will continue to increase and does not go down
		/// </summary>
		int Days { get; }
		
		/// <summary>
		/// The current game world hour.
		/// Returns a value from 0 -> 23
		/// Once this value reaches 24, it will increase the <see cref="Days"/> value
		/// and reset back to zero
		/// </summary>
		int Hours { get; }
		
		/// <summary>
		/// The current game world minute.
		/// Returns a value from 0 -> 59
		/// Once this value reaches 60, it will increase the <see cref="Hours"/> value
		/// and reset back to zero
		/// </summary>
		int Minutes { get; }
		
		/// <summary>
		/// The current game world seconds.
		/// Returns a value from 0 -> 59.9 repeating
		/// Once this value reaches 60, it will increase the <see cref="Minutes"/> value
		/// and reset back to zero
		/// </summary>
		float Seconds { get; }
		
		/// <summary>
		/// A multiplier that gets applied to the elapsed time.
		/// A value of 2.0f would achieve double speed.
		/// A value of 0.5f would achieve half-speed.
		/// A value of 1.0f would be the default speed.
		/// </summary>
		float TimeMultiplier { get; set; }

		/// <summary>
		/// Sets the current game world time using the given values.
		/// Overrides all time values.
		/// </summary>
		/// <remarks>
		/// Loading a game world that has a saved time value.
		/// The <see cref="Days"/>, <see cref="Hours"/>, <see cref="Minutes"/>, <see cref="Seconds"/>
		/// values can be saved and used here to load the time into this object
		/// </remarks>
		/// <param name="day">The day value to override <see cref="Days"/></param>
		/// <param name="hour">The hour value to override <see cref="Hours"/></param>
		/// <param name="minute">The minutes value to override <see cref="Minutes"/></param>
		/// <param name="second">The seconds value to override <see cref="Seconds"/></param>
		void SetCurrentTime(int day, int hour, int minute, float second);

		/// <summary>
		/// Sets the current game world time using the given value.
		/// Overrides all time values.
		/// </summary>
		/// <remarks>
		/// Loading a game world that has a saved time value.
		/// The <see cref="TotalSeconds"/> value can be saved and
		/// used here to load the time into this object
		/// </remarks>
		/// <param name="totalGameWorldSeconds"></param>
		void SetCurrentTime(float totalGameWorldSeconds);
		
		/// <summary>
		/// Advances the game world time
		/// Typical usage would be to pass Time.deltaTime
		/// </summary>
		/// <param name="elapsedRealWorldSeconds">The number of real world seconds to increment by</param>
		void AdvanceTime(float elapsedRealWorldSeconds);
	}
}