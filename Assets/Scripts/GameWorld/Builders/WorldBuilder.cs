using System;
using Extensions;
using Game.MessengerSystem;
using GameWorld.Messages;
using GameWorld.Spawners;
using GameWorld.Tiles;
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
			GenerateWorld();
			_worldObjectPreviewManager = new WorldObjectPreviewManager(world, transform);
			
			GlobalMessenger.Subscribe<WizardSpawnRequestMessage>(OnWizardSpawnRequested);
			GlobalMessenger.Subscribe<EnemySpawnRequestMessage>(OnEnemySpawnRequested);
			_worldObjectPreviewManager.SubscribeToMessages();
			_worldObjectPreviewManager.CreateWorldObjectRequested += OnCreateWorldObjectRequested;
		}

		private void OnDestroy()
		{
			GlobalMessenger.Unsubscribe<WizardSpawnRequestMessage>(OnWizardSpawnRequested);
			GlobalMessenger.Unsubscribe<EnemySpawnRequestMessage>(OnEnemySpawnRequested);
			_worldObjectPreviewManager.UnsubscribeFromMessages();
			_worldObjectPreviewManager.CreateWorldObjectRequested -= OnCreateWorldObjectRequested;
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
					chunk.Initialize(world.WorldDetails.ChunkTiles, chunkPosition, GenerateTilesForChunk(chunk, chunkTransform));
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

		private void OnCreateWorldObjectRequested(object sender, CreateWorldObjectRequestEventArgs args)
		{
			if (!TrySpawnSingle(args.WorldObjectPrefab, args.ChunkPosition, args.TilePosition, worldObjectParent))
			{
				Debug.LogWarning("Unable to create world object");
			}
		}

		private bool TrySpawnSingle(GameObject prefab, Vector2Int chunkPosition, Vector2Int localChunkPosition, Transform container)
		{
			if (!world.Chunks.TryGetValue(chunkPosition, out Chunk chunk))
				return false;

			// Initial check to avoid creating unnecessary objects and destroying them
			// BUG: This wouldn't work for objects bigger than 1x1
			if (!chunk.IsValidTilePosition(localChunkPosition) || !chunk.IsWorldObjectSpaceEmpty(localChunkPosition))
				return false;
			
			// Convert to world position to be placed
			Vector3 worldPosition = world
				.WorldPositionFromTilePosition(localChunkPosition, chunk.Position, centerOfTile: false)
				.ToVector3(VectorSub.XSubY);
			
			// Instantiate and set position
			WorldObject worldObject = Instantiate(prefab, container).GetComponent<WorldObject>();
			worldObject.transform.SetPositionAndRotation(worldPosition + worldObject.InitialPositionOffset, Quaternion.identity);
			worldObject.Init(new ChunkPlacementData(chunk.Position, localChunkPosition));
			
			// Add reference to the world object in the chunk and destroy the world object if unable to add to chunk
			if (!chunk.TryAddWorldObject(worldObject))
			{
				worldObject.gameObject.Destroy();
				return false;
			}

			return true;
		}
	}
}