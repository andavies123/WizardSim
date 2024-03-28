using UnityEngine;

namespace GameWorld.Tiles
{
	public class Tile : MonoBehaviour
	{
		public Vector2Int TilePosition { get; private set; }

		public void Initialize(Vector2Int tilePosition)
		{
			TilePosition = tilePosition;
		}
	}
}