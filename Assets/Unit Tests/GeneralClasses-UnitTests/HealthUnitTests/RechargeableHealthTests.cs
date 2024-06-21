using System;
using System.Timers;
using FluentAssertions;
using GeneralClasses.Health;
using GeneralClasses.Timers.Interfaces;
using NSubstitute;
using NUnit.Framework;
// ReSharper disable CompareOfFloatsByEqualityOperator

namespace Unit_Tests.GeneralClasses_UnitTests.HealthUnitTests
{
	[TestFixture]
	public class RechargeableHealthTests : HealthTests
	{
		#region Constructor Tests
		
		#region RechargeableHealth(ITimerFactory, HealthProperties) Tests

		[Test]
		public void Constructor_SetsLocalValues_FromHealthProperties()
		{
			// Arrange
			ITimerFactory timerFactory = MockTimerFactory;
			HealthProperties healthProperties = SampleHealthProperties;
			
			// Act
			RechargeableHealth rechargeableHealth = new(timerFactory, healthProperties);
			
			// Assert
			rechargeableHealth.HealthGainedPerInterval.Should().Be(healthProperties.HealthGainedPerInterval);
			rechargeableHealth.TimeUntilRechargeStartsMSec.Should().Be(healthProperties.TimeUntilRechargeStartsMSec);
			rechargeableHealth.RechargeIntervalMSec.Should().Be(healthProperties.RechargeIntervalMSec);
		}

		[Test]
		public void Constructor_CreatesTwoTimers_FromTimerFactory()
		{
			// Arrange
			ITimerFactory timerFactory = MockTimerFactory;
			HealthProperties healthProperties = SampleHealthProperties;
			
			// Act
			_ = new RechargeableHealth(timerFactory, healthProperties);
			
			// Assert
			timerFactory.Received(2).Create(Arg.Any<double>(), Arg.Any<bool>());
		}

		[Test]
		public void Constructor_CreatesStartRechargingTimer_WithCorrectValues()
		{
			// Arrange
			ITimerFactory timerFactory = MockTimerFactory;
			HealthProperties healthProperties = SampleHealthProperties;
			
			// Act
			_ = new RechargeableHealth(timerFactory, healthProperties);
			
			// Assert
			timerFactory.Received(1).Create(
				Arg.Is<double>(interval => interval == healthProperties.TimeUntilRechargeStartsMSec),
				Arg.Is<bool>(autoReset => autoReset == false));
		}

		[Test]
		public void Constructor_CreatesRechargeTimer_WithCorrectValues()
		{
			// Arrange
			ITimerFactory timerFactory = MockTimerFactory;
			HealthProperties healthProperties = SampleHealthProperties;
			
			// Act
			_ = new RechargeableHealth(timerFactory, healthProperties);
			
			// Assert
			timerFactory.Received(1).Create(
				Arg.Is<double>(interval => interval == healthProperties.RechargeIntervalMSec),
				Arg.Is<bool>(autoReset => autoReset == true));
		}
		
		#endregion
        
		#endregion

		#region Timer Tests

		#region Start Recharging Timer Tests

		[Test]
		public void StartRechargingTimer_Stops_AfterTimerHasElapsed()
		{
			// Arrange
			HealthProperties healthProperties = SampleHealthProperties;
			ITimer startRechargingTimer = MockTimer;
			ITimerFactory timerFactory = MockTimerFactory;
			timerFactory.Create(
				Arg.Is<double>(interval => interval == healthProperties.TimeUntilRechargeStartsMSec), 
				Arg.Is<bool>(autoReset => autoReset == false))
				.Returns(startRechargingTimer);
			_ = new RechargeableHealth(timerFactory, healthProperties);
			
			// Act
			startRechargingTimer.Elapsed += Raise.Event<ElapsedEventHandler>(null, null);

			// Assert
			startRechargingTimer.Received(1).Stop();
		}

		[Test]
		public void StartRechargingTimer_StartsRechargeTimer_AfterTimerHasElapsed()
		{
			// Arrange
			HealthProperties healthProperties = SampleHealthProperties;
			ITimer startRechargingTimer = MockTimer;
			ITimer rechargeTimer = MockTimer;
			ITimerFactory timerFactory = MockTimerFactory;
			timerFactory.Create(
					Arg.Is<double>(interval => interval == healthProperties.TimeUntilRechargeStartsMSec), 
					Arg.Is<bool>(autoReset => autoReset == false))
				.Returns(startRechargingTimer);
			timerFactory.Create(
					Arg.Is<double>(interval => interval == healthProperties.RechargeIntervalMSec),
					Arg.Is<bool>(autoReset => autoReset == true))
				.Returns(rechargeTimer);
			_ = new RechargeableHealth(timerFactory, healthProperties);
			
			// Act
			startRechargingTimer.Elapsed += Raise.Event<ElapsedEventHandler>(null, null);

			// Assert
			rechargeTimer.Received(1).Start();
		}

		[Test]
		public void StartRechargingTimer_Stops_WhenHealthReachesMaxHealth()
		{
			// Arrange
			HealthProperties healthProperties = SampleHealthProperties;
			ITimer startRechargingTimer = MockTimer;
			ITimerFactory timerFactory = MockTimerFactory;
			timerFactory.Create(
					Arg.Is<double>(interval => interval == healthProperties.TimeUntilRechargeStartsMSec),
					Arg.Is<bool>(autoReset => autoReset == false))
				.Returns(startRechargingTimer);
			RechargeableHealth rechargeableHealth = new(timerFactory, healthProperties);

			// Act
			rechargeableHealth.CurrentHealth--;
			rechargeableHealth.CurrentHealth = rechargeableHealth.MaxHealth;

			// Assert
			startRechargingTimer.Received(1).Stop();
		}

		[Test]
		public void StartRechargingTimer_Restarts_WhenHealthDecreases()
		{
			// Arrange
			HealthProperties healthProperties = SampleHealthProperties;
			ITimer startRechargingTimer = MockTimer;
			ITimerFactory timerFactory = MockTimerFactory;
			timerFactory.Create(
					Arg.Is<double>(interval => interval == healthProperties.TimeUntilRechargeStartsMSec),
					Arg.Is<bool>(autoReset => autoReset == false))
				.Returns(startRechargingTimer);
			RechargeableHealth rechargeableHealth = new(timerFactory, healthProperties);

			// Act
			rechargeableHealth.CurrentHealth--;

			// Assert
			startRechargingTimer.Received(1).Restart();
		}

		[Test]
		public void StartRechargingTimer_DoesNotRestart_WhenHealthIncreases()
		{
			// Arrange
			HealthProperties healthProperties = SampleHealthProperties;
			ITimer startRechargingTimer = MockTimer;
			ITimerFactory timerFactory = MockTimerFactory;
			timerFactory.Create(
					Arg.Is<double>(interval => interval == healthProperties.TimeUntilRechargeStartsMSec),
					Arg.Is<bool>(autoReset => autoReset == false))
				.Returns(startRechargingTimer);
			RechargeableHealth rechargeableHealth = new(timerFactory, healthProperties);

			// Act
			rechargeableHealth.CurrentHealth--;
			startRechargingTimer.ClearReceivedCalls();
			rechargeableHealth.CurrentHealth++;

			// Assert
			startRechargingTimer.DidNotReceive().Restart();
		}

		#endregion
		
		#region Recharge Timer Tests

		[Test]
		public void RechargeTimer_IncreasesCurrentHealth_AfterTimerHasElapsed()
		{
			// Arrange
			HealthProperties healthProperties = SampleHealthProperties;
			ITimer rechargeTimer = MockTimer;
			ITimerFactory timerFactory = MockTimerFactory;
			timerFactory.Create(
					Arg.Is<double>(interval => interval == healthProperties.RechargeIntervalMSec),
					Arg.Is<bool>(autoReset => autoReset == true))
				.Returns(rechargeTimer);
			RechargeableHealth rechargeableHealth = new(timerFactory, healthProperties) {CurrentHealth = 0}; // Health starts at 0

			// Act
			rechargeTimer.Elapsed += Raise.Event<ElapsedEventHandler>(null, null);

			// Assert
			rechargeableHealth.CurrentHealth.Should().Be(healthProperties.HealthGainedPerInterval);
		}

		[Test]
		public void RechargeTimer_Stops_WhenHealthReachesMaxHealth()
		{
			// Arrange
			HealthProperties healthProperties = SampleHealthProperties;
			ITimer rechargeTimer = MockTimer;
			ITimerFactory timerFactory = MockTimerFactory;
			timerFactory.Create(
					Arg.Is<double>(interval => interval == healthProperties.RechargeIntervalMSec),
					Arg.Is<bool>(autoReset => autoReset == true))
				.Returns(rechargeTimer);
			RechargeableHealth rechargeableHealth = new(timerFactory, healthProperties);

			// Act
			rechargeableHealth.CurrentHealth--;
			rechargeableHealth.CurrentHealth = rechargeableHealth.MaxHealth;

			// Assert
			rechargeTimer.Received(1).Stop();
		}
		
        #endregion
        
		#endregion
		
		#region Test Helpers

		private static ITimerFactory MockTimerFactory => Substitute.For<ITimerFactory>();
		private static ITimer MockTimer => Substitute.For<ITimer>();
		
		private static HealthProperties SampleHealthProperties => 
			new() 
			{
				HealthGainedPerInterval = 1,
				RechargeIntervalMSec = 1000,
				TimeUntilRechargeStartsMSec = 5000,
				MaxHealth = 50
			};

		#endregion
	}
}