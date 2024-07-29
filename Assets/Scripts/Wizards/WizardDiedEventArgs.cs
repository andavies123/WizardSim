using System;

namespace Wizards
{
	public class WizardDiedEventArgs : EventArgs
	{
		public WizardDiedEventArgs(Wizard deadWizard)
		{
			DeadWizard = deadWizard;
		}
		
		public Wizard DeadWizard { get; }
	}
}