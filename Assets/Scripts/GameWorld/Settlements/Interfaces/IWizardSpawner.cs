using GameWorld.Characters.Wizards;
using UnityEngine;

namespace GameWorld.Settlements.Interfaces
{
	public interface IWizardSpawner
	{
		void Initialize(IWizardRepo wizardRepo);
		void SpawnWizard(Vector3 worldPosition, WizardType wizardType);
	}
}