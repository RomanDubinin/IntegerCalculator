using System.Numerics;

namespace Calculation
{
    public struct CalculationResult
    {
        public BigInteger? Result { get; set; }
        public ValidationResult Validation { get; set; }
    }
}