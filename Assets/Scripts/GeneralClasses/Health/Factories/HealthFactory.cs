using GeneralClasses.Health.Interfaces;
using GeneralClasses.Timers.Interfaces;

namespace GeneralClasses.Health.Factories
{
	public class HealthFactory : IHealthFactory
	{
		private readonly ITimerFactory _timerFactory;

		public HealthFactory(ITimerFactory timerFactory)
		{
			_timerFactory = timerFactory;
		}
		
		public IHealth CreateHealth(HealthProperties healthProperties)
		{
			if (healthProperties.CanRechargeHealth)
				return new RechargeableHealth(_timerFactory, healthProperties);

			return new Health(healthProperties);
		}
	}
}