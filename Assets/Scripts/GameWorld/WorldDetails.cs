using GameWorld.Builders;
using GameWorld.Builders.Chunks;
using GameWorld.Tiles;
using UnityEngine;

namespace GameWorld
{
	[CreateAssetMenu(menuName = "World/Details", fileName = "WorldDetails", order = 0)]
	public class WorldDetails : ScriptableObject
	{
		[Header("Generation Settings")]
		[Tooltip("The size of the tile in world units")]
		[SerializeField] private Vector2Int tileSize = Vector2Int.one;

		[Tooltip("The size of the chunk in tiles")]
		[SerializeField] private Vector2Int chunkTiles = new(10, 10);

		[Header("Prefabs")]
		[Tooltip("The prefab that will be instantiated when generating tiles")]
		[SerializeField] private Tile tilePrefab;

		[Tooltip("The prefab that will be instantiated when generating chunks")]
		[SerializeField] private Chunk chunkPrefab;
		
		/// <summary>
		/// The size of the tile in world units
		/// </summary>
		public Vector2Int TileSize => tileSize;

		/// <summary>
		/// The size of a chunk in tiles
		/// </summary>
		public Vector2Int ChunkTiles => chunkTiles;

		/// <summary>
		/// The size of a chunk in world units.
		/// Calculated using <see cref="TileSize"/> and <see cref="ChunkTiles"/>
		/// </summary>
		public Vector2Int ChunkSize => chunkTiles * TileSize;

		/// <summary>
		/// The prefab that will be instantiated when generating tiles
		/// </summary>
		public Tile TilePrefab => tilePrefab;

		/// <summary>
		/// The prefab that will be instantiated when generating chunks
		/// </summary>
		public Chunk ChunkPrefab => chunkPrefab;
	}
}