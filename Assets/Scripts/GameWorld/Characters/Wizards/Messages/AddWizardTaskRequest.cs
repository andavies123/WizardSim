using Game.MessengerSystem;
using GameWorld.Characters.Wizards.Tasks;

namespace GameWorld.Characters.Wizards.Messages
{
	public class AddWizardTaskRequest : Message
	{
		public IWizardTask Task { get; set; }
	}
}