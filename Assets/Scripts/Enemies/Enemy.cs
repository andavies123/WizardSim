using Extensions;
using Game.MessengerSystem;
using GameWorld;
using GeneralBehaviours;
using GeneralClasses.Health.HealthEventArgs;
using Stats;
using UI.ContextMenus;
using UI.Messages;
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
			ContextMenuUser.AddMenuItem(new ContextMenuItem("Idle", null, isEnabledFunc: ContextMenuItem.AlwaysFalse));
			ContextMenuUser.AddMenuItem(new ContextMenuItem("Move To", () => GlobalMessenger.Publish(new StartInteractionRequest(OnInteractionCallback))));

			base.InitializeContextMenu();
		}

		private void UpdateInteractableInfoText()
		{
			Interactable.TitleText = CharacterType;
			Interactable.InfoText = $"Enemy - {Health.Health.CurrentHealth:0}/{Health.Health.MaxHealth:0} ({Health.Health.CurrentHealth.PercentageOf(Health.Health.MaxHealth):0}%)";
		}

		private void OnCurrentHealthChanged(object sender, CurrentHealthChangedEventArgs args) => UpdateInteractableInfoText();
		
		private void OnInteractionCallback(MonoBehaviour component)
		{
			print("Moving is not setup for Enemies");
			GlobalMessenger.Publish(new EndInteractionRequest());
			return;
			// if (!component.TryGetComponent(out Tile tile))
			// 	return;
			//
			// Vector3 tilePosition = tile.Transform.position;
			// Vector3 moveToPosition = new(tilePosition.x, Transform.position.y, tilePosition.z);
			// //Enemy.StateMachine.MoveTo(moveToPosition);
			// GlobalMessenger.Publish(new EndInteractionRequest());
		}
	}
}