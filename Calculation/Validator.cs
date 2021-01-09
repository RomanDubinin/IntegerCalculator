using System.Linq;
using System.Text.RegularExpressions;

namespace Calculation
{
    public class Validator
    {
        private readonly string unacceptableSymbolsPattern;
        private readonly string valuesWithoutOperationPattern;
        private readonly string operationsWithoutValuesPattern;

        public Validator()
        {
            var binaryOperations = string.Join(@"\", OperatorsProvider.DefaultBinaryOperators);
            var allOperations = string.Join(@"\", OperatorsProvider.DefaultBinaryOperators.Concat(OperatorsProvider.DefaultUnaryOperators));
            var onlyBinaryOperations = string.Join(@"\", OperatorsProvider.DefaultBinaryOperators.Except(OperatorsProvider.DefaultUnaryOperators));

            unacceptableSymbolsPattern = $@"[^\d {allOperations}]";
            valuesWithoutOperationPattern = @"\d( +)\d";
            operationsWithoutValuesPattern = $"(^ *[{onlyBinaryOperations}])|([{binaryOperations}] *[{onlyBinaryOperations}])|([{binaryOperations}] *[{binaryOperations}] *[{binaryOperations}])|([{binaryOperations}] *$)";

        }

        public ValidationResult GetIndexOfInvalidCharacter(string expression)
        {
            var unacceptableSymbolsMatch = Regex.Match(expression, unacceptableSymbolsPattern);
            var valuesWithoutOperationMatch = Regex.Match(expression, valuesWithoutOperationPattern);
            var operationsWithoutValuesMatch = Regex.Match(expression, operationsWithoutValuesPattern);

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