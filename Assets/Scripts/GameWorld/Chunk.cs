using System;
using System.Collections.Generic;
using GameWorld.Tiles;
using GameWorld.WorldObjects;
using UnityEngine;

namespace GameWorld
{
	public class Chunk : MonoBehaviour
	{
		[SerializeField] private Transform worldObjectsParent;
        
		private Tile[,] _tiles;
		private WorldObject[,] _worldObjects;

		public Transform WorldObjectsParent => worldObjectsParent;
		public Vector2Int SizeInTiles { get; private set; }
		public Vector2Int Position { get; private set; }

		public void Initialize(Vector2Int sizeInTiles, Vector2Int position, Tile[,] tiles)
		{
			Position = position;
			_tiles = tiles;
			_worldObjects = new WorldObject[sizeInTiles.x, sizeInTiles.y]; // Should be same size as tile array

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

		public bool TryAddWorldObject(WorldObject worldObject, Vector2Int localChunkPosition)
		{
			List<Vector2Int> positions = new();

			for (int x = localChunkPosition.x; x < worldObject.Size.x; x++)
			{
				for (int z = localChunkPosition.y; z < worldObject.Size.y; z++)
				{
					Vector2Int position = new(x, z);
					
					// Make sure the positions are valid and there isn't anything here
					// Todo: Might be an issue for objects that span multiple chunks
					if (!IsValidTilePosition(position) || !IsWorldObjectSpaceEmpty(position))
						return false;
					
					positions.Add(position);
				}
			}
			
			// Update local collection
			positions.ForEach(position => _worldObjects[position.x, position.y] = worldObject);

			return true;
		}

		private bool IsValidTilePosition(Vector2Int tilePosition) =>
			tilePosition.x >= 0 && tilePosition.x < SizeInTiles.x &&
			tilePosition.y >= 0 && tilePosition.y < SizeInTiles.y;

		private bool IsWorldObjectSpaceEmpty(Vector2Int worldObjectPosition) =>
			!_worldObjects[worldObjectPosition.x, worldObjectPosition.y];
	}
}