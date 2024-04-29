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

		private void Awake()
		{
			_wizard = GetComponent<Wizard>();
			
			AddMenuItem(new ContextMenuItem("Idle", () => _wizard.StateMachine.Idle(), isEnabledFunc: () => !_wizard.IsIdling));
			AddMenuItem(new ContextMenuItem("Move To", () => interactionEvents.RequestInteraction(_wizard, OnInteractionCallback)));
			AddMenuItem(new ContextMenuItem("Heal 10%", () => IncreaseHealth(0.1f), isEnabledFunc: IsNotAtMaxHealth));
			AddMenuItem(new ContextMenuItem("Hurt 10%", () => DecreaseHealth(0.1f), isEnabledFunc: IsNotAtMinHealth));
			AddMenuItem(new ContextMenuItem("Heal 100%", () => IncreaseHealth(1), isEnabledFunc: IsNotAtMaxHealth));
			AddMenuItem(new ContextMenuItem("Hurt 100%", () => DecreaseHealth(1), isEnabledFunc: IsNotAtMinHealth));
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

		private void IncreaseHealth(float percent01) => _wizard.Health.IncreaseHealth(_wizard.Health.MaxHealth * percent01);
		private void DecreaseHealth(float percent01) => _wizard.Health.DecreaseHealth(_wizard.Health.MaxHealth * percent01);

		private bool IsNotAtMaxHealth() => !_wizard.Health.IsAtMaxHealth;
		private bool IsNotAtMinHealth() => !_wizard.Health.IsAtMinHealth;
	}
}