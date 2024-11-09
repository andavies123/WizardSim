using GameWorld.WorldObjects;
using MessagingSystem;
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