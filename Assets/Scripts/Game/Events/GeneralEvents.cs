namespace Game.Events
{
	public class GeneralEvents
	{
		/// <summary>
		/// Raised when the game is paused
		/// </summary>
		public GameEvent GamePaused { get; } = new();
		
		/// <summary>
		/// Raised when the game is resuming from being paused
		/// </summary>
		public GameEvent GameResumed { get; } = new();
		
		/// <summary>
		/// Raised when the game finished saving
		/// </summary>
		public GameEvent GameSaved { get; } = new();

		/// <summary>
		/// Raised when the game is finished loading
		/// </summary>
		public GameEvent GameLoaded { get; } = new();
		
		
		
		/// <summary>
		/// Requests to pause the game
		/// </summary>
		public GameRequest PauseGame { get; } = new();
		
		/// <summary>
		/// Requests to resume the game from being paused
		/// </summary>
		public GameRequest ResumeGame { get; } = new();
		
		/// <summary>
		/// Requests to quit to main menu
		/// </summary>
		public GameRequest QuitGame { get; } = new();
		
		/// <summary>
		/// Requests to save the game
		/// </summary>
		public GameRequest SaveGame { get; } = new();
		
		/// <summary>
		/// Requests to close the entire program
		/// </summary>
		public GameRequest CloseGame { get; } = new();
	}
}