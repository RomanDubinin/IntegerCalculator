using Calculation;
using NUnit.Framework;

namespace CalculatorUnitTests
{
    public class ExpressionNormalizerTests
    {
        private ExpressionNormalizer normalizer;

        [SetUp]
        public void SetUp()
        {
            normalizer = new ExpressionNormalizer();
        }

        [Test]
        [TestCase("1  +2", "1+2")]
        [TestCase("-2", "~2")]
        [TestCase("-2++13", "~2+13")]
        [TestCase("-1--1", "~1-~1")]
        public void NormalizationTest(string expression, string expected)
        {
            var actual = normalizer.NormalizeExpression(expression);
            Assert.AreEqual(expected, actual);
        }
    }
}