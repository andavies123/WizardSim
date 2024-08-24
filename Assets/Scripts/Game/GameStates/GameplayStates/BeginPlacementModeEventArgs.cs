using System;
using GameWorld.WorldObjects;

namespace Game.GameStates.GameplayStates
{
	public class BeginPlacementModeEventArgs : EventArgs
	{
		public BeginPlacementModeEventArgs(WorldObjectDetails placementDetails)
		{
			PlacementDetails = placementDetails;
		}
		
		public WorldObjectDetails PlacementDetails { get; }
	}
}