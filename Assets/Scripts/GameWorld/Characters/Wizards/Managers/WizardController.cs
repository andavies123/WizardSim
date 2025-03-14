using System;
using System.Collections.Generic;
using System.Linq;
using Game;
using GameWorld.Characters.Wizards.Tasks;
using GameWorld.Tiles;
using GeneralBehaviours.Utilities.ContextMenuBuilders;
using UnityEngine;
using Utilities.Attributes;

namespace GameWorld.Characters.Wizards.Managers
{
	[DisallowMultipleComponent]
	public class WizardController : MonoBehaviour
	{
		[SerializeField, Required] private WizardFactory factory;
		[SerializeField, Required] private WizardRepo repo;
		[SerializeField, Required] private WizardTaskManager taskManager;

		public void SpawnWizard(Tile tile, WizardType wizardType) =>
			SpawnWizard(Globals.World.WorldPositionFromTile(tile, centerOfTile: true), wizardType);
		
		public void SpawnWizard(Vector3 worldPosition, WizardType wizardType)
		{
			Vector3 spawnPosition = new(worldPosition.x, 1, worldPosition.y);
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
				
				if (wizardType != WizardType.Undecided)
				{
					Globals.ContextMenuInjections.InjectContextMenuOption<Wizard>(
						ContextMenuBuilder.BuildPath("Assign", wizardType.ToString()),
						menuClickCallback: wizard => factory.SetWizardType(wizard, wizardType),
						isVisibleFunc: wizard => wizard.WizardType == WizardType.Undecided);
				}
			}
			
			Globals.ContextMenuInjections.InjectContextMenuOption<Wizard>(
				ContextMenuBuilder.BuildPath("Action", "Idle"),
				wizard => wizard.StateMachine.Idle(),
				isEnabledFunc: wizard => !wizard.IsIdling);
			
			// Globals.ContextMenuInjections.InjectContextMenuOption<Wizard>(
			// 	ContextMenuBuilder.BuildPath("Action", "Move To"),
			// 	_ => _messageBroker.PublishSingle(
			// 		new StartInteractionRequest
			// 		{
			// 			Sender = this,
			// 			InteractionCallback = OnInteractionCallback
			// 		}));

			Globals.ContextMenuInjections.InjectContextMenuOption<Wizard>(
				ContextMenuBuilder.BuildPath("Kill"),
				wizard => wizard.Damageable.DealDamage(wizard.Health.CurrentHealth, null, null));
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
			List<Wizard> wizards = repo.AllWizards.Values
				.OrderBy(wiz => wiz.TaskHandler.AssignedTaskCount)
				.ToList();
			
			foreach (Wizard wizard in wizards)
			{
				if (taskManager.TryAssignTask(wizardTask, wizard))
					break;
			}
		}
	}
}