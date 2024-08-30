using Game.MessengerSystem;
using GameWorld.WorldObjects;
using UnityEngine;

namespace GameWorld.Messages
{
	public class WorldObjectPlacementRequest : Message
	{
		public Vector2Int ChunkPosition { get; set; }
		public Vector2Int TilePosition { get; set; }
		public WorldObjectDetails WorldObjectDetails { get; set; }
	}
}