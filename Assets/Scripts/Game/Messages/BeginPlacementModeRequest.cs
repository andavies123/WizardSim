using Game.MessengerSystem;
using GameWorld.WorldObjects;

namespace Game.Messages
{
	public class BeginPlacementModeRequest : Message
	{
		public BeginPlacementModeRequest(object sender, WorldObjectDetails placementDetails) : base(sender)
		{
			PlacementDetails = placementDetails;
		}

		public WorldObjectDetails PlacementDetails { get; }
	}
}