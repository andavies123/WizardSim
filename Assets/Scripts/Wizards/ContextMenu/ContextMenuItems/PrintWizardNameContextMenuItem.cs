using UnityEngine;

namespace Wizards.ContextMenu.ContextMenuItems
{
	public class PrintWizardNameContextMenuItem : WizardContextMenuItem
	{
		public PrintWizardNameContextMenuItem(Wizard wizard) : base(wizard) { }
		
		public override string MenuName => "Print Name";

		protected override void OnMenuItemSelected()
		{
			Debug.Log($"Wizard: {Wizard.Name}");
		}
	}
}