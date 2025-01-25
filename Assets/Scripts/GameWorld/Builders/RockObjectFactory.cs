using Extensions;
using GameWorld.WorldObjects;
using UnityEngine;

namespace GameWorld.Builders
{
	public class RockObjectFactory : WorldObjectFactory
	{
		public override string BuilderType => "Rock";

		public override WorldObject CreateObject(Vector2Int chunkPosition, Vector2Int localChunkPosition)
		{
			WorldObjectDetails details = GetRandomDetails();
			WorldObject rock = Instantiate(details.Prefab);
			rock.Init(details, new ChunkPlacementData(chunkPosition, localChunkPosition));
			
			rock.gameObject.name = details.Name;
			
			return rock;
		}

		// public bool TrySpawnSingle(Chunk chunk, Vector2Int localChunkPosition)
		// {
		// 	// Initial check to avoid creating unnecessary objects and destroying them
		// 	// BUG: This wouldn't work for objects bigger than 1x1
		// 	if (!chunk.IsValidTilePosition(localChunkPosition) || chunk.IsTileOccupied(localChunkPosition))
		// 		return false;
		// 	
		// 	// Convert to world position to be placed
		// 	Vector3 worldPosition = _world
		// 		.WorldPositionFromTilePosition(localChunkPosition, chunk.Position, centerOfTile: false)
		// 		.ToVector3(VectorSub.XSubY) + new Vector3(0.5f, 0, 0.5f);
		// 	
		// 	// Instantiate and set position
		// 	WorldObject rock = Object.Instantiate(GetRandomWorldObjectDetails(), _rockContainer).GetComponent<WorldObject>();
		// 	rock.transform.SetPositionAndRotation(worldPosition, Quaternion.Euler(0, Random.Range(0, 360), 0));
		//
		// 	// Get the details of the world object
		// 	if (!_world.DetailsMap.TryGetDetails(ROCK_DETAILS_NAME, out WorldObjectDetails worldObjectDetails))
		// 	{
		// 		Debug.LogWarning($"Unable to find {nameof(WorldObjectDetails)} for \"{ROCK_DETAILS_NAME}\"");
		// 		return false;
		// 	}
		// 	rock.Init(worldObjectDetails, new ChunkPlacementData(chunk.Position, localChunkPosition));
		// 	
		// 	// Add reference to the world object in the chunk and destroy the world object if unable to add to chunk
		// 	if (!chunk.TryAddWorldObject(rock))
		// 	{
		// 		rock.gameObject.Destroy();
		// 		return false;
		// 	}
		//
		// 	return true;
		// }
	}
}