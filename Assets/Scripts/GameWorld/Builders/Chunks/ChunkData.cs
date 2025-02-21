using System;
using UnityEngine;

namespace GameWorld.Builders.Chunks
{
	public class ChunkData
	{
		public ChunkData(Vector2Int position, ChunkTerrain terrain)
		{
			Position = position;
			Terrain = terrain ?? throw new ArgumentNullException(nameof(terrain));
		}
		
		public Vector2Int Position { get; private set; }
		public ChunkTerrain Terrain { get; private set; }
	}
}