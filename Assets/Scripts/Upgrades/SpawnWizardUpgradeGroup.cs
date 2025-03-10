using System;
using System.Collections.Generic;
using System.Linq;
using GameWorld.Characters.Wizards;
using GameWorld.Characters.Wizards.Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Upgrades
{
	public class SpawnWizardUpgradeGroup : UpgradeGroup
	{
		private readonly List<WizardType> _wizardTypes = Enum.GetValues(typeof(WizardType)).Cast<WizardType>()
			.Where(wizardType => wizardType != WizardType.Undecided).ToList();

		private readonly WizardController _wizardController;

		public SpawnWizardUpgradeGroup(float selectionWeight, WizardController wizardController)
			: base(selectionWeight)
		{
			_wizardController = wizardController ? wizardController : throw new ArgumentNullException(nameof(wizardController));
		}

		public override Upgrade GetUpgrade()
		{
			WizardType wizardType = _wizardTypes[Random.Range(0, _wizardTypes.Count)];
			
			return new Upgrade
			{
				Id = $"{nameof(SpawnWizardUpgradeGroup)}.{wizardType}",
				Title = $"Spawn {wizardType} Wizard",
				Description = $"Spawns a single {wizardType} wizard into your settlement",
				Apply = () => _wizardController.SpawnWizard(Vector3.zero, wizardType),
				DisplaySettings = new UpgradeDisplaySettings
				{
					BackgroundColor = Color.cyan,
					OutlineColor = Color.yellow
				}
			};
		}
	}
}