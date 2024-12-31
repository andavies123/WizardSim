using System.Collections.Generic;
using Extensions;
using GameWorld.Characters.Wizards;
using GeneralBehaviours.Utilities.ContextMenuBuilders;
using GeneralClasses.Health.HealthEventArgs;

namespace GameWorld.Characters.Enemies
{
	public class Enemy : Character
	{
		// Components
		public EnemyStateMachine StateMachine { get; private set; }

		// Overrides
		public override CharacterStats CharacterStats { get; protected set; }
		protected override string CharacterType => "Enemy";

		protected override void Awake()
		{
			base.Awake();

			StateMachine = GetComponent<EnemyStateMachine>();

			Health.CurrentHealthChanged += OnCurrentHealthChanged;

			CharacterStats = new CharacterStats(() => 4);
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