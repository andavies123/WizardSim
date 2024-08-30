using Game.MessengerSystem;
using GameWorld.WorldObjects;

namespace Game.Messages
{
	public class BeginPlacementModeRequest : Message
	{
		public WorldObjectDetails PlacementDetails { get; set; }
	}
}