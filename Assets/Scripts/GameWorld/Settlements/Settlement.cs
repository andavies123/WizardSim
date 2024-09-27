using GameWorld.Settlements.Interfaces;
using UnityEngine;

namespace GameWorld.Settlements
{
	// Todo: Should have reference to the town hall as there should be one per settlement
	// Todo: Should have reference to all settlement buildings
	public class Settlement : ISettlement
	{
		public Settlement(IWizardFactory wizardFactory, Transform wizardContainer)
		{
			WizardManager = new SettlementWizardManager(wizardFactory, wizardContainer);
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

	public interface ISettlementBuildingManager
	{
		
	}
}