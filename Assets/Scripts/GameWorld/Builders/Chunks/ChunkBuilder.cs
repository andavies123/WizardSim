using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities.Attributes;

namespace GameWorld.Builders.Chunks
{
	public class ChunkBuilder : MonoBehaviour
	{
		[SerializeField, Required] private Texture tileTexture;
		[SerializeField, Required] private List<Color> grassColors;
        
		private readonly ConcurrentQueue<Chunk> _chunkObjectPool = new();
		
		public Chunk BuildNewChunk(Vector2Int chunkPosition, int worldSeed)
		{
			ChunkTerrain chunkTerrain = GenerateChunkTerrain(chunkPosition, worldSeed, Chunk.ChunkSize);
			ChunkData chunkData = new(chunkPosition, chunkTerrain);

			return BuildChunk(chunkData);
		}

		public Chunk BuildExistingChunk(ChunkData chunkData) => BuildChunk(chunkData);

		public void ReleaseChunk(Chunk chunk)
		{
			chunk.CleanUp();
			_chunkObjectPool.Enqueue(chunk);
		}
		
		private Chunk BuildChunk(ChunkData chunkData)
		{
			if (!_chunkObjectPool.TryDequeue(out Chunk chunk))
			{
				chunk = CreateChunkGameObject();
			}
			
			chunk.Initialize(chunkData);
			CreateChunkMesh(chunk);
			return chunk;
		}
		
		// Only in charge of generating the terrain for a chunk
		// Not in charge of instantiating any objects
		private ChunkTerrain GenerateChunkTerrain(Vector2Int chunkPosition, int worldSeed, int chunkSize)
		{
			Random.State originalRandomState = Random.state;

			ChunkTerrain chunkTerrain = new(chunkSize);
            
			for (int x = 0; x < chunkSize; x++)
			{
				for (int z = 0; z < chunkSize; z++)
				{
					Vector2Int tilePosition = chunkPosition * Chunk.ChunkSize + new Vector2Int(x, z);
					int positionSeed = worldSeed ^ (tilePosition.x * 73856093 ^ tilePosition.y * 19349663);
					Random.InitState(positionSeed);

					chunkTerrain.GrassType[x, z] = Random.Range(0, grassColors.Count);
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
			Vector3Int chunkWorldPos = new Vector3Int(chunkData.Position.x, 0, chunkData.Position.y) * Chunk.ChunkSize;
			
			for (int x = 0; x < chunkData.Terrain.GrassType.GetLength(0); x++)
			{
				for (int z = 0; z < chunkData.Terrain.GrassType.GetLength(1); z++)
				{
					vertices.AddRange(ChunkMeshHelper.GetOffsetVertices(chunkWorldPos + new Vector3Int(x, 0, z)));
					uvs.AddRange(ChunkMeshHelper.Uvs);
					
					int grassType = chunkData.Terrain.GrassType[x, z];
					triangleIndices[grassType].AddRange(ChunkMeshHelper.GetOffsetTriangleIndices(x * chunkData.Terrain.GrassType.GetLength(1) + z).ToList());
				}
			}

			chunk.ChunkMesh.Init(vertices.ToArray(), triangleIndices.Select(list => list.ToArray()).ToList(), uvs.ToArray(), grassColors, tileTexture);
		}
	}
}