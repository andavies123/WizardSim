using System;
using Extensions;
using FluentAssertions;
using NUnit.Framework;

namespace Extensions_UnitTests
{
	[TestFixture]
	public class FloatExtensionsTests
	{
		#region PercentageOf Tests

		[Test]
		[TestCase(0F, 10F, 0F)]
		[TestCase(1F, 10F, 10F)]
		[TestCase(10F, 10F, 100F)]
		public void PercentageOf_ReturnsCorrectValue(float value, float total, float expected)
		{
			// Act
			float percentage = value.PercentageOf(total);
			
			// Assert
			percentage.Should().Be(expected);
		}

		[Test]
		public void PercentageOf_ThrowsDivideByZeroException_WhenTotalIsZero()
		{
			// Act
			Action action = () => 10f.PercentageOf(0);
			
			// Assert
			action.Should().ThrowExactly<DivideByZeroException>();
		}

		#endregion

		#region PercentageOf01 Tests

		[Test]
		[TestCase(0F, 10F, 0F)]
		[TestCase(1F, 10F, .1F)]
		[TestCase(10F, 10F, 1F)]
		public void PercentageOf01_ReturnsCorrectValue(float value, float total, float expected)
		{
			// Act
			float percentage01 = value.PercentageOf01(total);
			
			// Assert
			percentage01.Should().Be(expected);
		}

		[Test]
		public void PercentageOf01_ThrowsDivideByZeroException_WhenTotalIsZero()
		{
			// Act
			Action action = () => 10f.PercentageOf01(0);
			
			// Assert
			action.Should().ThrowExactly<DivideByZeroException>();
		}

		#endregion
	}
}