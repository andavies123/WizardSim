using System.Collections.Generic;
using GameWorld.Tiles;
using GameWorld.WorldObjects;
using UnityEngine;

namespace GameWorld
{
	public class Chunk : MonoBehaviour
	{
		[SerializeField] private Transform worldObjectsParent;

		private readonly World parentWorld;
		private Tile[,] _tiles;
		private bool[,] _occupiedTiles;

		public Vector2Int SizeInTiles { get; private set; }
		public Vector2Int Position { get; private set; }

		public void Initialize(World world, Vector2Int position, Tile[,] tiles)
		{
			SizeInTiles = world.WorldDetails.ChunkTiles;
			Position = position;
			_tiles = tiles;
			_occupiedTiles = new bool[SizeInTiles.x, SizeInTiles.y];

			gameObject.name = $"Chunk - {position}";
		}

		public bool TryGetTile(Vector2Int tilePosition, out Tile tile) => TryGetTile(tilePosition.x, tilePosition.y, out tile);

		public bool TryGetTile(int tileXPosition, int tileZPosition, out Tile tile)
		{
			tile = null;
			
			if (tileXPosition < 0 || tileXPosition >= _tiles.GetLength(0) || tileZPosition < 0 || tileZPosition >= _tiles.GetLength(1))
				return false;

			tile = _tiles[tileXPosition, tileZPosition];
			return true;
		}

		public bool TryAddWorldObject(WorldObject worldObject)
		{
			List<Vector2Int> positions = new();

			Vector2Int tilePosition = worldObject.ChunkPlacementData.TilePosition;
			Vector3Int size = worldObject.Details.PlacementProperties.Size;
			for (int x = tilePosition.x; x < tilePosition.x + size.x; x++)
			{
				for (int z = tilePosition.y; z < tilePosition.y + size.z; z++)
				{
					Vector2Int position = new(x, z);
					
					// Make sure the positions are valid and there isn't anything here
					// BUG: Might be an issue for objects that span multiple chunks
					if (!IsValidTilePosition(position) || IsTileOccupied(position))
						return false;
					
					positions.Add(position);
				}
			}
			
			// Update local collection
			positions.ForEach(position => _occupiedTiles[position.x, position.y] = true);

			return true;
		}

		public bool IsValidTilePosition(Vector2Int tilePosition) =>
			tilePosition.x >= 0 && tilePosition.x < SizeInTiles.x &&
			tilePosition.y >= 0 && tilePosition.y < SizeInTiles.y;

		public bool IsTileOccupied(Vector2Int localChunkPosition) =>
			_occupiedTiles[localChunkPosition.x, localChunkPosition.y];
	}
}