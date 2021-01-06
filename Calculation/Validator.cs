using System.Linq;
using System.Text.RegularExpressions;

namespace Calculation
{
    public static class Validator
    {
        public static (int? Position, string Message) GetIndexOfInvalidCharacter(string expression)
        {
            var binaryOperations = string.Join(@"\", OperatorsProvider.DefaultBinaryOperators);
            var allOperations = string.Join(@"\", OperatorsProvider.DefaultBinaryOperators.Concat(OperatorsProvider.DefaultUnaryOperators));
            var onlyBinaryOperations = string.Join(@"\", OperatorsProvider.DefaultBinaryOperators.Except(OperatorsProvider.DefaultUnaryOperators));

            var unacceptableSymbolsPattern = $@"[^\d {allOperations}]";
            var valuesWithoutOperationPattern = @"\d( +)\d";
            var operationsWithoutValuesPattern = $"(^ *[{onlyBinaryOperations}])|([{binaryOperations}] *[{onlyBinaryOperations}])|([{binaryOperations}] *[{binaryOperations}] *[{binaryOperations}])|([{binaryOperations}] *$)";

            var unacceptableSymbolsMatch = Regex.Match(expression, unacceptableSymbolsPattern);
            var valuesWithoutOperationMatch = Regex.Match(expression, valuesWithoutOperationPattern);
            var operationsWithoutValuesMatch = Regex.Match(expression, operationsWithoutValuesPattern);

            if (unacceptableSymbolsMatch.Success)
                return (unacceptableSymbolsMatch.Index, $"Invalid character {unacceptableSymbolsMatch.Value} at position {unacceptableSymbolsMatch.Index}");

            if (valuesWithoutOperationMatch.Success)
                return (valuesWithoutOperationMatch.Groups[1].Index, $"Operation missed at position {valuesWithoutOperationMatch.Groups[1].Index}");

            if (operationsWithoutValuesMatch.Success)
                return (operationsWithoutValuesMatch.Index, $"Value missed at position {operationsWithoutValuesMatch.Index}");

            return (null, null);
        }
    }
}