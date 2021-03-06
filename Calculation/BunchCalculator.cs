﻿using System.Collections.Generic;

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
                if (expression.Length == 0)
                    continue;

                var result = calculator.CalculateFromString(expression);

                if (!result.Validation.IsSuccess)
                    yield return result.Validation.ErrorMessage;
                else
                    yield return result.Result.ToString();
            }
        }
    }
}