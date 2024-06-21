using GeneralClasses.Timers.Interfaces;

namespace GeneralClasses.Timers
{
	/// <summary>
	/// Basic implementation of <see cref="ITimerFactory"/>
	/// </summary>
	public class TimerFactory : ITimerFactory
	{
		public ITimer Create(double interval, bool autoReset)
		{
			return new Timer(interval)
			{
				AutoReset = autoReset
			};
		}
	}
}