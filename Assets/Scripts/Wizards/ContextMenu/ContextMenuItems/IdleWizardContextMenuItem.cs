namespace Wizards.ContextMenu.ContextMenuItems
{
	public class IdleWizardContextMenuItem : WizardContextMenuItem
	{
		public IdleWizardContextMenuItem(Wizard wizard) : base(wizard) { }

		public override string MenuName => "Idle";

		protected override void OnMenuItemSelected()
		{
			Wizard.StateMachine.Idle();
		}
	}
}