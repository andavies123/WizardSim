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
			_interactionEvents.RequestInteraction(OnInteraction);
		}

		private void OnInteraction(MonoBehaviour component)
		{
			if (component is not Tile tile)
				return;

			Wizard.StateMachine.MoveTo(tile.transform.position);
		}
	}
}