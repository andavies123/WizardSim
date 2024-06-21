using FluentAssertions;
using GeneralClasses.Health;
using GeneralClasses.Health.HealthEventArgs;
using NUnit.Framework;
// ReSharper disable UseObjectOrCollectionInitializer

namespace Unit_Tests.GeneralClasses_UnitTests.HealthUnitTests
{
	[TestFixture]
	public class HealthTests
	{
		#region Constructor Tests

		#region Value Constructor

		[Test]
		public void Constructor_SetsMaxHealth_WithValuePassed()
		{
			// Arrange
			const float maxHealth = 100;
			
			// Act
			Health health = new(maxHealth);
			
			// Assert
			health.MaxHealth.Should().Be(maxHealth);
		}

		[Test]
		public void Constructor_SetsCurrentHealth_ToMaxHealthValuePassed()
		{
			// Arrange
			const float maxHealth = 100;
			
			// Act
			Health health = new(maxHealth);

			// Assert
			health.CurrentHealth.Should().Be(maxHealth);
		}

		#endregion

		#region Properties Constructor

		[Test]
		public void Constructor_SetsMaxHealth_WithValueFromProperties()
		{
			// Arrange
			HealthProperties properties = new()
			{
				MaxHealth = 100
			};
			
			// Act
			Health health = new(properties);
			
			// Assert
			health.MaxHealth.Should().Be(properties.MaxHealth);
		}

		[Test]
		public void Constructor_SetsCurrentHealth_ToMaxHealthFromProperties()
		{
			// Arrange
			HealthProperties properties = new()
			{
				MaxHealth = 100
			};
			
			// Act
			Health health = new(properties);
			
			// Assert
			health.CurrentHealth.Should().Be(properties.MaxHealth);
		}

		#endregion
		
		#endregion
		
		#region MaxHealth Tests

		#region Value Tests
		
		[Test]
		public void MaxHealth_ClampsToZero_WhenSetToAValueBelowZero()
		{
			// Arrange
			Health health = new(50);
			
			// Act
			health.MaxHealth = -1;
			
			// Assert
			health.MaxHealth.Should().Be(0);
		}
		
		#endregion

		#region MaxHealthChanged Event Tests

		[Test]
		public void MaxHealth_RaisesOnMaxHealthChanged_WhenValueIsChanged()
		{
			// Arrange
			bool wasEventRaised = false;
			Health health = new(50);
			health.MaxHealthChanged += (_,_) => wasEventRaised = true;
			
			// Act
			health.MaxHealth++;
			
			// Assert
			wasEventRaised.Should().BeTrue();
		}

		[Test]
		public void MaxHealth_DoesNotRaiseOnMaxHealthChanged_WhenSameValueIsSet()
		{
			// Arrange
			bool wasEventRaised = false;
			Health health = new(50);
			health.MaxHealthChanged += (_, _) => wasEventRaised = true;
			
			// Act
			wasEventRaised = false; // Reset the flag since the constructor initially raised the event
			health.MaxHealth = 50;
			
			// Assert
			wasEventRaised.Should().BeFalse();
		}

		[Test]
		public void MaxHealth_OnMaxHealthChangedEventArgs_ContainsProperValues()
		{
			// Arrange
			MaxHealthChangedEventArgs eventArgs = null;
			Health health = new(50);
			health.MaxHealthChanged += (_, args) => eventArgs = args;
			
			// Act
			health.MaxHealth = 100;
			
			// Assert
			eventArgs.PreviousMaxHealth.Should().Be(50);
			eventArgs.NewMaxHealth.Should().Be(100);
		}
		
		#endregion
        
		#endregion

		#region CurrentHealth Tests

		#region CurrentHealthChanged Event
		
		[Test]
		public void CurrentHealth_CurrentHealthChangedRaised_WhenValueChanges()
		{
			// Arrange
			Health health = new(50);
			bool eventRaised = false;
			health.CurrentHealthChanged += (_, _) => eventRaised = true;
			
			// Act
			health.CurrentHealth = 25;

			// Assert
			eventRaised.Should().BeTrue();
		}

		[Test]
		public void CurrentHealth_CurrentHealthChangedNotRaised_WhenValueDoesNotChange()
		{
			// Arrange
			Health health = new(50);
			bool eventRaised = false;
			health.CurrentHealthChanged += (_, _) => eventRaised = true;
			
			// Act
			health.CurrentHealth = health.CurrentHealth;

			// Assert
			eventRaised.Should().BeFalse();
		}
        
		[Test]
		public void CurrentHealth_CurrentHealthChangedEventArgs_ContainsProperValues()
		{
			// Arrange
			CurrentHealthChangedEventArgs eventArgs = null;
			Health health = new(50);
			health.CurrentHealthChanged += (_, args) => eventArgs = args;
			
			// Act
			health.CurrentHealth = 25;
			
			// Assert
			eventArgs.PreviousCurrentHealth.Should().Be(50);
			eventArgs.NewCurrentHealth.Should().Be(25);
		}

		#endregion
		
		#region ReachedMinHealth Event

		[Test]
		public void CurrentHealth_ReachedMinHealth_Raised_WhenCurrentHealthIsZero()
		{
			// Arrange
			Health health = new(50);
			bool eventRaised = false;
			health.ReachedMinHealth += (_, _) => eventRaised = true;
			
			// Act
			health.CurrentHealth = 0;
			
			// Assert
			eventRaised.Should().BeTrue();
		}

		[Test]
		public void CurrentHealth_ReachedMinHealth_NotRaised_WhenCurrentHealthDoesNotReachZero()
		{
			// Arrange
			Health health = new(50);
			bool eventRaised = false;
			health.ReachedMinHealth += (_, _) => eventRaised = true;
			
			// Act
			health.CurrentHealth--;
			
			// Assert
			eventRaised.Should().BeFalse();
		}
        
		[Test]
		public void CurrentHealth_ReachedMinHealthEventArgs_ContainsProperValues()
		{
			// Arrange
			Health health = new(50);
			ReachedMinHealthEventArgs eventArgs = null;
			health.ReachedMinHealth += (_, args) => eventArgs = args;
			
			// Act
			health.CurrentHealth = 0;
			
			// Assert
			eventArgs.CurrentHealth.Should().Be(0);
		}
		
		#endregion
		
		#region ReachedMaxHealth Event
		
		[Test]
		public void CurrentHealth_ReachedMaxHealth_Raised_WhenCurrentHealthReachesMaxHealth()
		{
			// Arrange
			Health health = new(50);
			bool eventRaised = false;
			health.CurrentHealth = 5;
			health.ReachedMaxHealth += (_, _) => eventRaised = true;
			
			// Act
			health.CurrentHealth = health.MaxHealth;
			
			// Assert
			eventRaised.Should().BeTrue();
		}

		[Test]
		public void CurrentHealth_ReachedMaxHealth_NotRaised_WhenCurrentHealthSetToValueOtherThanMaxHealth()
		{
			// Arrange
			Health health = new(50);
			bool eventRaised = false;
			health.ReachedMaxHealth += (_, _) => eventRaised = true;
			
			// Act
			health.CurrentHealth--;
			
			// Assert
			eventRaised.Should().BeFalse();
		}
        
		[Test]
		public void CurrentHealth_ReachedMaxHealthEventArgs_ContainsProperValues()
		{
			// Arrange
			Health health = new(50);
			health.CurrentHealth = 5;
			ReachedMaxHealthEventArgs eventArgs = null;
			health.ReachedMaxHealth += (_, args) => eventArgs = args;
			
			// Act
			health.CurrentHealth = health.MaxHealth;
			
			// Assert
			eventArgs.CurrentHealth.Should().Be(50);
		}
		
		#endregion
		
		#endregion

		#region IsAtMaxHealth Tests

		[Test]
		public void IsAtMaxHealth_ReturnsTrue_WhenCurrentHealthIsEqualToMaxHealth()
		{
			// Arrange
			Health health = new(50);

			// Act
			health.CurrentHealth = health.MaxHealth;

			// Assert
			health.IsAtMaxHealth.Should().BeTrue();
		}

		[Test]
		public void IsAtMaxHealth_ReturnsTrue_WhenCurrentHealthIsSetToValueGreaterThanMaxHealth()
		{
			// Arrange
			Health health = new(50);

			// Act
			health.CurrentHealth = health.MaxHealth + 10;

			// Assert
			health.IsAtMaxHealth.Should().BeTrue();
		}

		[Test]
		public void IsAtMaxHealth_ReturnsFalse_WhenCurrentHealthIsLessThanMaxHealth()
		{
			// Arrange
			Health health = new(50);

			// Act
			health.CurrentHealth = health.MaxHealth - 10;

			// Assert
			health.IsAtMaxHealth.Should().BeFalse();
		}

		#endregion

		#region IsAtMinHealth Tests

		[Test]
		public void IsAtMinHealth_ReturnsTrue_WhenCurrentHealthIsEqualToZero()
		{
			// Arrange
			Health health = new(50);
			
			// Act
			health.CurrentHealth = 0;
			
			// Assert
			health.IsAtMinHealth.Should().BeTrue();
		}

		[Test]
		public void IsAtMinHealth_ReturnsFalse_WhenCurrentHealthIsGreaterThanZero()
		{
			// Arrange
			Health health = new(50);
			
			// Act
			health.CurrentHealth = 10;
			
			// Assert
			health.IsAtMinHealth.Should().BeFalse();
		}

		#endregion
	}
}