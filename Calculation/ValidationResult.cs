namespace Calculation
{
    public struct ValidationResult
    {
        public bool IsSuccess { get; set; }
        public int ErrorPosition { get; set; }
        public string ErrorMessage { get; set; }
    }
}