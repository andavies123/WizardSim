using Game.MessengerSystem;
using UnityEngine;

namespace Game.Messages
{
	public class BeginPlacementModeRequest : Message
	{
		public BeginPlacementModeRequest(object sender, GameObject placementPrefab) : base(sender)
		{
			PlacementPrefab = placementPrefab;
		}

		public GameObject PlacementPrefab { get; }
	}
}