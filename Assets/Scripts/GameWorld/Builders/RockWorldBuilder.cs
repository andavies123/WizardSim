using Extensions;
using GameWorld.WorldObjects;
using UnityEngine;

namespace GameWorld.Builders
{
	public class RockWorldBuilder : IWorldBuilder
	{
		private readonly World _world;
		private readonly GameObject _rockPrefab;
		
		public RockWorldBuilder(World world, GameObject rockPrefab)
		{
			_world = world;
			_rockPrefab = rockPrefab;
		}

		public bool TrySpawnSingle(Chunk chunk, Vector2Int localChunkPosition)
		{
			// Initial check to avoid creating unnecessary objects and destroying them
			// BUG: This wouldn't work for objects bigger than 1x1
			if (!chunk.IsValidTilePosition(localChunkPosition) || !chunk.IsWorldObjectSpaceEmpty(localChunkPosition))
				return false;
			
			// Convert to world position to be placed
			Vector3 worldPosition = _world.WorldPositionFromTilePosition(localChunkPosition, chunk.Position).ToVector3(VectorSub.XSubY);
			
			// Instantiate and set position
			WorldObject rock = Object.Instantiate(_rockPrefab, _world.WorldObjectContainer).GetComponent<WorldObject>();
			rock.transform.SetPositionAndRotation(worldPosition, Quaternion.identity);
			rock.Init(localChunkPosition);
			
			// Add reference to world object in the chunk and destroy world object if unable to add to chunk
			if (!chunk.TryAddWorldObject(rock))
			{
				rock.gameObject.Destroy();
				return false;
			}

			return true;
		}
	}
}