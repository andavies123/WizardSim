using GeneralClasses.Timers.Interfaces;

namespace GeneralClasses.Timers
{
	public class Timer : System.Timers.Timer, ITimer
	{
		public Timer(double interval) : base(interval) { }
		
		public void Restart()
		{
			if (Enabled)
				Stop();
			
			Start();
		}
	}
}