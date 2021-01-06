using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Calculation
{
    public class Calculator
    {
        public long Calculate(string expression)
        {
            var tokens = GetTokens(ExpressionNormalizer.NormalizeExpression(expression));

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

        private string[] GetTokens(string expression)
        {
            var operators = string.Join(@"\", OperatorsProvider.ExtendedOperators);
            var splitPattern = $"([{operators}])";

            var tokens = Regex.Split(expression, splitPattern)
                              .Where(x => !string.IsNullOrEmpty(x))
                              .ToArray();

            return tokens;
        }
    }
}