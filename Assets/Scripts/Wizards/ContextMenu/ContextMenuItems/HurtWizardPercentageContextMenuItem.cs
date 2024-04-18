namespace Wizards.ContextMenu.ContextMenuItems
{
	public class HurtWizardPercentageContextMenuItem : WizardContextMenuItem
	{
		private readonly float _hurtPercentage;
		
		public HurtWizardPercentageContextMenuItem(Wizard wizard, float hurtPercentage) : base(wizard)
		{
			_hurtPercentage = hurtPercentage;
		}

		public override string MenuName => $"Hurt {_hurtPercentage:#}%";

		protected override void OnMenuItemSelected()
		{
			Wizard.Health.DecreaseHealth(Wizard.Health.MaxHealth * _hurtPercentage / 100);
		}
	}
}