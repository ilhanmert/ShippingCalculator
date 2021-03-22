using ShippingCalculator.CommonLayer.Logger.Abstract;

namespace ShippingCalculator.CommonLayer.Logger.Concrete
{
    public class Logger : MyLogger
    {
        protected override string logpath
        {
            get => "Log";
            set => throw new System.NotImplementedException();
        }
    }
}
