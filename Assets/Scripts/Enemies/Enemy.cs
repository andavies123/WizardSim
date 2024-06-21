using Extensions;
using Game.MessengerSystem;
using GameWorld;
using GameWorld.Tiles;
using GeneralBehaviours;
using GeneralClasses.Health;
using GeneralClasses.Health.HealthEventArgs;
using GeneralClasses.Health.Interfaces;
using Stats;
using UI;
using UI.ContextMenus;
using UI.Messages;
using UnityEngine;

namespace Enemies
{
	[RequireComponent(typeof(ContextMenuUser))]
	[RequireComponent(typeof(Interactable))]
	public class Enemy : Entity
	{
		[SerializeField] private EnemyStats stats;

		private Interactable _interactable;
		private ContextMenuUser _contextMenuUser;
		
		// Components
		public Transform Transform { get; private set; }
		public EnemyStateMachine StateMachine { get; private set; }
		public Movement Movement { get; private set; }
		
		// Systems
		public IHealth Health { get; private set; }
		
		public override string DisplayName => "Enemy";
		public override MovementStats MovementStats => Stats.MovementStats;
		public EnemyStats Stats => stats;
		
		private void Awake()
		{
			Transform = transform;
			StateMachine = GetComponent<EnemyStateMachine>();
			Movement = GetComponent<Movement>();
			_interactable = GetComponent<Interactable>();
			_contextMenuUser = GetComponent<ContextMenuUser>();
			Health = new Health(50);

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
		
		private void InitializeContextMenu()
		{
			_contextMenuUser.AddMenuItem(new ContextMenuItem("Idle", null, isEnabledFunc: ContextMenuItem.AlwaysFalse));
			_contextMenuUser.AddMenuItem(new ContextMenuItem("Move To", () => GlobalMessenger.Publish(new StartInteractionRequest(OnInteractionCallback))));
			_contextMenuUser.AddMenuItem(new ContextMenuItem("Heal 10%", () => IncreaseHealth(0.1f), isEnabledFunc: IsNotAtMaxHealth));
			_contextMenuUser.AddMenuItem(new ContextMenuItem("Hurt 10%", () => DecreaseHealth(0.1f), isEnabledFunc: IsNotAtMinHealth));
			_contextMenuUser.AddMenuItem(new ContextMenuItem("Heal 100%", () => IncreaseHealth(1), isEnabledFunc: IsNotAtMaxHealth));
			_contextMenuUser.AddMenuItem(new ContextMenuItem("Hurt 100%", () => DecreaseHealth(1), isEnabledFunc: IsNotAtMinHealth));
		}

		private void UpdateInteractableInfoText()
		{
			_interactable.TitleText = DisplayName;
			_interactable.InfoText = $"Enemy - {Health.CurrentHealth:0}/{Health.MaxHealth:0} ({Health.CurrentHealth.PercentageOf(Health.MaxHealth):0}%)";
		}

		private void OnCurrentHealthChanged(object sender, CurrentHealthChangedEventArgs args) => UpdateInteractableInfoText();
		
		private void IncreaseHealth(float percent01) => Health.CurrentHealth += Health.MaxHealth * percent01;
		private void DecreaseHealth(float percent01) => Health.CurrentHealth -= Health.MaxHealth * percent01;

		private bool IsNotAtMaxHealth() => !Health.IsAtMaxHealth;
		private bool IsNotAtMinHealth() => !Health.IsAtMinHealth;
		
		private void OnInteractionCallback(MonoBehaviour component)
		{
			print("Moving is not setup for Enemies");
			GlobalMessenger.Publish(new EndInteractionRequest());
			return;
			if (!component.TryGetComponent(out Tile tile))
				return;

			Vector3 tilePosition = tile.Transform.position;
			Vector3 moveToPosition = new(tilePosition.x, Transform.position.y, tilePosition.z);
			//Enemy.StateMachine.MoveTo(moveToPosition);
			GlobalMessenger.Publish(new EndInteractionRequest());
		}
	}
}