using Game.MessengerSystem;
using Wizards.Tasks;

namespace Wizards.Messages
{
	public class AddWizardTaskRequest : IMessage
	{
		public AddWizardTaskRequest(IWizardTask task)
		{
			Task = task;
		}
		
		public IWizardTask Task { get; }
	}
}