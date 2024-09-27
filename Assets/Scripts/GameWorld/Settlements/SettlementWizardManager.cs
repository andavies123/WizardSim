using Game.MessengerSystem;
using GameWorld.Characters.Wizards;
using GameWorld.Characters.Wizards.Messages;
using GameWorld.Characters.Wizards.Tasks;
using GameWorld.Settlements.Interfaces;
using UnityEngine;

namespace GameWorld.Settlements
{
	public class SettlementWizardManager : ISettlementWizardManager
	{
		public SettlementWizardManager(IWizardFactory wizardFactory, Transform wizardContainer)
		{
			Factory = wizardFactory;
			Repo = new WizardRepo(wizardContainer);
			TaskManager = new WizardTaskManager();
		}
		
		public IWizardRepo Repo { get; }
		public IWizardFactory Factory { get; }
		public IWizardTaskManager TaskManager { get; }

		public void Init()
		{
			Factory.Initialize(Repo);
            
			Repo.WizardAdded += OnWizardAddedToRepo;
			Repo.WizardRemoved += OnWizardRemovedFromRepo;

			TaskManager.TaskAdded += OnWizardTaskAdded;
			
			GlobalMessenger.Subscribe<AddWizardTaskRequest>(OnAddWizardTaskRequested);
		}

		public void CleanUp()
		{
			Repo.WizardAdded -= OnWizardAddedToRepo;
			Repo.WizardRemoved -= OnWizardRemovedFromRepo;

			TaskManager.TaskAdded -= OnWizardTaskAdded;
			
			GlobalMessenger.Unsubscribe<AddWizardTaskRequest>(OnAddWizardTaskRequested);
		}
		
		public bool TryGetClosestWizard(Vector3 worldPosition, out Wizard closestWizard, out float distance)
		{
			closestWizard = null;
			distance = float.MaxValue;
            
			foreach (Wizard wizard in Repo.AllWizards.Values)
			{
				float currentDistance = Vector3.Distance(worldPosition, wizard.Transform.position);

				if (currentDistance >= distance)
					continue;
				
				distance = currentDistance;
				closestWizard = wizard;
			}
			
			return (bool)closestWizard;
		}

		private void OnWizardAddedToRepo(Wizard addedWizard) => TaskManager.AssignTaskToWizard(addedWizard);
		private void OnWizardRemovedFromRepo(Wizard removedWizard) => TaskManager.RemoveAllTasksFromWizard(removedWizard);
		private void OnAddWizardTaskRequested(AddWizardTaskRequest request) => TaskManager.AddTask(request.Task);

		private void OnWizardTaskAdded(IWizardTask wizardTask)
		{
			foreach (Wizard wizard in Repo.AllWizards.Values)
			{
				if (TaskManager.TryAssignTask(wizardTask, wizard))
					break;
			}
		}
	}
}