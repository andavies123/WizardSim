using System.Collections.Generic;
using System.Linq;
using Game;
using GameWorld.Characters.Wizards.Managers;
using GameWorld.WorldResources;
using UnityEngine;
using Utilities.Attributes;

namespace Upgrades
{
	public class UpgradeManager : MonoBehaviour
	{
		[SerializeField, Required] private WizardController wizardController;
		[SerializeField, Required] private TownResourceStockpile townResourceStockpile;
		
		private TownResourceRepo _townResourceRepo;

		private SpawnWizardUpgradeGroup _spawnWizardUpgradeGroup;
		private IncreaseResourcesUpgradeGroup _increaseResourcesUpgradeGroup;
		
		public List<Upgrade> GetRandomUpgrades(int count)
		{
			List<Upgrade> upgrades = new();

			// Todo: Figure out a better way to use the selection weights
			for (int index = 0; index < count; index++)
			{
				Upgrade upgrade = Random.Range(0, 2) % 2 == 0
					? _spawnWizardUpgradeGroup.GetUpgrade()
					: _increaseResourcesUpgradeGroup.GetUpgrade();

				// Make sure we don't have any duplicates
				if (upgrades.Any(x => x.Id == upgrade.Id))
				{
					index--;
					continue;
				}
				
				upgrades.Add(upgrade);
			}

			return upgrades;
		}

		private void Awake()
		{
			_townResourceRepo = Dependencies.Get<TownResourceRepo>();
			
			_spawnWizardUpgradeGroup = new SpawnWizardUpgradeGroup(10, wizardController);
			_increaseResourcesUpgradeGroup =
				new IncreaseResourcesUpgradeGroup(10, _townResourceRepo, townResourceStockpile);
		}
	}
}