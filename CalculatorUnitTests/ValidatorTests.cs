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
        public void ValidationInvalidTest(string expression, int expectedPosition, string expectedMessage)
        {
            var actual = Validator.GetIndexOfInvalidCharacter(expression);
            var expected = new ValidationResult
            {
                IsSuccess = false,
                ErrorPosition = expectedPosition,
                ErrorMessage = expectedMessage
            };
            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("1 *+1")]
        public void ValidationValidTest(string expression)
        {
            var actual = Validator.GetIndexOfInvalidCharacter(expression);
            var expected = new ValidationResult {IsSuccess = true};
            Assert.AreEqual(expected, actual);
        }
    }
}