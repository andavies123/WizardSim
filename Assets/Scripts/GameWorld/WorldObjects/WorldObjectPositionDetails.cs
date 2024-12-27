using UnityEngine;

namespace GameWorld.WorldObjects
{
	public readonly struct WorldObjectPositionDetails
	{
		public WorldObjectPositionDetails(Vector3 position, Vector3 size)
		{
			Position = position;
			Size = size;
		}

		public Vector3 Position { get; }
		public Vector3 Size { get; }

		public Vector3 Center => Position + (Size / 2f);
	}
}