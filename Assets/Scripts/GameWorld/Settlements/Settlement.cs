using GameWorld.Characters.Wizards;
using GameWorld.Settlements.Interfaces;
using UnityEngine;

namespace GameWorld.Settlements
{
	// Todo: Should have reference to the town hall as there should be one per settlement
	// Todo: Should have reference to all settlement buildings
	public class Settlement : ISettlement
	{
		public Settlement(IWizardSpawner wizardSpawner, Transform wizardContainer)
		{
			WizardSpawner = wizardSpawner;
			WizardRepo = new WizardRepo(wizardContainer);
			WizardTaskManager = new WizardTaskManager(WizardRepo);
			
			wizardSpawner.Initialize(WizardRepo);
		}

		public IWizardSpawner WizardSpawner { get; }
		public IWizardRepo WizardRepo { get; }
		public IWizardTaskManager WizardTaskManager { get; }
		public string SettlementName { get; set; } = "New Settlement";

		public bool TryGetClosestWizard(Vector3 worldPosition, out Wizard closestWizard, out float distance)
		{
			closestWizard = null;
			distance = float.MaxValue;
            
			foreach (Wizard wizard in WizardRepo.AllWizards.Values)
			{
				float currentDistance = Vector3.Distance(worldPosition, wizard.Transform.position);

				if (currentDistance >= distance)
					continue;
				
				distance = currentDistance;
				closestWizard = wizard;
			}
			
			return (bool)closestWizard;
		}
	}
}