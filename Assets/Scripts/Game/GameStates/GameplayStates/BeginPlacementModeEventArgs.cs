using System;
using UnityEngine;

namespace Game.GameStates.GameplayStates
{
	public class BeginPlacementModeEventArgs : EventArgs
	{
		public BeginPlacementModeEventArgs(GameObject placementPrefab)
		{
			PlacementPrefab = placementPrefab;
		}
		
		public GameObject PlacementPrefab { get; }
	}
}