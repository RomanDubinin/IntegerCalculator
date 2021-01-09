using System.Linq;
using System.Text.RegularExpressions;

namespace Calculation
{
    public class ExpressionNormalizer
    {
        private readonly (Regex Regex, string Replacement)[] unaryNormalizationPatterns;

        public ExpressionNormalizer()
        {
            unaryNormalizationPatterns =
                OperatorsProvider.UnaryOperatorMapping
                                 .Keys
                                 .Select(x => (new Regex($@"([{string.Join(@"\", OperatorsProvider.DefaultBinaryOperators)}])(\{x})|(^)(\{x})"),
                                               $"$1{OperatorsProvider.UnaryOperatorMapping[x]}"))
                                 .ToArray();
        }


        public string NormalizeExpression(string expression)
        {
            expression = expression.Replace(" ", "");
            foreach (var (regex, replacement) in unaryNormalizationPatterns)
                expression = regex.Replace(expression, replacement);

            return expression;
        }
    }
}