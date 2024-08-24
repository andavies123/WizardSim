using System;
using GameWorld.WorldObjects;
using UnityEngine;

namespace GameWorld.Builders
{
	public class CreateWorldObjectRequestEventArgs : EventArgs
	{
		public WorldObjectDetails WorldObjectDetails { get; set; }
		public Vector2Int ChunkPosition { get; set; }
		public Vector2Int TilePosition { get; set; }
	}
}