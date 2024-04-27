using UnityEngine;

namespace GameWorld.Builders
{
	public interface IWorldBuilder
	{
		public bool TrySpawnSingle(Chunk chunk, Vector2Int localChunkPosition);
	}
}