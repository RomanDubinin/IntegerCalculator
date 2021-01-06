using Calculation;
using NUnit.Framework;

namespace CalculatorUnitTests
{
    public class ExpressionNormalizerTests
    {
        [Test]
        [TestCase("1  +2", "1+2")]
        [TestCase("-2", "~2")]
        [TestCase("-2++13", "~2+13")]
        [TestCase("-1--1", "~1-~1")]
        public void NormalizationTest(string expression, string expected)
        {
            var actual = ExpressionNormalizer.NormalizeExpression(expression);
            Assert.AreEqual(expected, actual);
        }
    }
}