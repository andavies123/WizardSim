using GameWorld.Tiles;
using UnityEngine;

namespace GameWorld.Utilities
{
	public class WorldBuilder : MonoBehaviour
	{
		[SerializeField] private World world;
		[SerializeField] private GameObject chunkPrefab;
		[SerializeField] private GameObject tilePrefab;
		
		[Header("Settings")]
		[SerializeField] private int initialGenerationRadius = 5;

		[Header("Chunk Settings")] 
		[SerializeField] private int chunkSize = 10;

		private void Awake()
		{
			GenerateWorld();
		}

		private void GenerateWorld()
		{
			Transform worldTransform = world.transform; // Added so world.transform isn't called for each loop iteration
			
			for (int x = -initialGenerationRadius + 1; x < initialGenerationRadius; x++)
			{
				for (int z = -initialGenerationRadius + 1; z < initialGenerationRadius; z++)
				{
					Vector2Int chunkPosition = new(x, z);
					Vector3 spawnPosition = new Vector3(x, 0, z) * chunkSize;
					Chunk chunk = Instantiate(chunkPrefab, worldTransform).GetComponent<Chunk>();

					Transform chunkTransform = chunk.transform;
					chunkTransform.localPosition = spawnPosition;
					chunkTransform.name = $"Chunk - {chunkPosition}";
					chunk.Initialize(chunkPosition, GenerateTilesForChunk(chunkTransform));
					
					world.AddChunk(chunk);
				}
			}
		}

		private Tile[,] GenerateTilesForChunk(Transform parent)
		{
			Tile[,] tiles = new Tile[chunkSize, chunkSize];

			for (int x = 0; x < tiles.GetLength(0); x++)
			{
				for (int z = 0; z < tiles.GetLength(1); z++)
				{
					Vector2Int tilePosition = new(x, z);
					Vector3 spawnPosition = new(x, 0, z);
					Tile tile = Instantiate(tilePrefab, parent).GetComponent<Tile>();

					Transform tileTransform = tile.transform;
					tileTransform.localPosition = spawnPosition;
					tileTransform.name = $"Tile - {tilePosition}";
					tile.Initialize(tilePosition);
					
					tiles[x, z] = tile;
				}
			}

			return tiles;
		}
	}
}