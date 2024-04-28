using Extensions;
using GameWorld;
using GeneralBehaviours;
using GeneralBehaviours.Health;
using Stats;
using UI;
using UnityEngine;
using Utilities;
using Wizards.States;

namespace Wizards
{
	public class Wizard : Entity
	{
		[SerializeField] private WizardStats stats;

		private Interactable _interactable;
		
		public string Name { get; set; }
		
		public Transform Transform { get; private set; }
		public WizardStateMachine StateMachine { get; private set; }
		public Movement Movement { get; private set; }
		public Health Health { get; private set; }

		public override string DisplayName => Name;
		public override MovementStats MovementStats => Stats.MovementStats;
		public WizardStats Stats => stats;

		public bool IsIdling => StateMachine.CurrentState is WizardIdleState;
		
		private void Awake()
		{
			Transform = transform;
			StateMachine = GetComponent<WizardStateMachine>();
			Movement = GetComponent<Movement>();
			Health = GetComponent<Health>();
			_interactable = GetComponent<Interactable>();
			
			Name = NameGenerator.GetNewName();

			Health.CurrentHealthChanged += OnCurrentHealthChanged;
			
			gameObject.name = $"Wizard - {Name}";
		}

		private void Start()
		{
			if (_interactable)
			{
				_interactable.TitleText = Name;
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
				_interactable.InfoText = $"Wizard - {Health.CurrentHealth:0}/{Health.MaxHealth:0} ({Health.CurrentHealth.PercentageOf(Health.MaxHealth):0}%)";
		}

		private void OnCurrentHealthChanged(object sender, HealthChangedEventArgs args) => UpdateInteractableInfoText();
	}
}