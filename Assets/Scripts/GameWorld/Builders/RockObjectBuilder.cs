using Extensions;
using GameWorld.WorldObjects;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameWorld.Builders
{
	public class RockObjectBuilder : IWorldObjectBuilder
	{
		private readonly World _world;
		private readonly GameObject _rockPrefab;
		private readonly Transform _rockContainer;
		
		public RockObjectBuilder(World world, GameObject rockPrefab, Transform rockContainer)
		{
			_world = world;
			_rockPrefab = rockPrefab;

			_rockContainer = new GameObject("Rock Container").transform;
			_rockContainer.SetParent(rockContainer);
		}

		public bool TrySpawnSingle(Chunk chunk, Vector2Int localChunkPosition)
		{
			// Initial check to avoid creating unnecessary objects and destroying them
			// BUG: This wouldn't work for objects bigger than 1x1
			if (!chunk.IsValidTilePosition(localChunkPosition) || !chunk.IsWorldObjectSpaceEmpty(localChunkPosition))
				return false;
			
			// Convert to world position to be placed
			Vector3 worldPosition = _world
				.WorldPositionFromTilePosition(localChunkPosition, chunk.Position, centerOfTile: false)
				.ToVector3(VectorSub.XSubY);
			
			// Instantiate and set position
			WorldObject rock = Object.Instantiate(_rockPrefab, _rockContainer).GetComponent<WorldObject>();
			rock.transform.SetPositionAndRotation(worldPosition + rock.InitialPositionOffset, Quaternion.identity);
			rock.Init(new ChunkPlacementData(chunk.Position, localChunkPosition));
			
			// Add reference to the world object in the chunk and destroy the world object if unable to add to chunk
			if (!chunk.TryAddWorldObject(rock))
			{
				rock.gameObject.Destroy();
				return false;
			}

			return true;
		}
	}
}