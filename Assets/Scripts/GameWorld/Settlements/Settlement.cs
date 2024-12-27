using System;
using GameWorld.WorldObjects;
using GameWorld.WorldResources;
using UnityEngine;
using Utilities.Attributes;

namespace GameWorld.Settlements
{
	// Todo: Should have reference to all settlement buildings
	public class Settlement : MonoBehaviour
	{
		[Header("Resource Objects")]
		[SerializeField, Required] private TownResourceStockpile resourceStockpile;
		
		[Header("Wizard Objects")]
		[SerializeField, Required] private WizardFactory wizardFactory;
		[SerializeField, Required] private Transform wizardContainer;

		private TownHall _townHall;

		public event Action<TownHall> TownHallUpdated; 

		public ISettlementWizardManager WizardManager { get; private set; }
		public TownResourceStockpile ResourceStockpile => resourceStockpile;
		public string SettlementName { get; set; } = "Un-named Settlement";

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
		
		private void Awake()
		{
			WizardManager = new SettlementWizardManager(wizardFactory, wizardContainer);
			WizardManager.Init();
		}

		private void OnDestroy()
		{
			WizardManager.CleanUp();
		}
	}
}