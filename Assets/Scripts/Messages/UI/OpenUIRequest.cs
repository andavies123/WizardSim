using Messages.UI.Enums;
using MessagingSystem;

namespace Messages.UI
{
	public class OpenUIRequest : IMessage
	{
		public object Sender { get; set; }
		public UIWindow Window { get; set; }
	}
}