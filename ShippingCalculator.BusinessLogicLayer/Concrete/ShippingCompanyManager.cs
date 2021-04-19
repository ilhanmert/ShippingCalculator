using Npgsql;
using ShippingCalculator.CommonLayer.Logger.Concrete;
using ShippingCalculator.DataAccessLayer.PostgreSQL;
using ShippingCalculator.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace ShippingCalculator.BusinessLogicLayer.Concrete
{
    public class ShippingCompanyManager
    {
        private readonly Logger logger;
        private Database database;
        private NpgsqlDataReader reader;
        public ShippingCompanyManager(Logger _logger)
        {
            logger = _logger;
        }
        /// <summary>
        /// Kargo Şirketlerini Reader İle Getiren Fonksiyon
        /// </summary>
        /// <returns></returns>
        public List<ShippingCompany> GetShippingCompanies()
        {
            List<ShippingCompany> shippingCompanies = new List<ShippingCompany>(); // kargo şirketleri nesnesi oluşturuluyor.
            try
            {
                database = new Database(); // database nesnesi oluşturuluyor.
                using (database)
                {
                    reader = database.Select("cargo", "companies"); // database'de "cargo" şeması altında bulunan "companies" fonksiyonundan, kargo şirketleri reader'a atılıyor. 
                    if (reader!=null) // reader boş değil ise...
                    {
                        while (reader.Read()) // reader'da bulunan veriler shippingCompanies nesnesine ekleniyor.
                        {
                            shippingCompanies.Add(
                                new ShippingCompany
                                {
                                    Id=reader.GetInt32(reader.GetOrdinal("id")),
                                    Name=reader.GetString(reader.GetOrdinal("name"))
                                }
                                );
                        }
                    }
                }
            }
            catch (Exception ex) // reader boş ise hata yakalanıyor ve log oluşturuluyor.
            {
                logger.CreateLog(ex.Message);
            }
            return shippingCompanies; // işlemler bittikten sonra shippingCompanies nesnesi döndürülüyor.
        }
        /// <summary>
        /// Kargo Şirketlerini Json Formatında Getiren Fonksiyon
        /// </summary>
        /// <returns></returns>
        public string GetShippingCompaniesJson()
        {
            database = new Database(); // database nesnesi oluşturuluyor.
            string json = string.Empty; // json adında boş string oluşturuluyor.
            using (database)
            {
                json = database.SingleSelect("cargo", "companiesjson").ToString(); // database'de "cargo" şeması altında bulunan "companiesjson" fonksiyonundan, kargo şirketleri alınıyor ve string'e çevirilip json'a atılıyor.
            }
            return json; // json döndürülüyor.
        }
        /// <summary>
        /// Json Formatında Gelen Kargo Şirketlerini Nesneye Dönüştüren Fonksiyon
        /// </summary>
        /// <returns></returns>
        public List<ShippingCompany> GetShippingCompaniesJsonList()
        {
            try
            {
                string json = GetShippingCompaniesJson(); // GetShippingCompaniesJson() fonksiyonundan gelen json veriler buradaki json değişkenine aktarılıyor.
                if (!string.IsNullOrEmpty(json)) // json boş değil ise...
                {
                    return JsonSerializer.Deserialize<List<ShippingCompany>>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }); // gelen json veriler nesneye deserialize ediliyor ve döndürülüyor.
                }
            }
            catch (Exception ex) // json boş ise hata yakalanıyor ve log oluşturuluyor.
            {
                logger.CreateLog(ex.Message);
            }
            return null; // hata tutulduktan sonra null döndürülüyor.
        }
        /// <summary>
        /// Kargo Şirketi Ekleyen Fonksiyon
        /// </summary>
        /// <param name="shippingCompany">Kargo Şirketi Adı</param>
        /// <returns></returns>
        public NPGResult Create (ShippingCompany shippingCompany)
        {
            NPGResult result;
            try
            {
                if (!string.IsNullOrEmpty(shippingCompany.Name)) // kargo şirketi adı boş değil ise...
                {
                    database = new Database(); // database nesnesi oluşturuluyor.
                    using (database)
                    {
                        database.AddParameter("_name", shippingCompany.Name); // girilen kargo şirketi ismi database'de bulunan _name değişkenine atanıyor.
                        result = database.Insert("cargo", "company"); // girilen kargo şirketi database'de "cargo" şeması altında bulunan "company" fonksiyonuyla insert ediliyor.
                    }
                }
                else // kargo şirketi ismi boş girildiyse kullanıcıya hata mesajı gösteriliyor.
                {
                    result = new NPGResult();
                    result.ErrorCode = 700;
                    result.ErrorMessage = "Kargo Şirketi İsmi Boş Olamaz!";
                }
                return result;
            }
            catch (Exception ex) // hata yakalanıyor ve log oluşturuluyor.
            {
                logger.CreateLog(ex.Message);
            }
            return null; // null döndürülüyor.
        }
        /// <summary>
        /// Girilen ID ve Kargo Şirketi Adına Göre Kargo Şirketi Bilgilerini Güncelleyen Fonksiyon
        /// </summary>
        /// <param name="shippingCompany">Kargo Şirketi Adı</param>
        /// <returns></returns>
        public NPGResult Update(ShippingCompany shippingCompany)
        {
            NPGResult result;
            try
            {
                if (shippingCompany.Id > 0 && !string.IsNullOrEmpty(shippingCompany.Name)) // ID sıfırdan büyük ve kargo şirketi adı boş değil ise...
                {
                    database = new Database(); // database nesnesi oluşturuluyor.
                    using (database)
                    {
                        database.AddParameter("_id", shippingCompany.Id); // girilen ID database'de bulunan _id değişkenine atanıyor.
                        database.AddParameter("_name", shippingCompany.Name); // girilen kargo şirketi ismi database'de bulunan _name değişkenine atanıyor.
                        result = database.Update("cargo", "company"); // girilen kargo şirketi database'de "cargo" şeması altında bulunan "company" fonksiyonuyla update ediliyor.
                    }
                }
                else // ID sıfırdan küçük, boş girildiyse veya kargo şirketi adı boş girildiyse kullanıcıya hata mesajı gösteriliyor.
                {
                    result = new NPGResult();
                    result.ErrorCode = 700;
                    result.ErrorMessage = "Id Sıfırdan Büyük Olmalı ve Kargo Şirketi Adı Boş Olmalı!";
                }
                return result;
            }
            catch (Exception ex) // hata yakalanıyor ve log oluşturuluyor.
            {
                logger.CreateLog(ex.Message);
            }
            return null; // null döndürülüyor.
        }
        /// <summary>
        /// Girilen ID Değerine Sahip Kargo Şirketini Silen Fonksiyon
        /// </summary>
        /// <param name="shippingCompany"></param>
        /// <returns></returns>
        public NPGResult Delete(ShippingCompany shippingCompany)
        {
            NPGResult result;
            try
            {
                if (shippingCompany.Id > 0) // ID sıfırdan büyük ise...
                {
                    database = new Database(); // database nesnesi oluşturuluyor.
                    using (database)
                    {
                        database.AddParameter("_id", shippingCompany.Id); // girilen ID database'de bulunan _id değişkenine atanıyor.
                        result = database.Delete("cargo", "company"); // girilen kargo şirketi database'de "cargo" şeması altında bulunan "company" fonksiyonuyla delete ediliyor.
                    }
                }
                else // ID sıfırdan küçük veya boş girildiyse kullanıcıya hata mesajı gösteriliyor.
                {
                    result = new NPGResult();
                    result.ErrorCode = 700;
                    result.ErrorMessage = "Id Sıfırdan Büyük Olmalı!";
                }
                return result;
            }
            catch (Exception ex) // hata yakalanıyor ve log oluşturuluyor.
            {
                logger.CreateLog(ex.Message);
            }
            return null; // null döndürülüyor.
        }
    }
}
