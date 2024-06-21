using GeneralClasses.Health.Factories;
using GeneralClasses.Health.Interfaces;
using GeneralClasses.Timers;
using GeneralClasses.Timers.Interfaces;

namespace Game
{
	public static class GlobalFactories
	{
		public static ITimerFactory TimerFactory { get; }
		public static IHealthFactory HealthFactory { get; }

		static GlobalFactories()
		{
			TimerFactory = new TimerFactory();
			HealthFactory = new HealthFactory(TimerFactory);
		}
	}
}