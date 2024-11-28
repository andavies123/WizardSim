using Messages.UI.Enums;
using MessagingSystem;
using System.Text;

namespace Messages.UI
{
	public class OpenUIRequest : Message
	{
		public UIWindow Window { get; set; }

		public override StringBuilder GetDisplayText()
		{
			StringBuilder stringBuilder = base.GetDisplayText();
			stringBuilder.AppendLine($"Window: {Window}");
			return stringBuilder;
		}
	}
}