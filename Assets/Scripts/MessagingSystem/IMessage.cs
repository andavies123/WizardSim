using System.Text;

namespace MessagingSystem
{
	public interface IMessage
	{
		object Sender { get; }
		StringBuilder GetDisplayText();
	}

	public interface IMessageKey
	{
		string CompareString { get; }
		object Sender { get; }
	}
}