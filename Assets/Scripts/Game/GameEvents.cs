using GameWorld.WorldObjects;
using UnityEngine;

namespace Game
{
	public static class GameEvents
	{
		public static GeneralEvents General { get; private set; } = new();
		public static GameWorldEvents GameWorld { get; private set; } = new();
		public static SettlementEvents Settlement { get; private set; } = new();

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void Reset()
		{
			General = new GeneralEvents();
			GameWorld = new GameWorldEvents();
			Settlement = new SettlementEvents();
		}
	}

	public class GeneralEvents
	{
		public GameEvent GamePaused { get; } = new();
		public GameRequest PauseGame { get; } = new();
		
		public GameEvent GameResumed { get; } = new();
		public GameRequest ResumeGame { get; } = new();
		
		public GameEvent GameQuit { get; } = new();
		public GameRequest QuitGame { get; } = new();
		
		public GameEvent GameSaved { get; } = new();
		public GameRequest SaveGame { get; } = new();
	}

	public class GameWorldEvents
	{
		// World Objects
		public GameRequest<PlacementEventArgs> PlaceWorldObject { get; } = new();
		
		// World Object Previews
		public GameRequest<PlacementEventArgs> PlacePreviewWorldObject { get; } = new();
		public GameRequest DeletePreviewWorldObject { get; } = new();

		public class PlacementEventArgs : GameEventArgs
		{
			public Vector2Int ChunkPosition { get; set; }
			public Vector2Int TilePosition { get; set; }
			public WorldObjectDetails WorldObjectDetails { get; set; }
			public bool Visibility { get; set; }
		}
	}

	public class SettlementEvents
	{
		public GameEvent<GameEventArgs> WizardAdded { get; } = new();
		public GameEvent<GameEventArgs> WizardDied { get; } = new();
		public GameEvent<GameEventArgs> BuildingAdded { get; } = new();
		public GameEvent<GameEventArgs> BuildingDestroyed { get; } = new();
	}
}