using Game.MessengerSystem;
using GameWorld.WorldObjects;
using UnityEngine;

namespace GameWorld.Messages
{
	public class WorldObjectPlacementRequest : Message
	{
		public WorldObjectPlacementRequest(object sender, Vector2Int chunkPosition, Vector2Int tilePosition, WorldObjectDetails worldObjectDetails)
			: base(sender)
		{
			ChunkPosition = chunkPosition;
			TilePosition = tilePosition;
			WorldObjectDetails = worldObjectDetails;
		}
		
		public Vector2Int ChunkPosition { get; }
		public Vector2Int TilePosition { get; }
		public WorldObjectDetails WorldObjectDetails { get; }
	}
}