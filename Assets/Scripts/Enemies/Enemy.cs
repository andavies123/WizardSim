using Extensions;
using GameWorld;
using GeneralBehaviours;
using GeneralBehaviours.Utilities.ContextMenuBuilders;
using GeneralClasses.Health.HealthEventArgs;
using Stats;
using UnityEngine;

namespace Enemies
{
	public class Enemy : Character
	{
		[SerializeField] private EnemyStats stats;
		
		// Components
		public EnemyStateMachine StateMachine { get; private set; }
		public Movement Movement { get; private set; }
		
		public EnemyStats Stats => stats;
		
		// Overrides
		public override MovementStats MovementStats => Stats.MovementStats;
		protected override string CharacterType => "Enemy";
		
		protected override void Awake()
		{
			base.Awake();
            
			StateMachine = GetComponent<EnemyStateMachine>();
			Movement = GetComponent<Movement>();

			Health.Health.CurrentHealthChanged += OnCurrentHealthChanged;
		}

		private void Start()
		{
			UpdateInteractableInfoText();
			InitializeContextMenu();
		}

		private void OnDestroy()
		{
			Health.Health.CurrentHealthChanged -= OnCurrentHealthChanged;
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
			Interactable.InfoText = $"Enemy - {Health.Health.CurrentHealth:0}/{Health.Health.MaxHealth:0} ({Health.Health.CurrentHealth.PercentageOf(Health.Health.MaxHealth):0}%)";
		}

		private void OnCurrentHealthChanged(object sender, CurrentHealthChangedEventArgs args) => UpdateInteractableInfoText();
	}
}