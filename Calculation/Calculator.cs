using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Calculation
{
    public class Calculator
    {
        private readonly Validator validator;
        private readonly ExpressionNormalizer normalizer;
        private readonly string splitPattern;

        public Calculator(Validator validator, ExpressionNormalizer normalizer)
        {
            this.validator = validator;
            this.normalizer = normalizer;
            splitPattern = $"([{string.Join(@"\", OperatorsProvider.ExtendedOperators)}])";
        }

        public CalculationResult CalculateFromString(string expression)
        {
            var validationResult = validator.GetIndexOfInvalidCharacter(expression);
            long? result = null;
            string calculationError = null;
            if (validationResult.IsSuccess)
                try
                {
                    var tokens = GetTokens(normalizer.NormalizeExpression(expression));
                    result = CalculateInternal(tokens);
                }
                catch (OverflowException)
                {
                    calculationError = "Too large numbers";
                }

            return new CalculationResult {Result = result, Validation = validationResult, CalculationError = calculationError};
        }

        //todo make CalculateFromStream for very long expressions

        private long CalculateInternal(IEnumerable<string> tokens)
        {
            var operators = new Stack<string>();
            var operands = new Stack<long>();

            var previousPriority = int.MaxValue;
            foreach (var token in tokens)
            {
                if (IsOperator(token))
                {
                    if (Priority(token) <= previousPriority)
                    {
                        CalculateTopPriority(operands, operators, Priority(token));
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
                    operands.Push(long.Parse(token));
                }
            }

            CalculateTopPriority(operands, operators, 0);

            return operands.Pop();
        }

        private void CalculateTopPriority(Stack<long> operands, Stack<string> operators, int currentPriority)
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
            return Regex.Split(expression, splitPattern)
                        .Where(x => !string.IsNullOrEmpty(x))
                        .ToArray();
        }
    }
}