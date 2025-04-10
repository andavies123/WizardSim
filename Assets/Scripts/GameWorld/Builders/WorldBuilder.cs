﻿using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Events;
using GameWorld.Builders.Chunks;
using GameWorld.Characters.Wizards.Managers;
using GameWorld.Characters.Wizards.Tasks;
using GameWorld.WorldObjectPreviews;
using GameWorld.WorldObjects;
using UnityEngine;
using GameWorld.WorldObjects.Rocks;
using GeneralBehaviours.Utilities.ContextMenuBuilders;
using Utilities.Attributes;

namespace GameWorld.Builders
{
	public class WorldBuilder : MonoBehaviour
	{
		[SerializeField, Required] private World world;
		[SerializeField, Required] private ChunkBuilder chunkBuilder;
		[SerializeField, Required] private Transform worldObjectParent;
		[SerializeField, Required] private Transform playerCamera;

		[Header("Settings")]
		[SerializeField, Required] private int worldSeed = 46873654;
		[SerializeField, Required] private int initialGenerationRadius = 5;

		[Header("Rock Generation Settings")]
		[SerializeField, Required] private List<GameObject> rockPrefabs;
		[SerializeField, Required] private int rocksPerChunk;
		[SerializeField, Required] private WizardTaskManager wizardTaskManager;

		private readonly ConcurrentDictionary<Vector2Int, Chunk> _loadedChunks = new();
		private readonly Dictionary<string, IWorldObjectFactory> _worldObjectFactories = new();
		private readonly PlayerCameraChunkManager _playerCameraChunkManager = new();
		
		private Transform _activeChunkContainer;
		private WorldObjectPreviewManager _worldObjectPreviewManager;
		
		private void Awake()
		{
			_activeChunkContainer = new GameObject("Active Chunks").transform;
			
			_playerCameraChunkManager.ChunkBelowChanged += OnChunkBelowCameraChanged;
			
			_worldObjectPreviewManager = new WorldObjectPreviewManager(world, transform);

			// Build the world object builder dictionary
			IWorldObjectFactory[] worldObjectBuilders = GetComponents<IWorldObjectFactory>();
			foreach (IWorldObjectFactory builder in worldObjectBuilders)
			{
				if (!_worldObjectFactories.TryAdd(builder.BuilderType, builder))
				{
					Debug.LogWarning($"Issue caching {nameof(IWorldObjectFactory)}: {builder.GetType().Name}");
				}
			}

			// Todo: Do I need to add a listener for requesting to place a world object?
			InjectContextMenuActions();
		}
		
		private void Start()
		{
			_worldObjectPreviewManager.SubscribeToMessages();
			GenerateWorld();
		}

		private void Update()
		{
			Vector3 cameraPosition = playerCamera.position;
			_playerCameraChunkManager.ChunkBelow =
				world.ChunkPositionFromWorldPosition(new Vector2(cameraPosition.x, cameraPosition.z));
			// Todo: Update the current rotation quadrant of the camera
		}

		private void OnDestroy()
		{
			_worldObjectPreviewManager.UnsubscribeFromMessages();	
			_playerCameraChunkManager.ChunkBelowChanged -= OnChunkBelowCameraChanged;
		}

		private IEnumerator LoadChunks(Vector2Int from, Vector2Int to)
		{
			for (int x = Math.Min(from.x, to.x); x < Math.Max(from.x, to.x); x++)
			{
				for (int y = Math.Min(from.y, to.y); y < Math.Max(from.y, to.y); y++)
				{
					Vector2Int chunkPosition = new(x, y);
					if (!_loadedChunks.ContainsKey(chunkPosition))
					{
						LoadChunk(chunkPosition);
						yield return null;
					}
				}
			}
		}
        
		private void LoadChunk(Vector2Int chunkPosition)
		{
			if (_loadedChunks.ContainsKey(chunkPosition))
				return;

			Chunk loadedChunk;

			if (world.Chunks.TryGetValue(chunkPosition, out ChunkData chunkData))
			{
				loadedChunk = chunkBuilder.BuildExistingChunk(chunkData);
			}
			else
			{
				loadedChunk = chunkBuilder.BuildNewChunk(chunkPosition, worldSeed);
				world.AddChunk(loadedChunk.ChunkData);
			}

			loadedChunk.gameObject.SetActive(true);
			_loadedChunks.TryAdd(chunkPosition, loadedChunk);
			
			loadedChunk.transform.SetParent(_activeChunkContainer);
			_activeChunkContainer.name = $"Active Chunks: {_activeChunkContainer.childCount}";
		}
		
		private void UnloadChunk(Vector2Int chunkPosition)
		{
			if (_loadedChunks.TryRemove(chunkPosition, out Chunk chunk))
			{
				chunkBuilder.ReleaseChunk(chunk);
			}
		}

		private Chunk CreateChunk(Vector2Int chunkPosition)
		{
			Vector3 spawnPosition = new(chunkPosition.x, 0, chunkPosition.y);
			Vector3 chunkSize = new(world.WorldDetails.ChunkSize.x, 0, world.WorldDetails.ChunkSize.y);
			spawnPosition.Scale(chunkSize);
					
			Chunk chunk = Instantiate(world.WorldDetails.ChunkPrefab, world.transform).GetComponent<Chunk>();

			Transform chunkTransform = chunk.transform;
			chunkTransform.localPosition = spawnPosition;
			//chunk.Initialize(world, chunkPosition, GenerateTilesForChunk(chunk, chunkTransform));
			//GenerateRocks(chunk);

			return chunk;
		}

		private void GenerateWorld()
		{
			if (!world)
				throw new NullReferenceException($"Unable to Generate World. {nameof(world)} object is null");
			if (!world.WorldDetails)
				throw new NullReferenceException($"Unable to Generate World. {nameof(world.WorldDetails)} object is null");

			IEnumerator loadChunks = LoadChunks(
				new Vector2Int(-initialGenerationRadius, -initialGenerationRadius),
				new Vector2Int(initialGenerationRadius, initialGenerationRadius));
			StartCoroutine(loadChunks);
		}

		// private Tile[,] GenerateTilesForChunk(Chunk parentChunk, Transform parent)
		// {
		// 	Tile[,] tiles = new Tile[world.WorldDetails.ChunkTiles.x, world.WorldDetails.ChunkTiles.y];
		//
		// 	for (int x = 0; x < tiles.GetLength(0); x++)
		// 	{
		// 		for (int z = 0; z < tiles.GetLength(1); z++)
		// 		{
		// 			Vector2Int tilePosition = new(x, z);
		// 			Vector3 spawnPosition = new(x * world.WorldDetails.TileSize.x, 0, z * world.WorldDetails.TileSize.y);
		// 			Tile tile = Instantiate(world.WorldDetails.TilePrefab, parent).GetComponent<Tile>();
		//
		// 			Transform tileTransform = tile.transform;
		// 			tileTransform.localPosition = spawnPosition;
		// 			tile.Initialize(world, parentChunk, tilePosition);
		// 			
		// 			tiles[x, z] = tile;
		// 		}
		// 	}
		//
		// 	return tiles;
		// }

		// private void OnPlaceWorldObjectRequested(IMessage message)
		// {
		// 	if (message is not WorldObjectPlacementRequest placementRequest)
		// 	{
		// 		Debug.Log($"Received invalid {nameof(WorldObjectPlacementRequest)}");
		// 		return;
		// 	}
		// 	
		// 	if (!TrySpawnSingle(placementRequest.WorldObjectDetails, placementRequest.ChunkPosition, placementRequest.TilePosition, worldObjectParent))
		// 	{
		// 		Debug.LogWarning($"{placementRequest.WorldObjectDetails.Name} was not able to be spawned.", this);
		// 	}
		// }

		// private bool TrySpawnSingle(WorldObjectDetails details, Vector2Int chunkPosition, Vector2Int localChunkPosition, Transform container)
		// {
		// 	// Initial checks to avoid creating unnecessary objects and destroying them
		// 	if (world.WorldObjectManager.IsAtMaxCapacity(details))
		// 	{
		// 		Debug.LogWarning($"Unable to place {details.Name}. There are already {world.WorldObjectManager.GetObjectCount(details)} of {details.PlacementProperties.MaxObjectsAllowed} objects already placed.");
		// 		return false;
		// 	}
  //
		// 	if (!world.Chunks.TryGetValue(chunkPosition, out Chunk chunk))
		// 	{
		// 		Debug.LogWarning($"Unable to place {details.Name}. The chunk at {chunkPosition} does not currently exist.");
		// 		return false;
		// 	}
		// 	
		// 	// Todo: Need to check all positions for objects that are larger than 1x1x1
		// 	if (!chunk.IsValidTilePosition(localChunkPosition))
		// 	{
		// 		Debug.LogWarning($"Unable to place {details.Name}. {localChunkPosition} is not a valid tile position.");
  //               return false;
		// 	}
  //
		// 	// Todo: Need to check all positions for objects that are larger than 1x1x1
		// 	if (chunk.IsTileOccupied(localChunkPosition))
		// 	{
		// 		Debug.LogWarning($"Unable to place {details.Name}. {localChunkPosition} is already occupied by another world object");
		// 		return false;
		// 	}
  //
		// 	// By this point we should have all checks to make sure we can place a valid object
		// 	// Convert to world position to be placed
		// 	Vector3 worldPosition = world
		// 		.WorldPositionFromTilePosition(localChunkPosition, chunk.Position, centerOfTile: false)
		// 		.ToVector3(VectorSub.XSubY);
		// 	
		// 	// Instantiate and set position
		// 	WorldObject worldObject = Instantiate(details.Prefab, container);
		// 	worldObject.transform.SetPositionAndRotation(worldPosition, Quaternion.identity);
  //
		// 	// Get the details of the world object
		// 	worldObject.Init(details, new ChunkPlacementData(chunk.Position, localChunkPosition));
		// 	
		// 	// Add reference to the world object in the chunk and destroy the world object if unable to add to chunk
		// 	if (!chunk.TryAddWorldObject(worldObject))
		// 	{
		// 		worldObject.gameObject.Destroy();
		// 		Debug.LogWarning($"Unable to place {details.Name}. Object was unable to be added to the chunk.");
		// 		return false;
		// 	}
		// 	
		// 	world.WorldObjectManager.AddWorldObject(worldObject);
  //
		// 	return true;
		// }

		private void OnChunkBelowCameraChanged(Vector2Int newChunkBelowCamera)
		{
			Vector2Int loadFrom = new(newChunkBelowCamera.x - initialGenerationRadius, newChunkBelowCamera.y - initialGenerationRadius);
			Vector2Int loadTo = new(newChunkBelowCamera.x + initialGenerationRadius, newChunkBelowCamera.y + initialGenerationRadius);
		
			Vector2Int min = new(Math.Min(loadFrom.x, loadTo.x), Math.Min(loadFrom.y, loadTo.y));
			Vector2Int max = new(Math.Max(loadFrom.x, loadTo.x), Math.Max(loadFrom.y, loadTo.y));
			
			// Unload chunks
			foreach (Vector2Int loadedChunkPosition in _loadedChunks.Keys.ToList())
			{
				if (loadedChunkPosition.x < min.x || loadedChunkPosition.x > max.x || loadedChunkPosition.y < min.y || loadedChunkPosition.y > max.y)
				{
					UnloadChunk(loadedChunkPosition);
				}
			}
			
			// Load chunks
			IEnumerator loadChunks = LoadChunks(loadFrom, loadTo);
			StartCoroutine(loadChunks);
		}

		private void InjectContextMenuActions()
		{
			// Rock context menu injections
			
			Globals.ContextMenuInjections.InjectContextMenuOption<Rock>(
				ContextMenuBuilder.BuildPath("Destroy", "Single"),
				rock => wizardTaskManager.AddTask(new DestroyRocksTask(new List<Rock> {rock})));
			
			Globals.ContextMenuInjections.InjectContextMenuOption<Rock>(
				ContextMenuBuilder.BuildPath("Destroy", "Surrounding"),
				rock => wizardTaskManager.AddTask(new DestroyRocksTask(GetSurroundingRocks(rock.transform.position))));
			
			// Town Hall context menu injections
			
			Globals.ContextMenuInjections.InjectContextMenuOption<TownHall>(
				ContextMenuBuilder.BuildPath("Open Menu"),
				_ => OpenTownHallMenu());

			Globals.ContextMenuInjections.InjectContextMenuOption<TownHall>(
				ContextMenuBuilder.BuildPath("Print Info"),
				_ => print("I'M SORRY I DON'T HAVE ANY INFO FOR YOU"));
		}
		
		private static List<Rock> GetSurroundingRocks(Vector3 initialRockPosition)
		{
			Collider[] hits = Physics.OverlapSphere(initialRockPosition, 10);

			return hits.Select(hit => hit.GetComponentInParent<Rock>())
				.Where(rock => rock)
				.ToList();
		}
		
		private void OpenTownHallMenu()
		{
			GameEvents.UI.OpenUI.Request(this, new OpenUIEventArgs(UIWindow.TownHallWindow));
		}
		
		private class PlayerCameraChunkManager
		{
			private Vector2Int _chunkBelow;
        
			public event Action<Vector2Int> ChunkBelowChanged;

			public Vector2Int ChunkBelow
			{
				get => _chunkBelow;
				set
				{
					if (_chunkBelow != value)
					{
						_chunkBelow = value;
						ChunkBelowChanged?.Invoke(_chunkBelow);
					}
				}
			}
		}
	}
}