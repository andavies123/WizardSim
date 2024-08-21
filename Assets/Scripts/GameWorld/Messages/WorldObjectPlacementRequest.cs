using Game.MessengerSystem;
using UnityEngine;

namespace GameWorld.Messages
{
	public class WorldObjectPlacementRequest : Message
	{
		public WorldObjectPlacementRequest(object sender, Vector2Int chunkPosition, Vector2Int tilePosition, GameObject worldObjectPrefab)
			: base(sender)
		{
			ChunkPosition = chunkPosition;
			TilePosition = tilePosition;
			WorldObjectPrefab = worldObjectPrefab;
		}
		
		public Vector2Int ChunkPosition { get; }
		public Vector2Int TilePosition { get; }
		public GameObject WorldObjectPrefab { get; }
	}
}