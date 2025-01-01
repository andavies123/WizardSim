using System.Collections.Generic;
using Extensions;
using GeneralBehaviours.Utilities.ContextMenuBuilders;
using GeneralClasses.Health.HealthEventArgs;
using UnityEngine;

namespace GameWorld.Characters.Enemies
{
	public class Enemy : Character
	{
		// Components
		public EnemyStateMachine StateMachine { get; private set; }
		
		// Non-Components
		public EnemyAttributes Attributes { get; private set; }
		public EnemyStats EnemyStats { get; private set; }

		// Overrides
		public override CharacterStats CharacterStats { get; protected set; }
		protected override string CharacterType => "Enemy";

		protected override void Awake()
		{
			base.Awake();

			StateMachine = GetComponent<EnemyStateMachine>();

			Health.CurrentHealthChanged += OnCurrentHealthChanged;

			Attributes = new EnemyAttributes
			{
				Strength = { CurrentLevel = Random.Range(1, 4) },
				Endurance = { CurrentLevel = Random.Range(1, 4) },
				Vitality = { CurrentLevel = Random.Range(1, 4) },
				Magic = { CurrentLevel = Random.Range(1, 4) },
				Mana = { CurrentLevel = Random.Range(1, 4) }
			};
			CharacterStats = new CharacterStats(() => 4);
			EnemyStats = new EnemyStats(Attributes);
		}

		protected override void Start()
		{
			base.Start();

			UpdateInteractableInfoText();
			InitializeContextMenu();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			Health.CurrentHealthChanged -= OnCurrentHealthChanged;
		}

		protected override void InitializeContextMenu()
		{
			ContextMenuUser.AddMenuItem(
				ContextMenuBuilder.BuildPath("Action", "Idle"),
				null,
				() => false,
				() => true);

			base.InitializeContextMenu();
		}

		private void UpdateInteractableInfoText()
		{
			Interactable.TitleText = CharacterType;
			Interactable.InfoText = new List<string>
			{
				"Enemy",
				$"{Health.CurrentHealth:0}/{Health.MaxHealth:0} ({Health.CurrentHealth.PercentageOf(Health.MaxHealth):0}%)"
			};
		}

		private void OnCurrentHealthChanged(object sender, CurrentHealthChangedEventArgs args) => UpdateInteractableInfoText();
	}
}