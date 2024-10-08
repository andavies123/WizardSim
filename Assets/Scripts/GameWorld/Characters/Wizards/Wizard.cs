﻿using System.Collections.Generic;
using AndysTools.GameWorldTimeManagement.Runtime;
using Extensions;
using Game.MessengerSystem;
using GameWorld.Tiles;
using GeneralBehaviours.Utilities.ContextMenuBuilders;
using GeneralClasses.Health.HealthEventArgs;
using Stats;
using TaskSystem.Interfaces;
using UI.Messages;
using UnityEngine;
using GameWorld.Characters.Wizards.States;
using GameWorld.Characters.Wizards.Tasks;
using System;
using GameWorld.Characters.Wizards.AgeSystem;

namespace GameWorld.Characters.Wizards
{
	public class Wizard : Character, ITaskUser<IWizardTask>
	{
		[SerializeField] private WizardStats stats;
		
		private GameWorldTimeBehaviour _worldTime;

		public string Name { get; set; }
		public WizardType WizardType { get; set; } = WizardType.Earth;
		
		// Components
		public WizardStateMachine StateMachine { get; private set; }

		public IAge Age { get; } = new Age();
		public WizardStats Stats => stats;
		public bool IsIdling => StateMachine.CurrentState is WizardIdleState;

		// Overrides
		public override MovementStats MovementStats => Stats.MovementStats;
		protected override string CharacterType => "Wizard";

		#region ITaskUser Implementation

		private readonly List<IWizardTask> _assignedTasks = new();

		public bool IsAssignedTask => CurrentTask != null;
		public bool CanBeAssignedTask => !IsAssignedTask;
		public IWizardTask CurrentTask { get; private set; }
		public IReadOnlyList<IWizardTask> AssignedTasks => _assignedTasks;
		
		public AgingStage AgeStage => Age.Years switch
		{
			<= 24 => AgingStage.Child,
			<= 49 => AgingStage.Teen,
			<= 999 => AgingStage.Adult,
			_ => AgingStage.Elderly
		};

		public void AssignTask(IWizardTask task)
		{
			if (task == null)
				return;
			
			task.AssignedWizard = this;
			_assignedTasks.Add(task);
			task.WizardTaskState.SetWizard(this);
			
			AssignNewCurrentTask();
		}

		public void RemoveTask(IWizardTask task)
		{
			if (CurrentTask == null && _assignedTasks.IsEmpty())
				return; // No task to un-assign
			
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
		
		public void InitializeWizard(string wizardName, WizardType wizardType, GameWorldTimeBehaviour worldTime)
		{
			Name = wizardName;
			WizardType = wizardType;
			_worldTime = worldTime;
			
			gameObject.name = $"Wizard - {Name} - {wizardType}";
		}
		
		protected override void Awake()
		{
			base.Awake();

			// Add/get components
			StateMachine = GetComponent<WizardStateMachine>();

			Health.CurrentHealthChanged += OnCurrentHealthChanged;
		}

		protected override void Start()
		{
			base.Start();

			UpdateInteractableInfoText();
			InitializeContextMenu();
			
			if (!IsAssignedTask)
				StateMachine.Idle();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			Health.CurrentHealthChanged -= OnCurrentHealthChanged;
		}

		protected override void Update()
		{
			base.Update();

			Age.IncreaseAge(_worldTime.DeltaTime);
			UpdateInteractableInfoText();
		}

		private void UpdateInteractableInfoText()
		{
			Interactable.TitleText = Name;
			string ageInDays = $"{Math.Floor(Age.Days * 10) / 10:0.#}";
			Interactable.InfoText = $"{ageInDays} days - {Health.CurrentHealth:0}/{Health.MaxHealth:0} ({Health.CurrentHealth.PercentageOf(Health.MaxHealth):0}%)";
		}

		protected override void InitializeContextMenu()
		{
			ContextMenuUser.AddMenuItem(
				ContextMenuBuilder.BuildPath("Action", "Idle"), 
				() => StateMachine.Idle(), 
				() => !IsIdling,
				() => true);
			
			ContextMenuUser.AddMenuItem(
				ContextMenuBuilder.BuildPath("Action", "Move To"),
				() => GlobalMessenger.Publish(new StartInteractionRequest{Sender = this, InteractionCallback = OnInteractionCallback}),
				() => true,
				() => true);

			ContextMenuUser.AddMenuItem(
				ContextMenuBuilder.BuildPath("Kill"),
				() => Health.CurrentHealth -= Health.CurrentHealth,
				() => true,
				() => true);

			base.InitializeContextMenu();
		}
		
		private void OnInteractionCallback(MonoBehaviour component)
		{
			if (!component.TryGetComponent(out Tile tile))
				return;

			Vector3 tilePosition = tile.Transform.position;
			Vector3 moveToPosition = new(tilePosition.x, Transform.position.y, tilePosition.z);
			StateMachine.MoveTo(moveToPosition);
            
			GlobalMessenger.Publish(new EndInteractionRequest {Sender = this});
		}
		
		private void OnCurrentHealthChanged(object sender, CurrentHealthChangedEventArgs args) => UpdateInteractableInfoText();
	}
}