using System.Collections.Generic;
using UnityEngine;

namespace GameWorld
{
	public class World : MonoBehaviour
	{
		private readonly Dictionary<Vector2Int, Chunk> _chunks = new();

		public void AddChunk(Chunk chunk)
		{
			_chunks[chunk.Position] = chunk;
		}
	}
}