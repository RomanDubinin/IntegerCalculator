using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Calculation
{
    public class ExpressionNormalizer
    {
        private readonly (string From, string To)[] unaryNormalizationPatterns;

        public ExpressionNormalizer()
        {
            unaryNormalizationPatterns =
                OperatorsProvider.UnaryOperatorMapping
                                 .Keys
                                 .Select(x => ($@"([{string.Join(@"\", OperatorsProvider.DefaultBinaryOperators)}])(\{x})|(^)(\{x})",
                                               $"$1{OperatorsProvider.UnaryOperatorMapping[x]}"))
                                 .ToArray();
        }


        public string NormalizeExpression(string expression)
        {
            expression = expression.Replace(" ", "");

            foreach (var unaryNormalizationPattern in unaryNormalizationPatterns)
            {
                expression = Regex.Replace(expression,
                    unaryNormalizationPattern.From,
                    unaryNormalizationPattern.To);
            }

            return expression;
        }
    }
}