using UI;
using UI.ContextMenus;
using UnityEngine;
using Wizards.ContextMenu.ContextMenuItems;

namespace Wizards.ContextMenu
{
	[RequireComponent(typeof(Wizard))]
	public class WizardContextMenuUser : ContextMenuUser<WizardContextMenuItem>
	{
		[SerializeField] private InteractionEvents interactionEvents;
		
		private Wizard _wizard;

		public override string MenuTitle => _wizard.Name;
		public override string InfoText { get; protected set; }

		private void Awake()
		{
			_wizard = GetComponent<Wizard>();

			MenuItems.AddRange(new WizardContextMenuItem[]
			{
				new PrintNameWizardContextMenuItem(_wizard),
				new IdleWizardContextMenuItem(_wizard),
				new MoveToWizardContextMenuItem(_wizard, interactionEvents)
			});
		}

		private void Update()
		{
			InfoText = _wizard.StateMachine.CurrentStateDisplayStatus;
		}
	}
}