using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using Game.MessengerSystem;
using GameWorld;
using GameWorld.Tiles;
using GeneralBehaviours;
using GeneralBehaviours.Health;
using Stats;
using TaskSystem.Interfaces;
using UI;
using UI.ContextMenus;
using UI.Messages;
using UnityEngine;
using Utilities;
using Wizards.States;
using Wizards.Tasks;

namespace Wizards
{
	[RequireComponent(typeof(Interactable))]
	[RequireComponent(typeof(ContextMenuUser))]
	public class Wizard : Entity, ITaskUser<IWizardTask>
	{
		[SerializeField] private WizardStats stats;

		private Interactable _interactable;
		private ContextMenuUser _contextMenuUser;
		
		public string Name { get; set; }
		public WizardType WizardType { get; set; } = WizardType.Earth;
		
		public Transform Transform { get; private set; }
		public WizardStateMachine StateMachine { get; private set; }
		public Movement Movement { get; private set; }
		public Health Health { get; private set; }
		

		public override string DisplayName => Name;
		public override MovementStats MovementStats => Stats.MovementStats;
		public WizardStats Stats => stats;

		public bool IsIdling => StateMachine.CurrentState is WizardIdleState;

		#region ITaskUser Implementation

		private readonly List<IWizardTask> _assignedTasks = new();

		public bool IsAssignedTask => CurrentTask != null;
		public bool CanBeAssignedTask => !IsAssignedTask;
		public IWizardTask CurrentTask { get; private set; }
		public IReadOnlyList<IWizardTask> AssignedTasks => _assignedTasks;

		public void AssignTask(IWizardTask task)
		{
			if (task == null)
				return;
			
			task.AssignedWizard = this;
			_assignedTasks.Add(task);
			task.WizardTaskState.SetWizard(this);
			
			AssignNewCurrentTask();
		}

		public void UnassignTask(IWizardTask task)
		{
			if (CurrentTask == null && _assignedTasks.IsEmpty())
				return; // No task to unassign
			
			if (task == CurrentTask)
			{
				CurrentTask = null;
				AssignNewCurrentTask();
				if (!IsAssignedTask)
				{
					StateMachine.Idle();
				}
			}
			else
			{
				_assignedTasks.Remove(task);
			}
		}

		private void AssignNewCurrentTask()
		{
			if (CurrentTask != null || _assignedTasks.IsEmpty())
				return; // Current task already exists or there are no tasks to be assigned

			IWizardTask newCurrentTask = _assignedTasks[0];
			_assignedTasks.RemoveAt(0);
			CurrentTask = newCurrentTask;
			StateMachine.OverrideCurrentState(CurrentTask.WizardTaskState);
		}

		#endregion
		
		public void InitializeWizard(string wizardName, WizardType wizardType)
		{
			Name = wizardName;
			WizardType = wizardType;
			
			gameObject.name = $"Wizard - {Name} - {wizardType.ToString()}";
		}
		
		private void Awake()
		{
			Transform = transform;
			StateMachine = GetComponent<WizardStateMachine>();
			Movement = GetComponent<Movement>();
			Health = GetComponent<Health>();
			_interactable = GetComponent<Interactable>();
			_contextMenuUser = GetComponent<ContextMenuUser>();

			Health.CurrentHealthChanged += OnCurrentHealthChanged;
		}

		private void Start()
		{
			UpdateInteractableInfoText();
			InitializeContextMenu();
		}

		private void OnDestroy()
		{
			Health.CurrentHealthChanged -= OnCurrentHealthChanged;
		}

		private void UpdateInteractableInfoText()
		{
			_interactable.TitleText = Name;
			_interactable.InfoText = $"Wizard - {Health.CurrentHealth:0}/{Health.MaxHealth:0} ({Health.CurrentHealth.PercentageOf(Health.MaxHealth):0}%)";
		}

		private void InitializeContextMenu()
		{
			_contextMenuUser.AddMenuItem(new ContextMenuItem("Idle", () => StateMachine.Idle(), isEnabledFunc: () => !IsIdling));
			_contextMenuUser.AddMenuItem(new ContextMenuItem("Move To", () => GlobalMessenger.Publish(new StartInteractionRequest(OnInteractionCallback))));
			_contextMenuUser.AddMenuItem(new ContextMenuItem("Heal 10%", () => IncreaseHealth(0.1f), isEnabledFunc: IsNotAtMaxHealth));
			_contextMenuUser.AddMenuItem(new ContextMenuItem("Hurt 10%", () => DecreaseHealth(0.1f), isEnabledFunc: IsNotAtMinHealth));
			_contextMenuUser.AddMenuItem(new ContextMenuItem("Heal 100%", () => IncreaseHealth(1), isEnabledFunc: IsNotAtMaxHealth));
			_contextMenuUser.AddMenuItem(new ContextMenuItem("Hurt 100%", () => DecreaseHealth(1), isEnabledFunc: IsNotAtMinHealth));
		}
		
		private void OnInteractionCallback(MonoBehaviour component)
		{
			if (!component.TryGetComponent(out Tile tile))
				return;

			Vector3 tilePosition = tile.Transform.position;
			Vector3 moveToPosition = new(tilePosition.x, Transform.position.y, tilePosition.z);
			StateMachine.MoveTo(moveToPosition);
			GlobalMessenger.Publish(new EndInteractionRequest());
		}

		private void IncreaseHealth(float percent01) => Health.IncreaseHealth(Health.MaxHealth * percent01);
		private void DecreaseHealth(float percent01) => Health.DecreaseHealth(Health.MaxHealth * percent01);

		private bool IsNotAtMaxHealth() => !Health.IsAtMaxHealth;
		private bool IsNotAtMinHealth() => !Health.IsAtMinHealth;

		private void OnCurrentHealthChanged(object sender, HealthChangedEventArgs args) => UpdateInteractableInfoText();
	}
}