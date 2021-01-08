namespace Calculation
{
    public struct CalculationResult
    {
        public long? Result { get; set; }
        public ValidationResult Validation { get; set; }
        public string CalculationError { get; set; }
    }
}