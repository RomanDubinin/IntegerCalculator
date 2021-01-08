using Calculation;
using NUnit.Framework;

namespace CalculatorUnitTests
{
    public class CalculatorTests
    {
        private Calculator calculator;

        [SetUp]
        public void Setup()
        {
            calculator = new Calculator();
        }

        [Test]
        [TestCase("1+  2", 3)]
        [TestCase("+1+ 2", 3)]
        [TestCase("-2", -2)]
        [TestCase("-2+13", 11)]
        [TestCase("-1-1", -2)]
        [TestCase("-1- -1", 0)]
        [TestCase("2/3", 0)]
        [TestCase("1+-2", -1)]
        [TestCase("1+2*2", 5)]
        [TestCase("11+22", 33)]
        [TestCase("1+1+1*2*2*2*2+1+1+1+1", 22)]
        public void CalculationTest(string expression, long expectedValue)
        {
            var actualValue = calculator.CalculateFromString(expression).Result;
            Assert.AreEqual(expectedValue, actualValue);
        }
    }
}