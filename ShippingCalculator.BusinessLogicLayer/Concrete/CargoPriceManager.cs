using ShippingCalculator.CommonLayer.Logger.Concrete;
using ShippingCalculator.Entities.Concrete;
using System;

namespace ShippingCalculator.BusinessLogicLayer.Concrete
{
    public class CargoPriceManager
    {
        private readonly Logger logger;
        public CargoPriceManager(Logger _logger)
        {
            logger = _logger;
        }
        /// <summary>
        /// Şehiriçi Mesafeler İçin Ücret Hesaplayan Fonksiyon
        /// </summary>
        /// <param name="desi">desi</param>
        /// <returns></returns>
        public decimal LocalPriceCalculator(int desi)
        {
            double result; // double tipinde result tanımlanıyor.
            result = Math.Pow(1.082, (desi - 1)); // girilen desiye işlem uygulanıyor ve result'a atanıyor.
            result = (result * 1.45) * 11.43; // işleme devam edilip yeni değer result'a atanıyor.
            return decimal.Round(Convert.ToDecimal(result), 2); // result döndürülüyor.
        }
        /// <summary>
        /// Yakın Mesafeler İçin Ücret Hesaplayan Fonksiyon
        /// </summary>
        /// <param name="desi">desi</param>
        /// <returns></returns>
        public decimal ClosePriceCalculator(int desi)
        {
            double result; // double tipinde result değişkeni tanımlanıyor.
            result = Math.Pow(1.082, (desi - 1)); // girilen desiye işlem uygulanıyor ve result'a atanıyor.
            result = (result * 2.15) * 11.43; // işleme devam edilip yeni değer result'a atanıyor.
            return decimal.Round(Convert.ToDecimal(result), 2); // result döndürülüyor.
        }
        /// <summary>
        /// Kısa Mesafeler İçin Ücret Hesaplayan Fonksiyon
        /// </summary>
        /// <param name="desi">desi</param>
        /// <returns></returns>
        public decimal ShortPriceCalculator(int desi)
        {
            double result; // double tipinde result değişkeni tanımlanıyor.
            result = Math.Pow(1.082, (desi - 1)); // girilen desiye işlem uygulanıyor ve result'a atanıyor.
            result = (result * 2.33) * 11.43; // işleme devam edilip yeni değer result'a atanıyor.
            return  decimal.Round(Convert.ToDecimal(result),2); // result döndürülüyor.
        }
        /// <summary>
        /// Orta Mesafeler İçin Ücret Hesaplayan Fonksiyon
        /// </summary>
        /// <param name="desi">desi</param>
        /// <returns></returns>
        public decimal MidlinePriceCalculator(int desi)
        {
            double result; // double tipinde result değişkeni oluşturuluyor.
            result = Math.Pow(1.082, (desi - 1)); // girilen desiye işlem uygulanıyor ve result'a atanıyor.
            result = (result * 2.56) * 11.43; // işleme devam edilip yeni değer result'a atanıyor.
            return decimal.Round(Convert.ToDecimal(result), 2); // result döndürülüyor.
        }
        /// <summary>
        /// Uzun Mesafeler İçin Ücret Hesaplayan Fonksiyon
        /// </summary>
        /// <param name="desi">desi</param>
        /// <returns></returns>
        public decimal LongPriceCalculator(int desi)
        {
            double result; // double tipinde result değişkeni oluşturuluyor.
            result = Math.Pow(1.082, (desi - 1)); // girilen desiye işlem uygulanıyor ve result'a atanıyor.
            result = (result * 2.82) * 11.43; // işleme devam edilip yeni değer result'a atanıyor.
            return decimal.Round(Convert.ToDecimal(result), 2); // result döndürülüyor.
        }
        /// <summary>
        /// Desi Hesaplayan Fonksiyon
        /// </summary>
        /// <param name="package">koli bilgileri</param>
        /// <returns></returns>
        public int DesiCalculator(Package package)
        {
            decimal desi; // decimal tipinde desi değişkeni tanımlanıyor.
            desi = (package.Height * package.Length * package.Width)/3000; // standartlara uygun desi hesaplanıyor.
            if (desi > package.Weight) // desi, koli ağırlığından fazla ise...
            {
                Math.Ceiling(desi);
                return Convert.ToInt32(desi); // decimal tipinde desi döndürülüyor.
            }
            else // koli ağırlığı, desiden büyük ise...
            {
                return Convert.ToInt32(package.Weight); // decimal tipinde koli ağırlık döndürülüyor.
            }
        }
    }
}
