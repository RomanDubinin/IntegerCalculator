using System.Linq;
using Calculation;
using NUnit.Framework;

namespace CalculatorUnitTests
{
    // test-time validation of operations consistency
    public class OperatorsProviderTests
    {
        [Test]
        public void PriorityValidityTest()
        {
            Assert.AreEqual(OperatorsProvider.ExtendedOperators, OperatorsProvider.Priority.Select(x => x.Key));
        }

        [Test]
        public void ActionsValidityTest()
        {
            Assert.AreEqual(OperatorsProvider.ExtendedOperators, OperatorsProvider.Actions.Select(x => x.Key));
        }

        [Test]
        public void UnaryMappingValidityTest()
        {
            Assert.IsFalse(OperatorsProvider.UnaryOperatorMapping.Keys.Except(OperatorsProvider.DefaultUnaryOperators).Any());
        }
    }
}