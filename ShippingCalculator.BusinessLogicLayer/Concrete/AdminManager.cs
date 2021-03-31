using ShippingCalculator.CommonLayer.Logger.Concrete;
using ShippingCalculator.DataAccessLayer.PostgreSQL;
using ShippingCalculator.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace ShippingCalculator.BusinessLogicLayer.Concrete
{
    public class AdminManager
    {
        private readonly Logger logger;
        private Database database;
        public AdminManager(Logger _logger)
        {
            logger = _logger;
        }
        /// <summary>
        /// Adminlerin Bilgilerini Json Formatında Getiren Fonksiyon
        /// </summary>
        /// <returns></returns>
        public string GetAdminsJson()
        {
            database = new Database(); // database nesnesi oluşturuluyor.
            string json = string.Empty; // json adında boş string oluşturuluyor.
            using (database)
            {
                json = database.SingleSelect("admin", "adminsjson").ToString(); // database'de "admin" şeması altında bulunan "adminsjson" fonksiyonundan, adminlerin bilgileri alınıyor ve string'e çevirilip json'a atılıyor.
            }
            return json; // json döndürülüyor.
        }
        /// <summary>
        /// Json Formatında Gelen Adminlerin Bilgilerini Nesneye Dönüştüren Fonksiyon 
        /// </summary>
        /// <returns></returns>
        public List<Admin> GetAdminsJsonList()
        {
            try
            {
                string json = GetAdminsJson(); // GetAdminsJson() fonksiyonundan gelen json veriler buradaki json değişkenine aktarılıyor.
                if (!string.IsNullOrEmpty(json)) // json boş değil ise...
                {
                    return JsonSerializer.Deserialize<List<Admin>>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }); // gelen json veriler nesneye deserialize ediliyor ve döndürülüyor.
                }
            }
            catch (Exception ex) // json boş ise hata yakalanıyor ve log oluşturuluyor.
            {
                logger.CreateLog(ex.Message);
            }
            return null; // hata tutulduktan sonra null döndürülüyor.
        }
        /// <summary>
        /// Girilen Bilgilere Göre Kullanıcı Adı ve Şifreyi Kontrol Eden Fonksiyon
        /// </summary>
        /// <param name="data">Girilen Kullanıcı Adı ve Şifre</param>
        /// <returns></returns>
        public bool CheckAdmins(Admin data)
        {
            List<Admin> admins = GetAdminsJsonList(); // GetAdminsJsonList() fonksiyonundan gelen veriler admins nesnesine atılıyor.
            foreach (var item in admins) // admins nesnesinin içi foreach ile geziliyor.
            {
                if (admins.Find(x => x.UserName == data.UserName && x.Password == data.Password) != null) // girilen bilgiler ile aynı olan veri bulunursa true döndürülüyor.
                {
                    return true;
                }
            }
            return false; // girilen bilgiler ile aynı veri bulunamazsa false döndürülüyor.
        }
    }
}
