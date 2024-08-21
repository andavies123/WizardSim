using Game.MessengerSystem;
using Wizards.Tasks;

namespace Wizards.Messages
{
	public class AddWizardTaskRequest : Message
	{
		public AddWizardTaskRequest(object sender, IWizardTask task) : base(sender)
		{
			Task = task;
		}

		public IWizardTask Task { get; }
	}
}