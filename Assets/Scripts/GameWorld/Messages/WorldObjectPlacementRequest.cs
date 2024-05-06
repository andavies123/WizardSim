using Game.MessengerSystem;
using UnityEngine;

namespace GameWorld.Messages
{
	public class WorldObjectPlacementRequest : IMessage
	{
		public WorldObjectPlacementRequest(Vector2Int chunkPosition, Vector2Int tilePosition, GameObject worldObjectPrefab)
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