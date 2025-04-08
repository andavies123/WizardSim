using Game.Events;
using Game.Values;
using GameWorld.WorldObjects;
using GameWorld.WorldResources;
using UnityEngine;
using Utilities.Attributes;

namespace GameWorld.Settlements
{
	public class Settlement : MonoBehaviour
	{
		[Header("Resource Objects")]
		[SerializeField, Required] private TownResourceStockpile resourceStockpile;
		
		public TownResourceStockpile ResourceStockpile => resourceStockpile;

		private TownHall _townHall; 
		public TownHall TownHall
		{
			get => _townHall;
			set
			{
				GameValues.Settlement.TownHall = value;
				
				if (value != _townHall)
				{
					_townHall = value;
					GameEvents.Settlement.TownHallPlaced.Raise(this, new TownHallPlacedEventArgs(_townHall));
				}
			}
		}
	}
}