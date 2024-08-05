using AndysTools.GameWorldTimeManagement.Runtime;
using NUnit.Framework;

namespace AndysTools.GameWorldTimeManagement.Tests.Runtime
{
	public class GameWorldTimeTests
	{
		#region SetCurrentTime(day, hour, minute, second) Tests

		[Test]
		public void SetCurrentTime_IndividualTimes_SetsAllTimesAccurately()
		{
			// Arrange
			GameWorldTime gameWorldTime = new(100);
			const int day = 20;
			const int hour = 13;
			const int minute = 42;
			const float second = 23.54f;
			
			// Act
			gameWorldTime.SetCurrentTime(day, hour, minute, second);
			
			// Assert
			Assert.AreEqual(gameWorldTime.Days, day);
			Assert.AreEqual(gameWorldTime.Hours, hour);
			Assert.AreEqual(gameWorldTime.Minutes, minute);
			Assert.AreEqual(gameWorldTime.Seconds, second);
		}

		[Test]
		public void SetCurrentTime_IndividualTimes_SetsDaysToZeroWhenPassingNegativeValue()
		{
			// Arrange
			GameWorldTime gameWorldTime = new(100);
			const int day = -1;
			const int hour = 10;
			const int minute = 24;
			const float second = 32.34f;
			
			// Act
			gameWorldTime.SetCurrentTime(day, hour, minute, second);
			
			// Assert
			Assert.AreEqual(gameWorldTime.Days, 0);
		}

		[Test]
		public void SetCurrentTime_IndividualTimes_SetsHoursToZeroWhenPassingNegativeValue()
		{
			// Arrange
			GameWorldTime gameWorldTime = new(100);
			const int day = 34;
			const int hour = -1;
			const int minute = 24;
			const float second = 32.34f;
			
			// Act
			gameWorldTime.SetCurrentTime(day, hour, minute, second);
			
			// Assert
			Assert.AreEqual(gameWorldTime.Hours, 0);
		}

		[Test]
		public void SetCurrentTime_IndividualTimes_SetsMinutesToZeroWhenPassingNegativeValue()
		{
			// Arrange
			GameWorldTime gameWorldTime = new(100);
			const int day = 34;
			const int hour = 10;
			const int minute = -1;
			const float second = 32.34f;
			
			// Act
			gameWorldTime.SetCurrentTime(day, hour, minute, second);
			
			// Assert
			Assert.AreEqual(gameWorldTime.Minutes, 0);
		}

		[Test]
		public void SetCurrentTime_IndividualTimes_SetsSecondsToZeroWhenPassingNegativeValue()
		{
			// Arrange
			GameWorldTime gameWorldTime = new(100);
			const int day = 34;
			const int hour = 10;
			const int minute = 24;
			const float second = -1.458f;
			
			// Act
			gameWorldTime.SetCurrentTime(day, hour, minute, second);
			
			// Assert
			Assert.AreEqual(gameWorldTime.Seconds, 0);
		}

		#endregion
		
		#region SetCurrentTime(totalGameWorldSeconds) Tests
		
		[Test]
		public void SetCurrentTime_OnlySeconds_SetsAllTimesAccurately()
		{
			// Arrange
			GameWorldTime gameWorldTime = new(100);
			const float totalGameWorldSeconds = RealWorldTimeConstants.SECONDS_PER_DAY * 2 - 0.5f;
			
			// Act
			gameWorldTime.SetCurrentTime(totalGameWorldSeconds);
			
			// Assert
			Assert.AreEqual(gameWorldTime.Days, 1);
			Assert.AreEqual(gameWorldTime.Hours, 23);
			Assert.AreEqual(gameWorldTime.Minutes, 59);
			Assert.AreEqual(gameWorldTime.Seconds, 59.5f);
		}

		[Test]
		public void SetCurrentTime_OnlySeconds_SetsAllTimesToZero_WhenPassingANegativeValue()
		{
			// Arrange
			GameWorldTime gameWorldTime = new(100);
			
			// Act
			gameWorldTime.SetCurrentTime(-1);
			
			// Assert
			Assert.AreEqual(gameWorldTime.Days, 0);
			Assert.AreEqual(gameWorldTime.Hours, 0);
			Assert.AreEqual(gameWorldTime.Minutes, 0);
			Assert.AreEqual(gameWorldTime.Seconds, 0);
		}
		
		#endregion
		
		#region AdvanceTime Tests
		
		[Test]
		public void AdvanceTime_UpdatesDeltaTimeValue()
		{
            // Arrange
            GameWorldTime gameWorldTime = new(43200);
            
            // Act
            gameWorldTime.AdvanceTime(1f);
            
            // Assert
            Assert.AreEqual(gameWorldTime.DeltaTime, 2f);
		}

		[Test]
		public void AdvanceTime_IncreasesTotalSecondsValue()
		{
			// Arrange
			GameWorldTime gameWorldTime = new(43200);
			
			// Act
			gameWorldTime.AdvanceTime(1f);
			gameWorldTime.AdvanceTime(2f);
			
			// Assert
			Assert.AreEqual(gameWorldTime.TotalSeconds, 6f);
		}

		[Test]
		public void AdvanceTime_IncreasesGameWorldTimeValues()
		{
			// Arrange
			GameWorldTime gameWorldTime = new(43200);
			
			// Act
			gameWorldTime.AdvanceTime(24);
			
			// Assert
			Assert.AreEqual(gameWorldTime.Seconds, 48);
		}
		
		#endregion

		#region Time Property Overflow Tests

		[Test]
		public void TimePropertyOverflow_MinutesIncreases_WhenSecondsOverflow()
		{
			// Arrange
			GameWorldTime gameWorldTime = new(RealWorldTimeConstants.SECONDS_PER_DAY);
			
			// Act
			gameWorldTime.AdvanceTime(RealWorldTimeConstants.SECONDS_PER_MINUTE);

			// Assert
			Assert.AreEqual(gameWorldTime.Days, 0);
			Assert.AreEqual(gameWorldTime.Hours, 0);
			Assert.AreEqual(gameWorldTime.Minutes, 1);
			Assert.AreEqual(gameWorldTime.Seconds, 0);
		}

		[Test]
		public void TimeProperty_HoursIncreases_WhenMinutesOverflow()
		{
			// Arrange
			GameWorldTime gameWorldTime = new(RealWorldTimeConstants.SECONDS_PER_DAY);
			const int secondsPerHour = RealWorldTimeConstants.SECONDS_PER_MINUTE * RealWorldTimeConstants.MINUTES_PER_HOUR;
			
			// Act
			gameWorldTime.AdvanceTime(secondsPerHour);
			
			// Assert
			Assert.AreEqual(gameWorldTime.Days, 0);
			Assert.AreEqual(gameWorldTime.Hours, 1);
			Assert.AreEqual(gameWorldTime.Minutes, 0);
			Assert.AreEqual(gameWorldTime.Seconds, 0);
		}

		[Test]
		public void TimeProperty_DaysIncreases_WhenHoursOverflow()
		{
			// Arrange
			GameWorldTime gameWorldTime = new(RealWorldTimeConstants.SECONDS_PER_DAY);
			
			// Act
			gameWorldTime.AdvanceTime(RealWorldTimeConstants.SECONDS_PER_DAY);
			
			// Assert
			Assert.AreEqual(gameWorldTime.Days, 1);
			Assert.AreEqual(gameWorldTime.Hours, 0);
			Assert.AreEqual(gameWorldTime.Minutes, 0);
			Assert.AreEqual(gameWorldTime.Seconds, 0);
		}

		#endregion

		#region TimeMultiplier Tests

		[Test]
		public void TimeMultiplier_CorrectlyAdjustsTime_WhenAdvancingTime()
		{
			// Arrange
			GameWorldTime gameWorldTime = new(RealWorldTimeConstants.SECONDS_PER_DAY)
			{
				TimeMultiplier = 2f
			};

			// Act
			gameWorldTime.AdvanceTime(10);
			
			// Assert
			Assert.AreEqual(gameWorldTime.DeltaTime, 20);
		}

		#endregion
	}
}