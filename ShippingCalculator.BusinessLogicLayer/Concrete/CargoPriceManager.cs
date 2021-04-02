using ShippingCalculator.CommonLayer.Logger.Concrete;
using System;

namespace ShippingCalculator.BusinessLogicLayer.Concrete
{
    class CargoPriceManager
    {
        private readonly Logger logger;
        public CargoPriceManager(Logger _logger)
        {
            logger = _logger;
        }
        public double LocalPriceCalculator(int desi)
        {
            double result;
            result = Math.Pow(1.082, (desi - 1));
            result = (result * 1.45) * 11.43;
            return result;
        }
        public double ClosePriceCalculator(int desi)
        {
            double result;
            result = Math.Pow(1.082, (desi - 1));
            result = (result * 2.15) * 11.43;
            return result;
        }
        public double ShortPriceCalculator(int desi)
        {
            double result;
            result = Math.Pow(1.082, (desi - 1));
            result = (result * 2.33) * 11.43;
            return result;
        }
        public double MidlinePriceCalculator(int desi)
        {
            double result;
            result = Math.Pow(1.082, (desi - 1));
            result = (result * 2.56) * 11.43;
            return result;
        }
        public double LongPriceCalculator(int desi)
        {
            double result;
            result = Math.Pow(1.082, (desi - 1));
            result = (result * 2.82) * 11.43;
            return result;
        }
    }
}
