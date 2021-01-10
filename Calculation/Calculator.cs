using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

namespace Calculation
{
    public class Calculator
    {
        private readonly Validator validator;
        private readonly ExpressionNormalizer normalizer;
        private readonly Regex splitRegex;

        private readonly Stack<string> operators;
        private readonly Stack<BigInteger> operands;

        public Calculator(Validator validator, ExpressionNormalizer normalizer)
        {
            this.validator = validator;
            this.normalizer = normalizer;
            splitRegex = new Regex($"([{string.Join(@"\", OperatorsProvider.ExtendedOperators)}])");

            operators = new Stack<string>();
            operands = new Stack<BigInteger>();
        }

        public CalculationResult CalculateFromString(string expression)
        {
            var validationResult = validator.GetIndexOfInvalidCharacter(expression);
            BigInteger? result = null;
            string calculationError = null;
            if (validationResult.IsSuccess)
            {
                var tokens = GetTokens(normalizer.NormalizeExpression(expression));
                result = CalculateInternal(tokens);
            }

            return new CalculationResult {Result = result, Validation = validationResult, CalculationError = calculationError};
        }

        //todo make CalculateFromStream for very long expressions

        private BigInteger CalculateInternal(IEnumerable<string> tokens)
        {
            operators.Clear();
            operands.Clear();

            var previousPriority = int.MaxValue;
            foreach (var token in tokens)
            {
                if (IsOperator(token))
                {
                    if (Priority(token) <= previousPriority)
                    {
                        CalculateTopPriority(Priority(token));
                        operators.Push(token);
                    }
                    else
                    {
                        operators.Push(token);
                    }

                    previousPriority = Priority(token);
                }
                else
                {
                    operands.Push(BigInteger.Parse(token));
                }
            }

            CalculateTopPriority(0);

            return operands.Pop();
        }

        private void CalculateTopPriority(int currentPriority)
        {
            while (operators.Count != 0 && currentPriority <= Priority(operators.Peek()))
            {
                var operation = OperatorsProvider.Actions[operators.Pop()];
                operation(operands);
            }
        }

        private int Priority(string @operator)
        {
            return OperatorsProvider.Priority[@operator];
        }

        private bool IsOperator(string token)
        {
            return OperatorsProvider.ExtendedOperators.Contains(token);
        }

        private IEnumerable<string> GetTokens(string expression)
        {
            return splitRegex.Split(expression)
                             .Where(x => !string.IsNullOrEmpty(x))
                             .ToArray();
        }
    }
}