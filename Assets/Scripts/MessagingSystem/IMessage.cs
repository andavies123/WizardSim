namespace MessagingSystem
{
	public interface IMessage
	{
		object Sender { get; }
	}

	public interface IMessageKey
	{
		string CompareString { get; }
		object Sender { get; }
	}
}