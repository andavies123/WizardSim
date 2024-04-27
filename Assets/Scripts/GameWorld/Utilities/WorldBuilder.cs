using System;
using Extensions;
using Game;
using GameWorld.Tiles;
using GameWorld.WorldObjects;
using UnityEngine;
using Utilities;

namespace GameWorld.Utilities
{
	public class WorldBuilder : MonoBehaviour
	{
		[SerializeField] private World world;
		[SerializeField] private Transform worldObjectParent;

		[Header("Settings")]
		[SerializeField] private int initialGenerationRadius = 5;

		[Header("Rock Generation Settings")]
		[SerializeField] private GameObject rockPrefab;
		[SerializeField] private int rocksPerChunk;

		public bool TrySpawnRock(Vector2Int chunkPosition, Vector2Int tilePosition)
		{
			Vector3 worldPosition = world.WorldPositionFromTilePosition(tilePosition, chunkPosition).ToVector3(VectorSub.XSubY);
			
			Chunk chunk = world.Chunks[chunkPosition];
			
			WorldObject rock = Instantiate(rockPrefab, worldObjectParent).GetComponent<WorldObject>();
			rock.transform.SetPositionAndRotation(worldPosition, Quaternion.identity);
			rock.gameObject.name = "Rock";
				
			// Try and add it to the chunk. If unable to we should destroy it and move on.
			// Todo: Update this so that it doesn't keep instantiating a ton of rocks while it looks for somewhere to put it
			if (!chunk.TryAddWorldObject(rock, tilePosition))
			{
				Destroy(rock.gameObject);
				return false;
			}

			return true;
		}

		private bool TrySpawnRock(Chunk chunk, Vector2Int tilePosition)
		{
			Vector3 worldPosition = world.WorldPositionFromTilePosition(tilePosition, chunk.Position).ToVector3(VectorSub.XSubY);
			
			WorldObject rock = Instantiate(rockPrefab, worldObjectParent).GetComponent<WorldObject>();
			rock.transform.SetPositionAndRotation(worldPosition, Quaternion.identity);
			rock.gameObject.name = "Rock";
				
			// Try and add it to the chunk. If unable to we should destroy it and move on.
			// Todo: Update this so that it doesn't keep instantiating a ton of rocks while it looks for somewhere to put it
			if (!chunk.TryAddWorldObject(rock, tilePosition))
			{
				Destroy(rock.gameObject);
				return false;
			}

			return true;
		}

		private void Awake()
		{
			Dependencies.RegisterDependency(this);
			
			GenerateWorld();
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

				if (TrySpawnRock(chunk, localChunkPosition))
					addedRocks++;
			}
		}
	}
}