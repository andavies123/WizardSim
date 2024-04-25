using GameWorld.Tiles;
using UI;
using UnityEngine;

namespace Wizards.ContextMenu.ContextMenuItems
{
	public class MoveToWizardContextMenuItem : WizardContextMenuItem
	{
		private readonly InteractionEvents _interactionEvents;

		public MoveToWizardContextMenuItem(Wizard wizard, InteractionEvents interactionEvents) : base(wizard)
		{
			_interactionEvents = interactionEvents;
		}

		public override string MenuName => "Move To";

		protected override void OnMenuItemSelected()
		{
			_interactionEvents.RequestInteraction(Wizard, OnInteraction);
		}

		private void OnInteraction(MonoBehaviour component)
		{
			if (!component.TryGetComponent(out Tile tile))
				return;

			Vector3 tilePosition = tile.Transform.position;
			Vector3 moveToPosition = new(tilePosition.x, Wizard.Transform.position.y, tilePosition.z);
			Wizard.StateMachine.MoveTo(moveToPosition);
			_interactionEvents.EndInteraction(Wizard);
		}
	}
}