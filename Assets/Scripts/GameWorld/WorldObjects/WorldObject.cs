using UnityEngine;

namespace GameWorld.WorldObjects
{
	public class WorldObject : MonoBehaviour
	{
		[SerializeField] private Vector2Int size;

		public Vector2Int Size => size;
		public Vector2Int LocalChunkPosition { get; private set; }

		public void Init(Vector2Int localChunkPosition)
		{
			LocalChunkPosition = localChunkPosition;
		}
	}
}