using System.Collections.Generic;
using GameWorld.Tiles;
using UnityEngine;

namespace GameWorld
{
	public class World : MonoBehaviour
	{
		[SerializeField] private WorldDetails worldDetails;
		
		private readonly Dictionary<Vector2Int, Chunk> _chunks = new();

		public WorldDetails WorldDetails => worldDetails;
		public IReadOnlyDictionary<Vector2Int, Chunk> Chunks => _chunks;

		public void AddChunk(Chunk chunk)
		{
			_chunks[chunk.Position] = chunk;
		}

		/// <summary>
		/// Calculates the world position based off of a given tile
		/// </summary>
		/// <param name="tile">The tile object containing chunk and tile information</param>
		/// <param name="centerOfTile">True if the position at the center of the tile will be returned. False if not</param>
		/// <returns>The world position in unity units of the given tile</returns>
		public Vector2 WorldPositionFromTile(Tile tile, bool centerOfTile = true)
		{
			Vector2 tilePosition = tile.TilePosition * worldDetails.TileSize;
			Vector2 chunkPosition = tile.ParentChunk.Position * worldDetails.ChunkSize;

			Vector2 worldPosition = chunkPosition + tilePosition;

			if (centerOfTile)
				worldPosition += worldDetails.TileSize / 2;

			return worldPosition;
		}

		public Vector2Int ChunkPositionFromWorldPosition(Vector2 worldPosition) => new(
			Mathf.FloorToInt(worldPosition.x / worldDetails.ChunkSize.x), 
			Mathf.FloorToInt(worldPosition.y / worldDetails.ChunkSize.x));

		/// <summary>
		/// Calculates and returns the Chunk object at the given world position
		/// </summary>
		/// <param name="worldPosition">Position in the world</param>
		/// <param name="chunk">The chunk object at the given world position</param>
		/// <returns>True if the chunk was found at the given world position. False if it wasn't</returns>
		public bool TryGetChunkFromWorldPosition(Vector2 worldPosition, out Chunk chunk)
		{
			Vector2Int chunkPosition = ChunkPositionFromWorldPosition(worldPosition);

			if (!_chunks.TryGetValue(chunkPosition, out chunk))
			{
				Debug.LogWarning($"Chunk was not found at position: {chunkPosition}");
				return false;
			}

			return true;
		}
		
		public bool TryGetChunkFromWorldPosition(Vector3 worldPosition, out Chunk chunk) => 
			TryGetChunkFromWorldPosition(new Vector2(worldPosition.x, worldPosition.z), out chunk);

		/// <summary>
		/// Calculates and returns the Tile object at the given world position
		/// </summary>
		/// <param name="worldPosition">Position in the world</param>
		/// <param name="tile">The tile object that was found to be at the given world position</param>
		/// <returns>True if the tile object at the world position exists. False if not</returns>
		public bool TryGetTileFromWorldPosition(Vector2 worldPosition, out Tile tile)
		{
			// Initialize the out argument
			tile = null;
            
			// Get the chunk position based off the world position
			Vector2Int chunkPosition = ChunkPositionFromWorldPosition(worldPosition);

			// Make sure the chunk exists
			if (!_chunks.TryGetValue(chunkPosition, out Chunk chunk))
			{
				Debug.LogWarning($"Unable to get chunk at position: {chunkPosition}");
				return false;
			}

			// Calculate the chunk position in world coordinates
			Vector2Int chunkWorldPosition = chunkPosition * worldDetails.ChunkSize;
			
			// Find the local position inside of the chunk
			Vector2 localPosition = worldPosition - chunkWorldPosition;
			
			// Based off the local position, calculate which tile it is
			int tilePositionX = Mathf.FloorToInt(localPosition.x / worldDetails.ChunkTiles.x);
			int tilePositionY = Mathf.FloorToInt(localPosition.y / worldDetails.ChunkTiles.y);
			Vector2Int tilePosition = new(tilePositionX, tilePositionY);
			
			// Make sure the tile exists
			if (!chunk.TryGetTile(tilePosition, out tile))
			{
				Debug.LogWarning($"Unable to get tile at position: {tilePosition}");
				return false;
			}

			// Return the tile
			return true;
		}

		public bool TryGetTileFromWorldPosition(Vector3 worldPosition, out Tile tile) => 
			TryGetTileFromWorldPosition(new Vector2(worldPosition.x, worldPosition.z), out tile);
	}
}