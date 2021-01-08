using System.Collections.Generic;

namespace Calculation
{
    public class BunchCalculator
    {
        private Calculator calculator;

        public BunchCalculator(Calculator calculator)
        {
            this.calculator = calculator;
        }

        public IEnumerable<string> Calculate(IEnumerable<string> expressions)
        {
            foreach (var expression in expressions)
            {
                var result = calculator.CalculateFromString(expression);

                if (!result.Validation.IsSuccess)
                    yield return result.Validation.ErrorMessage;
                else if (result.CalculationError != null)
                    yield return result.CalculationError;
                else
                    yield return result.Result.ToString();
            }
        }
    }
}