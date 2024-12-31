using AndysTools.GameWorldTimeManagement.Runtime;
using Extensions;
using GameWorld.Tiles;
using GeneralBehaviours.Utilities.ContextMenuBuilders;
using GeneralClasses.Health.HealthEventArgs;
using UnityEngine;
using GameWorld.Characters.Wizards.States;
using System;
using System.Collections.Generic;
using Game;
using GameWorld.Characters.Wizards.AgeSystem;
using GameWorld.Settlements;
using MessagingSystem;
using UI.Messages;
using Component = UnityEngine.Component;
using Random = UnityEngine.Random;

namespace GameWorld.Characters.Wizards
{
	[RequireComponent(typeof(WizardStateMachine))]
	[RequireComponent(typeof(WizardTaskHandler))]
	public class Wizard : Character
	{
		private MessageBroker _messageBroker;
		private GameWorldTimeBehaviour _worldTime;

		public string Name { get; set; }
		public IAge Age { get; } = new Age();
		public WizardType WizardType { get; set; } = WizardType.Undecided;
		public WizardAttributes Attributes { get; set; }
		public WizardStats WizardStats { get; private set; }
		public bool IsIdling => StateMachine.CurrentState is WizardIdleState;
		
		// Components
		public WizardStateMachine StateMachine { get; private set; }
		public WizardTaskHandler TaskHandler { get; private set; }
		
		// External References
		public Settlement Settlement { get; private set; }

		// Overrides
		public override CharacterStats CharacterStats { get; protected set; }
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
				Strength = { CurrentLevel = Random.Range(1, 4) },
				Endurance = { CurrentLevel = Random.Range(1, 4) },
				Vitality = { CurrentLevel = Random.Range(1, 4) },
				Magic = { CurrentLevel = Random.Range(1, 4) },
				Mana = { CurrentLevel = Random.Range(1, 4) },
				Intelligence = { CurrentLevel = Random.Range(1, 4) },
				Courage = { CurrentLevel = Random.Range(1, 4) }
			};

			CharacterStats = new CharacterStats(
				speedValueFormula: () => 5 + (Attributes.Endurance.CurrentLevel - 1) * 0.1f);
			
			WizardStats = new WizardStats(Attributes);

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
			Interactable.InfoText = new List<string>
			{
				$"{WizardType} Wizard",
				$"{Math.Floor(Age.Days * 10) / 10:0.#} days",
				$"{Health.CurrentHealth:0}/{Health.MaxHealth:0} ({Health.CurrentHealth.PercentageOf(Health.MaxHealth):0}%)"
			};
			Interactable.ExtendedInfoText = new List<string>
			{
				$"{StateMachine.CurrentStateDisplayName} - {StateMachine.CurrentStateDisplayStatus}",
				"-- Attributes --",
				$"Strength: {Attributes.Strength.CurrentLevel}",
				$"Endurance: {Attributes.Endurance.CurrentLevel}",
				$"Vitality: {Attributes.Vitality.CurrentLevel}",
				$"Magic: {Attributes.Magic.CurrentLevel}",
				$"Mana: {Attributes.Mana.CurrentLevel}",
				$"Intelligence: {Attributes.Intelligence.CurrentLevel}",
				$"Courage: {Attributes.Courage.CurrentLevel}"
			};
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