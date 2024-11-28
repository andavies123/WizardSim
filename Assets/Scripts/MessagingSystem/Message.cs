using System.Text;

namespace MessagingSystem
{
	public class Message : IMessage
	{
		public object Sender { get; set; }

		public virtual StringBuilder GetDisplayText()
		{
			StringBuilder stringBuilder = new();
			stringBuilder.AppendLine($"Sender: {Sender.GetType().Name}");
			return stringBuilder;
		}
	}
}