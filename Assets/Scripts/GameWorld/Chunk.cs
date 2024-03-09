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
	}
}