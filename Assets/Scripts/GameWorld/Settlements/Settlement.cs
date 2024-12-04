using GameWorld.Settlements.Interfaces;
using GameWorld.WorldResources;
using UnityEngine;

namespace GameWorld.Settlements
{
	// Todo: Change this back to a MonoBehaviour
	// Todo: Should have reference to the town hall as there should be one per settlement
	// Todo: Should have reference to all settlement buildings
	public class Settlement : ISettlement
	{
		public TownResourceStockpile ResourceStockpile;
		
		public Settlement(
			IWizardFactory wizardFactory,
			Transform wizardContainer, 
			TownResourceStockpile resourceStockpile)
		{
			WizardManager = new SettlementWizardManager(wizardFactory, wizardContainer);
			ResourceStockpile = resourceStockpile;
		}

		public ISettlementWizardManager WizardManager { get; }
		public string SettlementName { get; set; } = "Un-named Settlement";

		public void Init()
		{
			WizardManager.Init();
		}

		public void CleanUp()
		{
			WizardManager.CleanUp();
		}
	}
}