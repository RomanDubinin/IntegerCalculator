using Calculation;
using NUnit.Framework;

namespace CalculatorUnitTests
{
    public class ValidatorTests
    {
        [Test]
        [TestCase("1+a", 2, "Invalid character a at position 2")]
        [TestCase("-2.1", 2, "Invalid character . at position 2")]
        [TestCase("-2 13", 2, "Operation missed at position 2")]
        [TestCase("2   13", 1, "Operation missed at position 1")]
        [TestCase("1+++1", 1, "Value missed at position 1")]
        [TestCase("1---1", 1, "Value missed at position 1")]
        [TestCase("1+*1", 1, "Value missed at position 1")]
        [TestCase("*1+1", 0, "Value missed at position 0")]
        [TestCase("1+1 /", 4, "Value missed at position 4")]
        [TestCase("1 *+1", null, null)]
        public void ValidationTest(string expression, int? expectedPosition, string expectedMessage)
        {
            var actual = Validator.GetIndexOfInvalidCharacter(expression);
            Assert.AreEqual((expectedPosition, expectedMessage), actual);
        }
    }
}