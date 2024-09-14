using System;
using GameWorld.Characters.Wizards.Tasks;

namespace GameWorld.Characters.Wizards.TaskSystem
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