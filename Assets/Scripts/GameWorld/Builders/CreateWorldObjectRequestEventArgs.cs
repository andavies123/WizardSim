using System;
using UnityEngine;

namespace GameWorld.Builders
{
	public class CreateWorldObjectRequestEventArgs : EventArgs
	{
		public GameObject WorldObjectPrefab { get; set; }
		public Vector2Int ChunkPosition { get; set; }
		public Vector2Int TilePosition { get; set; }
	}
}