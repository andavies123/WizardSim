using System;
using Wizards.Tasks;

namespace UI.TaskManagement.WizardEventArgs
{
	internal class WizardTaskUIEventArgs : EventArgs
	{
		public WizardTaskUIEventArgs(IWizardTask wizardTask)
		{
			WizardTask = wizardTask;
		}
		
		public IWizardTask WizardTask { get; }
	}
}