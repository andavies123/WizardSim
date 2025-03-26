using GameWorld.Builders.Chunks;
using GameWorld.Tiles;
using UnityEngine;

namespace GameWorld.GameWorldEventArgs
{
	public class WorldPositionEventArgs
	{
		public WorldPositionEventArgs(Vector2Int chunkPosition, Vector2Int tilePosition)
		{
			ChunkPosition = chunkPosition;
			TilePosition = tilePosition;
		}

		public WorldPositionEventArgs(Tile tile)
		{
			ChunkPosition = tile.ParentChunk.ChunkData.Position;
			TilePosition = tile.TilePosition;
		}

		public WorldPositionEventArgs(Chunk chunk)
		{
			ChunkPosition = chunk.ChunkData.Position;
			TilePosition = Vector2Int.zero;
		}
		
		public Vector2Int ChunkPosition { get; }
		public Vector2Int TilePosition { get; }
	}
}