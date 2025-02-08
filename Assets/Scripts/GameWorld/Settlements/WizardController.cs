using System;
using Game;
using GameWorld.Characters;
using GameWorld.Characters.Wizards;
using GameWorld.Characters.Wizards.Tasks;
using GameWorld.Tiles;
using GeneralBehaviours.Utilities.ContextMenuBuilders;
using UnityEngine;
using Utilities.Attributes;

namespace GameWorld.Settlements
{
	[DisallowMultipleComponent]
	public class WizardController : MonoBehaviour
	{
		[SerializeField, Required] private WizardFactory factory;
		[SerializeField, Required] private WizardRepo repo;
		[SerializeField, Required] private WizardTaskManager taskManager;

		public void SpawnWizard(Tile tile, WizardType wizardType)
		{
			Vector2 tileWorldPosition = Globals.World.WorldPositionFromTile(tile, centerOfTile: true);
			Vector3 spawnPosition = new(tileWorldPosition.x, 1, tileWorldPosition.y);
			
			Wizard wizard = factory.CreateNewWizard(spawnPosition, wizardType);

			if (repo.TryAddWizard(wizard))
			{
				wizard.Death.Died += OnWizardDied;
				taskManager.AssignTaskToWizard(wizard);
			}
			else
			{
				Debug.LogWarning("Unable to spawn wizard");
				Destroy(wizard);
			}
		}
		
		public bool TryGetClosestWizard(Vector3 worldPosition, out Wizard closestWizard, out float distance)
		{
			closestWizard = null;
			distance = float.MaxValue;
            
			foreach (Wizard wizard in repo.AllWizards.Values)
			{
				float currentDistance = Vector3.Distance(worldPosition, wizard.Transform.position);

				if (currentDistance >= distance)
					continue;
				
				distance = currentDistance;
				closestWizard = wizard;
			}
			
			return (bool)closestWizard;
		}

		private void Start()
		{
			InitializeContextMenu();
			taskManager.TaskAdded += OnWizardTaskAdded;
		}

		private void OnDestroy()
		{
			taskManager.TaskRemoved -= OnWizardTaskAdded;
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
		
		private void OnWizardDied(object sender, CharacterDiedEventArgs args)
		{
			if (args.DeadCharacter is not Wizard deadWizard)
				return;

			if (repo.TryRemoveWizard(deadWizard))
			{
				deadWizard.Death.Died -= OnWizardDied;
				taskManager.RemoveAllTasksFromWizard(deadWizard);
				
				Destroy(deadWizard.gameObject);
			}
		}
		
		private void OnWizardTaskAdded(IWizardTask wizardTask)
		{
			/*
			 * Todo: Find a better way to assign wizards to a new incoming task
			 * Todo: the same wizard is being assigned new tasks
			 */
			foreach (Wizard wizard in repo.AllWizards.Values)
			{
				if (taskManager.TryAssignTask(wizardTask, wizard))
					break;
			}
		}
	}
}