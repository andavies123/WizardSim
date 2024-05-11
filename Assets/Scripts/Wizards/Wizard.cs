using Extensions;
using Game.MessengerSystem;
using GameWorld;
using GameWorld.Tiles;
using GeneralBehaviours;
using GeneralBehaviours.Health;
using Stats;
using UI;
using UI.ContextMenus;
using UI.Messages;
using UnityEngine;
using Utilities;
using Wizards.States;

namespace Wizards
{
	[RequireComponent(typeof(Interactable))]
	[RequireComponent(typeof(ContextMenuUser))]
	public class Wizard : Entity
	{
		[SerializeField] private WizardStats stats;

		private Interactable _interactable;
		private ContextMenuUser _contextMenuUser;
		
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
			_contextMenuUser = GetComponent<ContextMenuUser>();
			
			Name = NameGenerator.GetNewName();

			Health.CurrentHealthChanged += OnCurrentHealthChanged;
			
			gameObject.name = $"Wizard - {Name}";
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