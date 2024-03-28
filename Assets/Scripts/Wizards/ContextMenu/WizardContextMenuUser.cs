using UI.ContextMenus;
using UnityEngine;
using Wizards.ContextMenu.ContextMenuItems;

namespace Wizards.ContextMenu
{
	[RequireComponent(typeof(Wizard))]
	public class WizardContextMenuUser : ContextMenuUser<WizardContextMenuItem>
	{
		private Wizard _wizard;

		public override string MenuTitle => $"Wizard {_wizard.Name}";

		private void Awake()
		{
			_wizard = GetComponent<Wizard>();

			MenuItems.AddRange(new[]
			{
				new PrintWizardNameContextMenuItem(_wizard)
			});
		}
	}
}