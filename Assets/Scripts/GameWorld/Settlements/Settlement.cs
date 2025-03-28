using System;
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

		public event Action<TownHall> TownHallUpdated; 

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
					TownHallUpdated?.Invoke(_townHall);
				}
			}
		}
	}
}