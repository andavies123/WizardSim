using System.Linq;
using Extensions;
using GameWorld.WorldObjects;
using GeneralBehaviours.ShaderManagers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameWorld.Builders
{
	public class RockObjectBuilder : IWorldObjectBuilder
	{
		private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
		
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
			Vector3 worldPosition = _world.WorldPositionFromTilePosition(localChunkPosition, chunk.Position, centerOfTile: false).ToVector3(VectorSub.XSubY);
			
			// Instantiate and set position
			WorldObject rock = Object.Instantiate(_rockPrefab, _rockContainer).GetComponent<WorldObject>();
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

		public WorldObject SpawnPreview()
		{
			WorldObject rock = Object.Instantiate(_rockPrefab, _rockContainer).GetComponent<WorldObject>();
			
			rock.GetComponent<InteractionShaderManager>().enabled = false;
			rock.GetComponentsInChildren<Collider>(true).ToList().ForEach(x => x.enabled = false);
			rock.GetComponentsInChildren<MeshRenderer>(true).ToList().ForEach(renderer =>
			{
				Color color = renderer.material.GetColor(BaseColor);
				color.a = 0.25f;
				renderer.material.SetColor(BaseColor, color);
			});

			return rock;
		}
	}
}