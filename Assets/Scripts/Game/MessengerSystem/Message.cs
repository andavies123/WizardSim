namespace Game.MessengerSystem
{
	public class Message : IMessage
	{
		public Message(object sender)
		{
			Sender = sender;
		}
		
		public object Sender { get; }
	}
}