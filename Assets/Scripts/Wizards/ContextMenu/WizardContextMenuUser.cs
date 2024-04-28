using GameWorld.Tiles;
using UI;
using UI.ContextMenus;
using UnityEngine;

namespace Wizards.ContextMenu
{
	[RequireComponent(typeof(Wizard))]
	public class WizardContextMenuUser : ContextMenuUser
	{
		[SerializeField] private InteractionEvents interactionEvents;
		
		private Wizard _wizard;

		public override string MenuTitle => _wizard.Name;
		public override string InfoText { get; protected set; }

		private void Awake()
		{
			_wizard = GetComponent<Wizard>();

			MenuItems.AddRange(new ContextMenuItem[]
			{
				new("Idle", () => _wizard.StateMachine.Idle()),
				new("Move To", () => interactionEvents.RequestInteraction(_wizard, OnInteractionCallback)),
				new("Heal 10%", () => _wizard.Health.IncreaseHealth(_wizard.Health.MaxHealth * .1f)),
				new("Hurt 10%", () => _wizard.Health.DecreaseHealth(_wizard.Health.MaxHealth * .1f)),
				new("Heal 10%", () => _wizard.Health.IncreaseHealth(_wizard.Health.MaxHealth)),
				new("Hurt 10%", () => _wizard.Health.DecreaseHealth(_wizard.Health.MaxHealth))
			});
		}

		private void Update()
		{
			InfoText = _wizard.StateMachine.CurrentStateDisplayStatus;
		}
		
		private void OnInteractionCallback(MonoBehaviour component)
		{
			if (!component.TryGetComponent(out Tile tile))
				return;

			Vector3 tilePosition = tile.Transform.position;
			Vector3 moveToPosition = new(tilePosition.x, _wizard.Transform.position.y, tilePosition.z);
			_wizard.StateMachine.MoveTo(moveToPosition);
			interactionEvents.EndInteraction(_wizard);
		}
	}
}