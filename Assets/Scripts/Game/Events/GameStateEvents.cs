using GameWorld.WorldObjects;

namespace Game.Events
{
	public class GameStateEvents
	{
		public GameRequest<BeginPlacementModeEventArgs> BeginPlacementMode { get; }
		public GameRequest EndPlacementMode { get; }
	}

	public class BeginPlacementModeEventArgs : GameEventArgs
	{
		public WorldObjectDetails WorldObjectDetails { get; set; }
	}
}