namespace ShippingCalculator.Entities.Concrete
{
    public class ShippingCompany
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal LocalFactor { get; set; }
        public decimal CloseFactor { get; set; }
        public decimal ShortFactor { get; set; }
        public decimal MiddleFactor { get; set; }
        public decimal LongFactor { get; set; }
        public decimal Price { get; set; }
    }
}
