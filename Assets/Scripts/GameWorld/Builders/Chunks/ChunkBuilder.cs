using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using GameWorld.WorldObjects;
using UnityEngine;
using Utilities.Attributes;

namespace GameWorld.Builders.Chunks
{
	public class ChunkBuilder : MonoBehaviour
	{
		[SerializeField, Required] private Texture tileTexture;
		[SerializeField, Required] private List<Color> grassColors;
        
		private readonly ConcurrentQueue<Chunk> _chunkObjectPool = new();

		private Transform _releasedChunkContainer;
		
		// World Object Factories
		private RockObjectFactory _rockObjectFactory;
		
		public Chunk BuildNewChunk(Vector2Int chunkPosition, int worldSeed)
		{
			ChunkTerrain chunkTerrain = GenerateChunkTerrain(chunkPosition, worldSeed, Chunk.ChunkSize);
			ChunkData chunkData = new(chunkPosition, Chunk.ChunkSize, chunkTerrain);

			return BuildChunk(chunkData);
		}

		public Chunk BuildExistingChunk(ChunkData chunkData) => BuildChunk(chunkData);

		public void ReleaseChunk(Chunk chunk)
		{
			chunk.CleanUp();
			_chunkObjectPool.Enqueue(chunk);
			chunk.transform.SetParent(_releasedChunkContainer);
			_releasedChunkContainer.name = $"Released Chunks: {_releasedChunkContainer.childCount}";
		}
		
		private Chunk BuildChunk(ChunkData chunkData)
		{
			if (!_chunkObjectPool.TryDequeue(out Chunk chunk))
			{
				chunk = CreateChunkGameObject();
			}
			
			chunk.Initialize(chunkData);
			CreateChunkMesh(chunk);
			CreateWorldObjects(chunk);
			return chunk;
		}
		
		// Only in charge of generating the terrain for a chunk
		// Not in charge of instantiating any objects
		private ChunkTerrain GenerateChunkTerrain(Vector2Int chunkPosition, int worldSeed, Vector2Int chunkSize)
		{
			Random.State originalRandomState = Random.state;
			ChunkTerrain chunkTerrain = new(chunkSize);
            
			for (int x = 0; x < chunkSize.x; x++)
			{
				for (int z = 0; z < chunkSize.y; z++)
				{
					Vector2Int tilePosition = chunkPosition * Chunk.ChunkSize + new Vector2Int(x, z);
					
					int positionSeed = worldSeed ^ (tilePosition.x * 73856093 ^ tilePosition.y * 19349663);
					Random.InitState(positionSeed);

					// Grass
					chunkTerrain.TerrainType[x, z] = Random.Range(0, grassColors.Count);
					
					// Rocks
					if (Random.Range(0, 100) < 1)
					{
						chunkTerrain.Rocks.TryAdd(new Vector2Int(x, z), 0);
					}
				}
			}

			Random.state = originalRandomState;
			
			return chunkTerrain;
		}

		private static Chunk CreateChunkGameObject()
		{
			GameObject chunkGameObject = new("Chunk: Not Initialized", typeof(Chunk), typeof(ChunkMesh));
			Chunk chunk = chunkGameObject.GetComponent<Chunk>();
			chunk.ChunkMesh = chunk.GetComponent<ChunkMesh>();
			
			return chunk;
		}

		// In charge of creating a new chunk prefab
		// Not in charge of taking from the object pool
		private void CreateChunkMesh(Chunk chunk)
		{
			List<Vector3> vertices = new();
			List<List<int>> triangleIndices = new();
			List<Vector2> uvs = new();
			
			for (int grassType = 0; grassType < grassColors.Count; grassType++) // Initialize for all grass types
			{
				triangleIndices.Add(new List<int>());
			}

			ChunkData chunkData = chunk.ChunkData;
			Vector3Int chunkWorldPos = new(chunkData.Position.x * chunk.ChunkData.Size.x, 0, chunkData.Position.y * chunk.ChunkData.Size.y);
			
			for (int x = 0; x < chunkData.Terrain.TerrainType.GetLength(0); x++)
			{
				for (int z = 0; z < chunkData.Terrain.TerrainType.GetLength(1); z++)
				{
					vertices.AddRange(ChunkMeshHelper.GetOffsetVertices(chunkWorldPos + new Vector3Int(x, 0, z)));
					uvs.AddRange(ChunkMeshHelper.Uvs);
					
					int grassType = chunkData.Terrain.TerrainType[x, z];
					triangleIndices[grassType].AddRange(ChunkMeshHelper.GetOffsetTriangleIndices(x * chunkData.Terrain.TerrainType.GetLength(1) + z).ToList());
				}
			}

			chunk.ChunkMesh.Init(vertices.ToArray(), triangleIndices.Select(list => list.ToArray()).ToList(), uvs.ToArray(), grassColors, tileTexture);
		}

		private void CreateWorldObjects(Chunk chunk)
		{
			// Create Rocks
			foreach ((Vector2Int rockPosition, int rockType) in chunk.ChunkData.Terrain.Rocks)
			{
				// Todo: Get the world position
				// Todo: Get the correct rock object
				// Todo: Instantiate the world object

				WorldObject worldObject = _rockObjectFactory.CreateObject(chunk.ChunkData.Position, rockPosition);
				worldObject.transform.position = worldObject.PositionDetails.Position;
				chunk.AddWorldObject(worldObject);
			}
		}

		private void Awake()
		{
			_releasedChunkContainer = new GameObject("Released Chunks").transform;
			_rockObjectFactory = GetComponent<RockObjectFactory>();
		}
		
		// private void GenerateRocks(Chunk chunk)
		// {
		// 	int addedRocks = 0;
		//
		// 	if (!_worldObjectFactories.TryGetValue("Rock", out IWorldObjectFactory rockFactory) || rockFactory == null)
		// 	{
		// 		Debug.LogError($"Unable to find world object factory, Type: \"Rock\"");
		// 		return;
		// 	}
		// 	
		// 	while (addedRocks < rocksPerChunk)
		// 	{
		// 		Vector2Int localChunkPosition = RandomExt.RangeVector2Int(
		// 			0, world.WorldDetails.ChunkTiles.x,
		// 			0, world.WorldDetails.ChunkTiles.y);
		//
		// 		WorldObject rock = rockFactory.CreateObject(chunk.Position, localChunkPosition);
		// 		if (chunk.TryAddWorldObject(rock))
		// 		{
		// 			Vector3 worldPosition = world
		// 			 		.WorldPositionFromTilePosition(localChunkPosition, chunk.Position, centerOfTile: false)
		// 				    .ToVector3(VectorSub.XSubY) + new Vector3(0.5f, 0, 0.5f);
		// 			
		// 			//rock.transform.SetPositionAndRotation(worldPosition, Quaternion.identity);
		//
		// 			rock.transform.position = rock.PositionDetails.Position;
		// 			
		// 			addedRocks++;
		// 		}
		// 	}
		// }
	}
}