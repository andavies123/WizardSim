using GameWorld.Characters.Wizards.Tasks;
using MessagingSystem;

namespace GameWorld.Characters.Wizards.Messages
{
	public class AddWizardTaskRequest : Message
	{
		public IWizardTask Task { get; set; }
	}
}