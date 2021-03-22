namespace ShippingCalculator.Entities.Concrete
{
    public class NPGResult
    {
        public bool IsSuccess { get; set; }
        public int RowsAffected { get; set; }
        public dynamic Data { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
