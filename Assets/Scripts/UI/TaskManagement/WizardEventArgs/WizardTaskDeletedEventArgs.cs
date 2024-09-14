using GameWorld.Characters.Wizards.Tasks;

namespace UI.TaskManagement.WizardEventArgs
{
	internal class WizardTaskDeletedEventArgs
	{
		public WizardTaskDeletedEventArgs(IWizardTask deletedTask)
		{
			DeletedTask = deletedTask;
		}
		
		public IWizardTask DeletedTask { get; }
	}
}