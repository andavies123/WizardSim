namespace Game.Events
{
	public class GeneralEvents
	{
		public GameEvent GamePaused { get; } = new();
		public GameEvent GameResumed { get; } = new();
		public GameEvent GameQuit { get; } = new();
		public GameEvent GameSaved { get; } = new();
		
		public GameRequest PauseGame { get; } = new();
		public GameRequest ResumeGame { get; } = new();
		public GameRequest QuitGame { get; } = new();
		public GameRequest SaveGame { get; } = new();
	}
}