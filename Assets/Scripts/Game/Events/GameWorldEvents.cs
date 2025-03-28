using GameWorld.WorldObjects;
using UnityEngine;

namespace Game.Events
{
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
}