using System;
using Extensions;
using Game.Messages;
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

		private WorldObject _previewWorldObject;

		public RockObjectBuilder RockObjectBuilder { get; private set; }

		private void Awake()
		{
			RockObjectBuilder = new RockObjectBuilder(world, rockPrefab);
			GenerateWorld();
			
			GlobalMessenger.Subscribe<WizardSpawnRequestMessage>(OnWizardSpawnRequested);
			GlobalMessenger.Subscribe<EnemySpawnRequestMessage>(OnEnemySpawnRequested);
			GlobalMessenger.Subscribe<WorldObjectPreviewRequest>(OnWorldObjectPreviewRequested);
			GlobalMessenger.Subscribe<WorldObjectPlacementRequest>(OnWorldObjectPlacementRequested);
			GlobalMessenger.Subscribe<WorldObjectHidePreviewRequest>(OnWorldObjectHidePreviewRequested);
			GlobalMessenger.Subscribe<PlacementModeEnded>(OnPlacementModeEnded);
		}

		private void OnDestroy()
		{
			GlobalMessenger.Unsubscribe<WizardSpawnRequestMessage>(OnWizardSpawnRequested);
			GlobalMessenger.Unsubscribe<EnemySpawnRequestMessage>(OnEnemySpawnRequested);
			GlobalMessenger.Unsubscribe<WorldObjectPreviewRequest>(OnWorldObjectPreviewRequested);
			GlobalMessenger.Unsubscribe<WorldObjectPlacementRequest>(OnWorldObjectPlacementRequested);
			GlobalMessenger.Unsubscribe<WorldObjectHidePreviewRequest>(OnWorldObjectHidePreviewRequested);
			GlobalMessenger.Unsubscribe<PlacementModeEnded>(OnPlacementModeEnded);
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
		
		private void OnWorldObjectPreviewRequested(WorldObjectPreviewRequest message)
		{
			if (message.WorldObjectPrefab == rockPrefab)
			{
				if (_previewWorldObject == null)
					_previewWorldObject = RockObjectBuilder.SpawnPreview();
				
				Vector3 worldPosition = world.WorldPositionFromTilePosition(message.TilePosition, message.ChunkPosition, centerOfTile: false).ToVector3(VectorSub.XSubY);
				_previewWorldObject.transform.SetPositionAndRotation(worldPosition, Quaternion.identity);
			}

			// Make sure the game object is active as it might have been disabled due to the hide request
			_previewWorldObject.gameObject.SetActive(true);
		}
        
		private void OnWorldObjectPlacementRequested(WorldObjectPlacementRequest message)
		{
			if (!world.Chunks.TryGetValue(message.ChunkPosition, out Chunk chunk))
				return;
			
			if (message.WorldObjectPrefab == rockPrefab)
				RockObjectBuilder.TrySpawnSingle(chunk, message.TilePosition);
		}

		private void OnWorldObjectHidePreviewRequested(WorldObjectHidePreviewRequest message)
		{
			if (_previewWorldObject)
				_previewWorldObject.gameObject.SetActive(false);
		}

		private void OnPlacementModeEnded(PlacementModeEnded message) =>
			_previewWorldObject.gameObject.Destroy();
	}
}