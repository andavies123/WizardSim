using Extensions;
using GameWorld;
using GeneralBehaviours;
using GeneralBehaviours.Health;
using Stats;
using UI;
using UnityEngine;

namespace Enemies
{
	public class Enemy : Entity
	{
		[SerializeField] private EnemyStats stats;

		private Interactable _interactable;
		
		public Transform Transform { get; private set; }
		public EnemyStateMachine StateMachine { get; private set; }
		public Movement Movement { get; private set; }
		public Health Health { get; private set; }
		
		public override string DisplayName => "Enemy";
		public override MovementStats MovementStats => Stats.MovementStats;
		public EnemyStats Stats => stats;
		
		private void Awake()
		{
			Transform = transform;
			StateMachine = GetComponent<EnemyStateMachine>();
			Movement = GetComponent<Movement>();
			Health = GetComponent<Health>();
			_interactable = GetComponent<Interactable>();

			Health.CurrentHealthChanged += OnCurrentHealthChanged;
		}

		private void Start()
		{
			if (_interactable)
			{
				_interactable.TitleText = DisplayName;
				UpdateInteractableInfoText();
			}
		}

		private void OnDestroy()
		{
			Health.CurrentHealthChanged -= OnCurrentHealthChanged;
		}

		private void UpdateInteractableInfoText()
		{
			if (_interactable)
				_interactable.InfoText = $"Enemy - {Health.CurrentHealth:0}/{Health.MaxHealth:0} ({Health.CurrentHealth.PercentageOf(Health.MaxHealth):0}%)";
		}

		private void OnCurrentHealthChanged(object sender, HealthChangedEventArgs args) => UpdateInteractableInfoText();
	}
}