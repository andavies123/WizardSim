using System;

namespace Game.Events
{
	public class GameRequest
	{
		public event EventHandler Requested;
		public void Request(object sender) => Requested?.Invoke(sender, EventArgs.Empty);
	}

	public class GameRequest<TEventArgs> where TEventArgs : GameEventArgs
	{
		public event EventHandler<TEventArgs> Requested;
		public void Request(object sender, TEventArgs requestArgs) => Requested?.Invoke(sender, requestArgs);
	}
}