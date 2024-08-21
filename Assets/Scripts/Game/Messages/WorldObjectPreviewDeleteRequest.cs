using Game.MessengerSystem;

namespace Game.Messages
{
	public class WorldObjectPreviewDeleteRequest : Message
	{
		public WorldObjectPreviewDeleteRequest(object sender) : base(sender) { }
	}
}