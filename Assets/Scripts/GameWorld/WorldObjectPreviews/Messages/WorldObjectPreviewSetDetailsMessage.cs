using GameWorld.WorldObjects;
using MessagingSystem;
using UnityEngine;

namespace GameWorld.WorldObjectPreviews.Messages
{
	public class WorldObjectPreviewSetDetailsMessage : Message
	{
		public WorldObjectDetails Details { get; set; }
	}

	public class WorldObjectPreviewSetPositionMessage : Message
	{
		public Vector2Int ChunkPosition { get; set; }
		public Vector2Int TilePosition { get; set; }
	}

	public class WorldObjectPreviewDeleteMessage : Message
	{
		
	}

	public class WorldObjectPreviewSetVisibilityMessage : Message
	{
		public bool Visibility { get; set; }
	}
}