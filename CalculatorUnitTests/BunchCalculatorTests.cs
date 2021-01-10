using System.Linq;
using Calculation;
using NUnit.Framework;

namespace CalculatorUnitTests
{
    public class BunchCalculatorTests
    {
        private BunchCalculator bunchCalculator;

        [SetUp]
        public void Setup()
        {
            bunchCalculator = new BunchCalculator(new Calculator(new Validator(), new ExpressionNormalizer()));
        }

        [Test]
        public void FileCalculationTest()
        {
            var lines = new[] {"1+2", "1+", "999999999999999999999999999999999999999+1"};
            var expectedResults = new[] {"3", "Value missed at position 1", "1000000000000000000000000000000000000000"};
            var actualResults = bunchCalculator.Calculate(lines).ToArray();
            Assert.AreEqual(expectedResults, actualResults);
        }
    }
}