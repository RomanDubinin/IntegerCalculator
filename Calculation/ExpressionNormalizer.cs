using System;
using System.Text.RegularExpressions;

namespace Calculation
{
    public static class ExpressionNormalizer
    {
        private static readonly Func<string, string> getUnaryNormalizationPattern = x =>
            $@"([{string.Join(@"\", OperatorsProvider.DefaultBinaryOperators)}])(\{x})|(^)(\{x})";

        public static string NormalizeExpression(string expression)
        {
            var whitespacesDeleted = expression.Replace(" ", "");
            return NormalizeUnaryOperator(NormalizeUnaryOperator(whitespacesDeleted, "+"), "-");
        }

        private static string NormalizeUnaryOperator(string expression, string operationToReplace) =>
            Regex.Replace(expression,
                          getUnaryNormalizationPattern(operationToReplace),
                          $"$1{OperatorsProvider.UnaryOperatorMapping[operationToReplace]}");
    }
}