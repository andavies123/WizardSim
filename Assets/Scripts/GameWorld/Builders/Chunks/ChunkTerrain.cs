using System.Collections.Generic;
using UnityEngine;

namespace GameWorld.Builders.Chunks
{
	public class ChunkTerrain
	{
		/// <summary>
		/// The type of terrain for each "tile"
		/// </summary>
		public readonly int[,] TerrainType;
		
		/// <summary>
		/// Contains all rocks that are in this chunk
		/// (Position of the rock in the chunk, the type of rock)
		/// </summary>
		public readonly Dictionary<Vector2Int, int> Rocks = new();

		public ChunkTerrain(Vector2Int chunkSize)
		{
			TerrainType = new int[chunkSize.x, chunkSize.y];
		}
	}
}