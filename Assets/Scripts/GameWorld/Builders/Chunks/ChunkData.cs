using System;
using UnityEngine;

namespace GameWorld.Builders.Chunks
{
	public class ChunkData
	{
		public ChunkData(Vector2Int position, Vector2Int size, ChunkTerrain terrain)
		{
			Position = position;
			Size = size;
			Terrain = terrain ?? throw new ArgumentNullException(nameof(terrain));
		}
		
		public Vector2Int Position { get; private set; }
		public Vector2Int Size { get; private set; }
		public ChunkTerrain Terrain { get; private set; }
	}
}