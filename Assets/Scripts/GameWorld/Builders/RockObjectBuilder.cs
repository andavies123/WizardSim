using System.Collections.Generic;
using Extensions;
using GameWorld.WorldObjects;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameWorld.Builders
{
	public class RockObjectBuilder : IWorldObjectBuilder
	{
		private const string ROCK_DETAILS_NAME = "Rock";

		private readonly List<GameObject> _rockPrefabs;
		private readonly World _world;
		private readonly Transform _rockContainer;
		
		public RockObjectBuilder(World world, List<GameObject> rockPrefabs, Transform rockContainer)
		{
			_world = world;
			_rockPrefabs = rockPrefabs;

			_rockContainer = new GameObject("Rock Container").transform;
			_rockContainer.SetParent(rockContainer);
		}

		public bool TrySpawnSingle(Chunk chunk, Vector2Int localChunkPosition)
		{
			// Initial check to avoid creating unnecessary objects and destroying them
			// BUG: This wouldn't work for objects bigger than 1x1
			if (!chunk.IsValidTilePosition(localChunkPosition) || chunk.IsTileOccupied(localChunkPosition))
				return false;
			
			// Convert to world position to be placed
			Vector3 worldPosition = _world
				.WorldPositionFromTilePosition(localChunkPosition, chunk.Position, centerOfTile: false)
				.ToVector3(VectorSub.XSubY) + new Vector3(0.5f, 0, 0.5f);
			
			// Instantiate and set position
			WorldObject rock = Object.Instantiate(GetRandomRockPrefab(), _rockContainer).GetComponent<WorldObject>();
			rock.transform.SetPositionAndRotation(worldPosition, Quaternion.Euler(0, Random.Range(0, 360), 0));

			// Get the details of the world object
			if (!_world.DetailsMap.TryGetDetails(ROCK_DETAILS_NAME, out WorldObjectDetails worldObjectDetails))
			{
				Debug.LogWarning($"Unable to find {nameof(WorldObjectDetails)} for \"{ROCK_DETAILS_NAME}\"");
				return false;
			}
			rock.Init(worldObjectDetails, new ChunkPlacementData(chunk.Position, localChunkPosition));
			
			// Add reference to the world object in the chunk and destroy the world object if unable to add to chunk
			if (!chunk.TryAddWorldObject(rock))
			{
				rock.gameObject.Destroy();
				return false;
			}

			return true;
		}

		private GameObject GetRandomRockPrefab() => _rockPrefabs[Random.Range(0, _rockPrefabs.Count)];
	}
}