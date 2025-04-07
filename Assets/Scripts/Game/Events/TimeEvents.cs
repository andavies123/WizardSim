using Game.Common;

namespace Game.Events
{
	public class TimeEvents
	{
		/// <summary>
		/// Raised when a new game day has started.
		/// Basically midnight 
		/// </summary>
		public GameEvent NewGameDayStarted { get; } = new();

		/// <summary>
		/// Raised when the day is finishing transitioning from night to day
		/// </summary>
		public GameEvent DaytimeStarted { get; } = new();

		/// <summary>
		/// Raised when the day is finishing transitioning from day to night
		/// </summary>
		public GameEvent NighttimeStarted { get; } = new();
		
		/// <summary>
		/// Requests to update the game time speed
		/// </summary>
		public GameRequest<GameSpeedEventArgs> ChangeGameSpeed { get; } = new();
	}

	public class GameSpeedEventArgs : GameEventArgs
	{
		public GameSpeedEventArgs(GameSpeed gameSpeed) => GameSpeed = gameSpeed;
		
		public GameSpeed GameSpeed { get; }
	}
}