using System;
using Extensions;
using Game.MessengerSystem;
using GameWorld.Messages;
using GameWorld.Spawners;
using GameWorld.Tiles;
using GameWorld.WorldObjectPreviews;
using GameWorld.WorldObjects;
using UnityEngine;
using Utilities;
using Wizards;

namespace GameWorld.Builders
{
	public class WorldBuilder : MonoBehaviour
	{
		[SerializeField] private World world;
		[SerializeField] private Transform worldObjectParent;
		
		[Header("Spawners")]
		[SerializeField] private WizardSpawner wizardSpawner;
		[SerializeField] private EntitySpawner enemySpawner;

		[Header("Settings")]
		[SerializeField] private int initialGenerationRadius = 5;

		[Header("Rock Generation Settings")]
		[SerializeField] private GameObject rockPrefab;
		[SerializeField] private int rocksPerChunk;

		private WorldObjectPreviewManager _worldObjectPreviewManager;

		public RockObjectBuilder RockObjectBuilder { get; private set; }

		private void Awake()
		{
			RockObjectBuilder = new RockObjectBuilder(world, rockPrefab, worldObjectParent);
			_worldObjectPreviewManager = new WorldObjectPreviewManager(world, transform);
			
			GlobalMessenger.Subscribe<WorldObjectPlacementRequest>(OnPlaceWorldObjectRequested);
			GlobalMessenger.Subscribe<WizardSpawnRequestMessage>(OnWizardSpawnRequested);
			GlobalMessenger.Subscribe<EnemySpawnRequestMessage>(OnEnemySpawnRequested);
			
			_worldObjectPreviewManager.SubscribeToMessages();
		}

		private void Start()
		{
			GenerateWorld();
		}

		private void OnDestroy()
		{
			GlobalMessenger.Unsubscribe<WorldObjectPlacementRequest>(OnPlaceWorldObjectRequested);
			GlobalMessenger.Unsubscribe<WizardSpawnRequestMessage>(OnWizardSpawnRequested);
			GlobalMessenger.Unsubscribe<EnemySpawnRequestMessage>(OnEnemySpawnRequested);
			
			_worldObjectPreviewManager.UnsubscribeFromMessages();
		}

		private void GenerateWorld()
		{
			if (!world)
				throw new NullReferenceException($"Unable to Generate World. {nameof(world)} object is null");
			if (!world.WorldDetails)
				throw new NullReferenceException($"Unable to Generate World. {nameof(world.WorldDetails)} object is null");
			
			Transform worldTransform = world.transform; // Added so world.transform isn't called for each loop iteration
			
			for (int x = -initialGenerationRadius + 1; x < initialGenerationRadius; x++)
			{
				for (int z = -initialGenerationRadius + 1; z < initialGenerationRadius; z++)
				{
					Vector2Int chunkPosition = new(x, z);
					Vector3 spawnPosition = new(x, 0, z);
					Vector3 chunkSize = new(world.WorldDetails.ChunkSize.x, 0, world.WorldDetails.ChunkSize.y);
					spawnPosition.Scale(chunkSize);
					
					Chunk chunk = Instantiate(world.WorldDetails.ChunkPrefab, worldTransform).GetComponent<Chunk>();

					Transform chunkTransform = chunk.transform;
					chunkTransform.localPosition = spawnPosition;
					chunk.Initialize(world, chunkPosition, GenerateTilesForChunk(chunk, chunkTransform));
					GenerateRocks(chunk);
					
					world.AddChunk(chunk);
				}
			}
		}

		private Tile[,] GenerateTilesForChunk(Chunk parentChunk, Transform parent)
		{
			Tile[,] tiles = new Tile[world.WorldDetails.ChunkTiles.x, world.WorldDetails.ChunkTiles.y];

			for (int x = 0; x < tiles.GetLength(0); x++)
			{
				for (int z = 0; z < tiles.GetLength(1); z++)
				{
					Vector2Int tilePosition = new(x, z);
					Vector3 spawnPosition = new(x * world.WorldDetails.TileSize.x, 0, z * world.WorldDetails.TileSize.y);
					Tile tile = Instantiate(world.WorldDetails.TilePrefab, parent).GetComponent<Tile>();

					Transform tileTransform = tile.transform;
					tileTransform.localPosition = spawnPosition;
					tile.Initialize(world, parentChunk, tilePosition);
					
					tiles[x, z] = tile;
				}
			}

			return tiles;
		}

		private void GenerateRocks(Chunk chunk)
		{
			int addedRocks = 0;

			while (addedRocks < rocksPerChunk)
			{
				Vector2Int localChunkPosition = RandomExt.RangeVector2Int(
					0, world.WorldDetails.ChunkTiles.x,
					0, world.WorldDetails.ChunkTiles.y);

				if (RockObjectBuilder.TrySpawnSingle(chunk, localChunkPosition))
					addedRocks++;
			}
		}

		private void OnWizardSpawnRequested(WizardSpawnRequestMessage message) => wizardSpawner.SpawnWizard(message.SpawnPosition, message.WizardType);
		private void OnEnemySpawnRequested(EnemySpawnRequestMessage message) => enemySpawner.SpawnEntity(message.SpawnPosition);

		private void OnPlaceWorldObjectRequested(WorldObjectPlacementRequest message)
		{
			if (message == null)
			{
				Debug.Log($"Received invalid {nameof(WorldObjectPlacementRequest)} message");
				return;
			}
			
			if (!TrySpawnSingle(message.WorldObjectDetails, message.ChunkPosition, message.TilePosition, worldObjectParent))
			{
				Debug.LogWarning($"{message.WorldObjectDetails.Name} was not able to be spawned.", this);
			}
		}

		private bool TrySpawnSingle(WorldObjectDetails details, Vector2Int chunkPosition, Vector2Int localChunkPosition, Transform container)
		{
			// Initial checks to avoid creating unnecessary objects and destroying them
			if (world.WorldObjectManager.IsAtMaxCapacity(details))
			{
				Debug.LogWarning($"Unable to place {details.Name}. There are already {world.WorldObjectManager.GetObjectCount(details)} of {details.PlacementProperties.MaxObjectsAllowed} objects already placed.");
				return false;
			}

			if (!world.Chunks.TryGetValue(chunkPosition, out Chunk chunk))
			{
				Debug.LogWarning($"Unable to place {details.Name}. The chunk at {chunkPosition} does not currently exist.");
				return false;
			}
			
			// Todo: Need to check all positions for objects that are larger than 1x1x1
			if (!chunk.IsValidTilePosition(localChunkPosition))
			{
				Debug.LogWarning($"Unable to place {details.Name}. {localChunkPosition} is not a valid tile position.");
                return false;
			}

			// Todo: Need to check all positions for objects that are larger than 1x1x1
			if (chunk.IsTileOccupied(localChunkPosition))
			{
				Debug.LogWarning($"Unable to place {details.Name}. {localChunkPosition} is already occupied by another world object");
				return false;
			}

			// By this point we should have all checks to make sure we can place a valid object
			// Convert to world position to be placed
			Vector3 worldPosition = world
				.WorldPositionFromTilePosition(localChunkPosition, chunk.Position, centerOfTile: false)
				.ToVector3(VectorSub.XSubY);
			
			// Instantiate and set position
			WorldObject worldObject = Instantiate(details.Prefab, container);
			worldObject.transform.SetPositionAndRotation(worldPosition, Quaternion.identity);

			// Get the details of the world object
			worldObject.Init(details, new ChunkPlacementData(chunk.Position, localChunkPosition));
			
			// Add reference to the world object in the chunk and destroy the world object if unable to add to chunk
			if (!chunk.TryAddWorldObject(worldObject))
			{
				worldObject.gameObject.Destroy();
				Debug.LogWarning($"Unable to place {details.Name}. Object was unable to be added to the chunk.");
				return false;
			}
			
			world.WorldObjectManager.AddWorldObject(worldObject);

			return true;
		}
	}
}