using UnityEngine;

namespace GameWorld.WorldObjects
{
	public struct ChunkPlacementData
	{
		public Vector2Int ChunkPosition { get; }
		public Vector2Int TilePosition { get; }
        
		public ChunkPlacementData(Vector2Int chunkPosition, Vector2Int tilePosition)
		{
			ChunkPosition = chunkPosition;
			TilePosition = tilePosition;
		}
	}
}