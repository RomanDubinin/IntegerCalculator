using System.Linq;
using System.Text.RegularExpressions;

namespace Calculation
{
    public class Validator
    {
        private readonly Regex unacceptableSymbolsPatternRegex;
        private readonly Regex valuesWithoutOperationPatternRegex;
        private readonly Regex operationsWithoutValuesPatternRegex;

        public Validator()
        {
            var binaryOperations = string.Join(@"\", OperatorsProvider.DefaultBinaryOperators);
            var allOperations = string.Join(@"\", OperatorsProvider.DefaultBinaryOperators.Concat(OperatorsProvider.DefaultUnaryOperators));
            var onlyBinaryOperations = string.Join(@"\", OperatorsProvider.DefaultBinaryOperators.Except(OperatorsProvider.DefaultUnaryOperators));

            unacceptableSymbolsPatternRegex = new Regex($@"[^\d {allOperations}]");
            valuesWithoutOperationPatternRegex = new Regex(@"\d( +)\d");
            operationsWithoutValuesPatternRegex = new Regex($"(^ *[{onlyBinaryOperations}])|([{binaryOperations}] *[{onlyBinaryOperations}])|([{binaryOperations}] *[{binaryOperations}] *[{binaryOperations}])|([{binaryOperations}] *$)");
        }

        public ValidationResult GetIndexOfInvalidCharacter(string expression)
        {
            var unacceptableSymbolsMatch = unacceptableSymbolsPatternRegex.Match(expression);
            var valuesWithoutOperationMatch = valuesWithoutOperationPatternRegex.Match(expression);
            var operationsWithoutValuesMatch = operationsWithoutValuesPatternRegex.Match(expression);

            if (unacceptableSymbolsMatch.Success)
                return new ValidationResult
                {
                    ErrorPosition = unacceptableSymbolsMatch.Index,
                    ErrorMessage = $"Invalid character {unacceptableSymbolsMatch.Value} at position {unacceptableSymbolsMatch.Index}"
                };

            if (valuesWithoutOperationMatch.Success)
                return new ValidationResult
                {
                    ErrorPosition = valuesWithoutOperationMatch.Groups[1].Index,
                    ErrorMessage = $"Operation missed at position {valuesWithoutOperationMatch.Groups[1].Index}"
                };

            if (operationsWithoutValuesMatch.Success)
                return new ValidationResult
                {
                    ErrorPosition = operationsWithoutValuesMatch.Index,
                    ErrorMessage = $"Value missed at position {operationsWithoutValuesMatch.Index}"
                };

            return new ValidationResult {IsSuccess = true};
        }
    }
}