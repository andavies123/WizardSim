using System;
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
		private IWizardTask _currentTask;
		
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

		public bool IsAssignedTask { get; private set; }
		public bool CanBeAssignedTask { get; private set; } = true;

		public void AssignTask(IWizardTask task)
		{
			IsAssignedTask = true;
			task.WizardTaskState.SetWizard(this);
			task.Completed += OnCurrentTaskCompleted;
			StateMachine.OverrideCurrentState(task.WizardTaskState);
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

		private void OnCurrentTaskCompleted(object sender, EventArgs args)
		{
			if (sender is not IWizardTask wizardTask)
				return;

			IsAssignedTask = false;
			wizardTask.Completed -= OnCurrentTaskCompleted;
			
			// Go back to idling
			StateMachine.Idle();
		}

		private void IncreaseHealth(float percent01) => Health.IncreaseHealth(Health.MaxHealth * percent01);
		private void DecreaseHealth(float percent01) => Health.DecreaseHealth(Health.MaxHealth * percent01);

		private bool IsNotAtMaxHealth() => !Health.IsAtMaxHealth;
		private bool IsNotAtMinHealth() => !Health.IsAtMinHealth;

		private void OnCurrentHealthChanged(object sender, HealthChangedEventArgs args) => UpdateInteractableInfoText();
	}
}