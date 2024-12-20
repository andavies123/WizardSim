﻿using AndysTools.GameWorldTimeManagement.Runtime;
using Extensions;
using GameWorld.Tiles;
using GeneralBehaviours.Utilities.ContextMenuBuilders;
using GeneralClasses.Health.HealthEventArgs;
using Stats;
using UnityEngine;
using GameWorld.Characters.Wizards.States;
using System;
using Game;
using GameWorld.Characters.Wizards.AgeSystem;
using GameWorld.Settlements;
using MessagingSystem;
using UI.Messages;
using Component = UnityEngine.Component;

namespace GameWorld.Characters.Wizards
{
	[RequireComponent(typeof(WizardStateMachine))]
	[RequireComponent(typeof(WizardTaskHandler))]
	public class Wizard : Character
	{
		[SerializeField] private WizardStats stats;

		private MessageBroker _messageBroker;
		private GameWorldTimeBehaviour _worldTime;

		public string Name { get; set; }
		public IAge Age { get; } = new Age();
		public WizardType WizardType { get; set; } = WizardType.Undecided;
		public WizardAttributes Attributes { get; set; }
		public WizardStats Stats => stats;
		public bool IsIdling => StateMachine.CurrentState is WizardIdleState;
		
		// Components
		public WizardStateMachine StateMachine { get; private set; }
		public WizardTaskHandler TaskHandler { get; private set; }
		
		// External References
		public Settlement Settlement { get; private set; }

		// Overrides
		public override MovementStats MovementStats => Stats.MovementStats;
		protected override string CharacterType => "Wizard";
		
		public AgingStage AgeStage => Age.Years switch
		{
			<= 24 => AgingStage.Child,
			<= 49 => AgingStage.Teen,
			<= 999 => AgingStage.Adult,
			_ => AgingStage.Elderly
		};
		
		public void InitializeWizard(string wizardName, Settlement settlement, GameWorldTimeBehaviour worldTime)
		{
			Name = wizardName;
			Settlement = settlement;
			_worldTime = worldTime;

			Attributes = new WizardAttributes
			{
				Strength = { CurrentLevel = 1 },
				Endurance = { CurrentLevel = 1 },
				Vitality = { CurrentLevel = 1 },
				Magic = { CurrentLevel = 1 },
				Mana = { CurrentLevel = 1 },
				Intelligence = { CurrentLevel = 1 },
				Courage = { CurrentLevel = 1 }
			};

			Attributes.LeveledUp += OnAttributeLeveledUp;
			
			gameObject.name = $"Wizard - {Name} - {WizardType}";
		}

		private void OnAttributeLeveledUp(CharacterAttribute leveledUpAttribute)
		{
			// Todo: Recalculate stats
		}

		protected override void Awake()
		{
			base.Awake();

			Dependencies.Get<MessageBroker>();
			StateMachine = GetComponent<WizardStateMachine>();
			TaskHandler = GetComponent<WizardTaskHandler>();

			Health.CurrentHealthChanged += OnCurrentHealthChanged;
		}

		protected override void Start()
		{
			base.Start();

			UpdateInteractableInfoText();
			InitializeContextMenu();
			
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
				() => _messageBroker.PublishSingle(
					new StartInteractionRequest
					{
						Sender = this,
						InteractionCallback = OnInteractionCallback
					}),
				() => true,
				() => true);

			ContextMenuUser.AddMenuItem(
				ContextMenuBuilder.BuildPath("Kill"),
				() => Health.CurrentHealth -= Health.CurrentHealth,
				() => true,
				() => true);

			base.InitializeContextMenu();
		}
		
		private void OnInteractionCallback(Component component)
		{
			if (!component.TryGetComponent(out Tile tile))
				return;

			Vector3 tilePosition = tile.Transform.position;
			Vector3 moveToPosition = new(tilePosition.x, Transform.position.y, tilePosition.z);
			StateMachine.MoveTo(moveToPosition);
            
			_messageBroker.PublishSingle(new EndInteractionRequest {Sender = this});
		}

		private void OnCurrentHealthChanged(object sender, CurrentHealthChangedEventArgs args)
		{
			UpdateInteractableInfoText();
		}
	}
}