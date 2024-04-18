namespace Wizards.ContextMenu.ContextMenuItems
{
	public class HealWizardPercentageContextMenuItem : WizardContextMenuItem
	{
		private readonly float _healPercentage;
		
		public HealWizardPercentageContextMenuItem(Wizard wizard, float healPercentage) : base(wizard)
		{
			_healPercentage = healPercentage;
		}

		public override string MenuName => $"Heal {_healPercentage:#}%";

		protected override void OnMenuItemSelected()
		{
			Wizard.Health.IncreaseHealth(Wizard.Health.MaxHealth * _healPercentage / 100);
		}
	}
}