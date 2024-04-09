using UnityEngine;

namespace GameWorld.Tiles
{
	public class Tile : MonoBehaviour
	{
		public Transform Transform { get; private set; }
		public Vector2Int TilePosition { get; private set; }

		public void Initialize(Vector2Int tilePosition)
		{
			TilePosition = tilePosition;
		}

		private void Awake()
		{
			Transform = transform;
		}
	}
}