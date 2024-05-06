using Game.MessengerSystem;
using UnityEngine;

namespace Game.Messages
{
	public class BeginPlacementModeRequest : IMessage
	{
		public BeginPlacementModeRequest(GameObject placementPrefab) => PlacementPrefab = placementPrefab;
		
		public GameObject PlacementPrefab { get; }
	}
}