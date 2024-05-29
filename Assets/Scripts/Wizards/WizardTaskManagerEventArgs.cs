using System;
using Wizards.Tasks;

namespace Wizards
{
	public class WizardTaskManagerEventArgs : EventArgs
	{
		public WizardTaskManagerEventArgs(IWizardTask wizardTask)
		{
			WizardTask = wizardTask;
		}
		
		public IWizardTask WizardTask { get; }
	}
}