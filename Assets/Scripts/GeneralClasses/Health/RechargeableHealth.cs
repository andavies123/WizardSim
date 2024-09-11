using System;
using System.Timers;
using GeneralClasses.Health.HealthEventArgs;
using GeneralClasses.Timers.Interfaces;

namespace GeneralClasses.Health
{
	public class RechargeableHealth : Health
	{
		private readonly ITimer _startRechargingTimer;
		private readonly ITimer _rechargeTimer;
		
		public float HealthGainedPerInterval { get; }
		public int RechargeIntervalMSec { get; }
		public int TimeUntilRechargeStartsMSec { get; }
		
		public RechargeableHealth(ITimerFactory timerFactory, HealthProperties healthProperties) : base(healthProperties.MaxHealth)
		{
			_ = timerFactory ?? throw new ArgumentNullException(nameof(timerFactory));
			
			HealthGainedPerInterval = healthProperties.HealthGainedPerInterval;
			RechargeIntervalMSec = healthProperties.RechargeIntervalMSec;
			TimeUntilRechargeStartsMSec = healthProperties.TimeUntilRechargeStartsMSec;

			_startRechargingTimer = timerFactory.Create(TimeUntilRechargeStartsMSec, false);
			_rechargeTimer = timerFactory.Create(RechargeIntervalMSec, true);

			_startRechargingTimer.Elapsed += OnStartRechargingTimerElapsed;
			_rechargeTimer.Elapsed += OnRechargeTimerElapsed;
			CurrentHealthChanged += OnCurrentHealthChanged;
			ReachedMaxHealth += OnReachedMaxHealth;
		}

		private void OnStartRechargingTimerElapsed(object sender, ElapsedEventArgs args)
		{
			_startRechargingTimer.Stop();
			_rechargeTimer.Start();
		}

		private void OnRechargeTimerElapsed(object sender, ElapsedEventArgs args)
		{
			CurrentHealth += HealthGainedPerInterval;
		}

		private void OnCurrentHealthChanged(object sender, CurrentHealthChangedEventArgs args)
		{
			if (args.IsDecrease)
			{
				_startRechargingTimer.Restart();
			}
		}

		private void OnReachedMaxHealth(object sender, ReachedMaxHealthEventArgs args)
		{
			_startRechargingTimer.Stop();
			_rechargeTimer.Stop();
		}
	}
}