using UI.ContextMenus;

namespace Wizards.ContextMenu
{
	public abstract class WizardContextMenuItem : ContextMenuItem
	{
		protected WizardContextMenuItem(Wizard wizard)
		{
			Wizard = wizard;
		}
		
		protected Wizard Wizard { get; }
	}
}