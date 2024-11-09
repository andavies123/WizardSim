using GameWorld.WorldObjects;
using MessagingSystem;

namespace Game.Messages
{
	public class BeginPlacementModeRequest : Message
	{
		public WorldObjectDetails PlacementDetails { get; set; }
	}
}