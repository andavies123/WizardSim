using System;
using Wizards.Tasks;

namespace Wizards.TaskSystem
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