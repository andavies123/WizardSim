using Game.Events;
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

		private TownHall _townHall; 

		public TownResourceStockpile ResourceStockpile => resourceStockpile;
		public string SettlementName { get; set; } = "Unnamed Settlement";

		public TownHall TownHall
		{
			get => _townHall;
			set
			{
				if (value != _townHall)
				{
					_townHall = value;
					GameEvents.Settlement.TownHallPlaced.Raise(this, new TownHallPlacedEventArgs(_townHall));
				}
			}
		}
	}
}