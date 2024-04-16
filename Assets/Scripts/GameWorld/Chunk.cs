using GameWorld.Tiles;
using UnityEngine;

namespace GameWorld
{
	public class Chunk : MonoBehaviour
	{
		private Tile[,] _tiles;

		public Vector2Int Position { get; private set; }

		public void Initialize(Vector2Int position, Tile[,] tiles)
		{
			Position = position;
			_tiles = tiles;
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
	}
}