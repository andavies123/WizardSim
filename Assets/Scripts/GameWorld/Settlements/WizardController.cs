using System;
using Game;
using GameWorld.Characters.Wizards;
using GameWorld.Tiles;
using GeneralBehaviours.Utilities.ContextMenuBuilders;
using UnityEngine;
using Utilities.Attributes;

namespace GameWorld.Settlements
{
	[DisallowMultipleComponent]
	public class WizardController : MonoBehaviour
	{
		[SerializeField, Required] private WizardFactory wizardFactory;
		[SerializeField, Required] private WizardRepo wizardRepo;

		public void SpawnWizard(Tile tile, WizardType wizardType)
		{
			Vector2 tileWorldPosition = Globals.World.WorldPositionFromTile(tile, centerOfTile: true);
			Vector3 spawnPosition = new(tileWorldPosition.x, 1, tileWorldPosition.y);
			
			Wizard wizard = wizardFactory.CreateNewWizard(spawnPosition, wizardType);
			
			if (!wizardRepo.TryAddWizard(wizard))
			{
				Debug.LogWarning("Unable to spawn wizard");
				Destroy(wizard);
			}
		}

		private void Start()
		{
			InitializeContextMenu();
		}
		
		private void InitializeContextMenu()
		{
			foreach (WizardType wizardType in Enum.GetValues(typeof(WizardType)))
			{
				Globals.ContextMenuInjections.InjectContextMenuOption<Tile>(
					ContextMenuBuilder.BuildPath("Spawn Wizard", wizardType.ToString()),
					tile => SpawnWizard(tile, wizardType));
			}
		}
	}
}