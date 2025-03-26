using System;

namespace Game
{
	public class GameEvent
	{
		public event EventHandler Raised;
		public void Raise(object sender) => Raised?.Invoke(sender, EventArgs.Empty);
	}
	
	public class GameEvent<TEventArgs> where TEventArgs : GameEventArgs
	{
		public event EventHandler<TEventArgs> Raised;
		public void Raise(object sender, TEventArgs args) => Raised?.Invoke(sender, args);
	}

	public abstract class GameEventArgs : EventArgs { }
}