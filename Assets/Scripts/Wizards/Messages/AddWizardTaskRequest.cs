using Game.MessengerSystem;
using Wizards.Tasks;

namespace Wizards.Messages
{
	public class AddWizardTaskRequest : Message
	{
		public IWizardTask Task { get; set; }
	}
}