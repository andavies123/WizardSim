using GameWorld.WorldObjects;
using UnityEngine;

namespace GameWorld.Builders
{
	public interface IWorldObjectBuilder
	{
		public bool TrySpawnSingle(Chunk chunk, Vector2Int localChunkPosition);
		public WorldObject SpawnPreview();
	}
}