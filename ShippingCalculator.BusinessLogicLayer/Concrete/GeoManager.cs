using ShippingCalculator.CommonLayer.Logger.Concrete;
using ShippingCalculator.DataAccessLayer.PostgreSQL;
using Npgsql;
using ShippingCalculator.Entities.Concrete;
using System.Collections.Generic;
using System;
using System.Text.Json;

namespace ShippingCalculator.BusinessLogicLayer.Concrete
{
    public class GeoManager
    {
        private readonly Logger logger;
        private Database database;
        private NpgsqlDataReader reader;
        public GeoManager(Logger _logger)
        {
            logger = _logger;
        }
        /// <summary>
        /// Kıtaları Reader İle Getiren Fonksiyon
        /// </summary>
        /// <returns></returns>
        public List<Continent> GetContinents()
        {
            List<Continent> continents = new List<Continent>(); // kıtalar nesnesi oluşturuluyor.
            try
            {
                database = new Database(); // database nesnesi oluşturuluyor.
                using (database)
                {
                    reader = database.Select("geo", "continents"); // database'de "geo" şeması altında bulunan "continents" fonksiyonundan, kıtalar reader'a atılıyor. 
                    if (reader != null) // reader boş değil ise...
                    {
                        while (reader.Read()) // reader'da bulunan veriler continents nesnesine ekleniyor.
                        {
                            continents.Add(
                                new Continent()
                                {
                                    Id=reader.GetInt32(reader.GetOrdinal("id")),
                                    Name = reader.GetString(reader.GetOrdinal("name"))
                                }
                                );
                        }
                    }
                }
            }
            catch (Exception ex) // reader boş ise hata yakalanıyor ver log oluşturuluyor.
            {
                logger.CreateLog(ex.Message);
            }
            return continents; // işlemler bittikten sonra continents nesnesi döndürülüyor.
        }
        /// <summary>
        /// Kıtaları Json Formatında Getiren Fonksiyon
        /// </summary>
        /// <returns></returns>
        public string GetContinentsJson()
        {
            database = new Database(); // database nesnesi oluşturuluyor.
            string json = string.Empty; // json adında boş string oluşturuluyor.
            using (database)
            {
                json = database.SingleSelect("geo", "continentsjson").ToString(); // database'de "geo" şeması altında bulunan "continentsjson" fonksiyonundan, kıtalar alınıyor ve string'e çevirilip json'a atılıyor.
            }
            return json; // json döndürülüyor.
        }
        /// <summary>
        /// Json Formatında Gelen Kıtaları Nesneye Dönüştüren Fonksiyon
        /// </summary>
        /// <returns></returns>
        public List<Continent> GetContinentsJsonList()
        {
            try
            {
                string json = GetContinentsJson(); // GetContinentsJson() fonksiyonundan gelen json veriler buradaki json değişkenine aktarılıyor.
                if (!string.IsNullOrEmpty(json)) // json boş değil ise...
                {
                    return JsonSerializer.Deserialize<List<Continent>>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }); // gelen json veriler nesneye deserialize ediliyor ve döndürülüyor.
                }
            }
            catch (Exception ex) // json boş ise hata yakalanıyor ve log oluşturuluyor.
            {
                logger.CreateLog(ex.Message);
            }
            return null; // hata tutulduktan sonra null döndürülüyor.
        }
        /// <summary>
        /// Ülkeleri Reader İle Getiren Fonksiyon
        /// </summary>
        /// <returns></returns>
        public List<Country> GetCountries()
        {
            List<Country> countries = new List<Country>(); // ülkeler nesnesi oluşturuluyor.
            try
            {
                database = new Database(); // database nesnesi oluşturuluyor.
                using (database)
                {
                    reader = database.Select("geo", "countries"); // database'de "geo" şeması altında bulunan "countries" fonksiyonundan, ülkeler reader'a atılıyor. 
                    if (reader != null) // reader boş değil ise...
                    {
                        while (reader.Read()) // reader'da bulunan veriler countries nesnesine ekleniyor.
                        {
                            countries.Add(
                                new Country()
                                {
                                    Id=reader.GetInt32(reader.GetOrdinal("id")),
                                    Code=reader.GetString(reader.GetOrdinal("code")),
                                    Name=reader.GetString(reader.GetOrdinal("name")),
                                    ContinentId=reader.GetInt32(reader.GetOrdinal("continentid"))
                                }
                                );
                        }
                    }
                }
            }
            catch (Exception ex) // reader boş ise hata yakalanıyor ver log oluşturuluyor.
            {
                logger.CreateLog(ex.Message);
            }
            return countries; // işlemler bittikten sonra countries nesnesi döndürülüyor.
        }
        /// <summary>
        /// Ülkeleri Json Formatında Getiren Fonksiyon
        /// </summary>
        /// <returns></returns>
        public string GetCountriesJson()
        {
            database = new Database(); // database nesnesi oluşturuluyor.
            string json = string.Empty; // json adında boş string oluşturuluyor.
            using (database)
            {
                json = database.SingleSelect("geo", "countriesjson").ToString(); // database'de "geo" şeması altında bulunan "countriesjson" fonksiyonundan, ülkeler alınıyor ve string'e çevirilip json'a atılıyor.
            }
            return json; // json döndürülüyor.
        }
        /// <summary>
        /// Json Formatında Gelen Ülkeleri Nesneye Dönüştüren Fonksiyon
        /// </summary>
        /// <returns></returns>
        public List<Country> GetCountriesJsonList()
        {
            try
            {
                string json = GetCountriesJson(); // GetCountriesJson() fonksiyonundan gelen json veriler buradaki json değişkenine aktarılıyor.
                if (!string.IsNullOrEmpty(json)) // json boş değil ise...
                {
                    return JsonSerializer.Deserialize<List<Country>>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }); // gelen json veriler nesneye deserialize ediliyor ve döndürülüyor.
                }
            }
            catch (Exception ex) // json boş ise hata yakalanıyor ve log oluşturuluyor.
            {
                logger.CreateLog(ex.Message);
            }
            return null; // hata tutulduktan sonra null döndürülüyor.
        }
        /// <summary>
        /// Kıta ID'sine Göre Ülkeleri Reader İle Getiren Fonksiyon
        /// </summary>
        /// <param name="id">KITA ID</param>
        /// <returns></returns>
        public List<Country> GetCountriesByContinentId(int id)
        {
            List<Country> countries = new List<Country>(); // ülkeler nesnesi oluşturuluyor.
            try
            {
                database = new Database(); // database nesnesi oluşturuluyor.
                using (database)
                {
                    database.AddParameter("_continentid", id); // girilen kıta id'si database'de bulunan kıta id'ye atanıyor.
                    reader = database.Select("geo", "countriesbycontinentid"); // database'de "geo" şeması altında bulunan "countriesbycontinentid" fonksiyonundan, ülkeler reader'a atılıyor.
                    if (reader != null) // reader boş değil ise...
                    {
                        while (reader.Read()) // reader'da bulunan veriler countries nesnesine ekleniyor.
                        {
                            countries.Add(
                                new Country()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                                    Code = reader.GetString(reader.GetOrdinal("code")),
                                    Name = reader.GetString(reader.GetOrdinal("name")),
                                    ContinentId = reader.GetInt32(reader.GetOrdinal("continentid"))
                                }
                                );
                        }
                    }
                }
            }
            catch (Exception ex) // reader boş ise hata yakalanıyor ver log oluşturuluyor.
            {
                logger.CreateLog(ex.Message);
            }
            return countries; // işlemler bittikten sonra countries nesnesi döndürülüyor.
        }
        /// <summary>
        /// Kıta ID'sine Göre Ülkeleri Json Formatında Getiren Fonksiyon
        /// </summary>
        /// <param name="id">KITA ID</param>
        /// <returns></returns>
        public List<Country> GetCountriesByContinentIdJson(int id)
        {
            List<Country> countries = new List<Country>(); // ülkeler nesnesi oluşturuluyor.
            try
            {
                database = new Database(); // database nesnesi oluşturuluyor.
                using (database)
                {
                    database.AddParameter("_continentid", id); // girilen kıta id'si database'de bulunan kıta id'ye atanıyor.
                    string json = database.SingleSelect("geo", "countriesbycontinentidjson").ToString(); // database'de "geo" şeması altında bulunan "countriesbycontinentid" fonksiyonundan, ülkeler alınıyor ve string'e çevirilip json'a atılıyor.
                    if (!string.IsNullOrEmpty(json)) // json boş değil ise...
                    {
                        countries = JsonSerializer.Deserialize<List<Country>>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }); // gelen json veriler nesneye deserialize ediliyor ve döndürülüyor.
                    }
                }
            }
            catch (Exception ex) // json boş ise hata yakalanıyor ve log oluşturuluyor.
            {
                logger.CreateLog(ex.Message);
            }
            return countries; // işlemler bittikten sonra countries nesnesi döndürülüyor.
        }
        /// <summary>
        /// Şehirleri Reader İle Getiren Fonksiyon
        /// </summary>
        /// <returns></returns>
        public List<City> GetCities()
        {
            List<City> cities = new List<City>(); // şehirler nesnesi oluşturuluyor.
            try
            {
                database = new Database(); // database nesnesi oluşturuluyor.
                using (database)
                {
                    reader = database.Select("geo", "cities"); // database'de "geo" şeması altında bulunan "cities" fonksiyonundan, şehirler alınıp reader'a atılıyor.
                    if (reader != null) // reader boş değil ise...
                    {
                        while (reader.Read()) // reader'da bulunan veriler cities nesnesine ekleniyor.
                        {
                            cities.Add(
                                new City()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                                    CountryCode=reader.GetString(reader.GetOrdinal("countrycode")),
                                    Name = reader.GetString(reader.GetOrdinal("name"))
                                }
                                );
                        }
                    }
                }
            }
            catch (Exception ex) // reader boş ise hata yakalanıyor ver log oluşturuluyor.
            {
                logger.CreateLog(ex.Message);
            }
            return cities; // işlemler bittikten sonra cities nesnesi döndürülüyor.
        }
        /// <summary>
        /// Şehirleri Json Formatında Getiren Fonksiyon
        /// </summary>
        /// <returns></returns>
        public string GetCitiesJson()
        {
            database = new Database(); // database nesnesi oluşturuluyor.
            string json = string.Empty; // json adında boş string oluşturuluyor.
            using (database)
            {
                json = database.SingleSelect("geo", "citiesjson").ToString(); // database'de "geo" şeması altında bulunan "citiesjson" fonksiyonundan, ülkeler alınıyor ve string'e çevirilip json'a atılıyor.
            }
            return json; // json döndürülüyor.
        }
        /// <summary>
        /// Json Formatında Gelen Şehirleri Nesneye Dönüştüren Fonksiyon
        /// </summary>
        /// <returns></returns>
        public List<City> GetCitiesJsonList()
        {
            try
            {
                string json = GetCitiesJson(); // GetCitiesJson() fonksiyonundan gelen json veriler buradaki json değişkenine aktarılıyor.
                if (!string.IsNullOrEmpty(json)) // json boş değil ise...
                {
                    return JsonSerializer.Deserialize<List<City>>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }); // gelen json veriler nesneye deserialize ediliyor ve döndürülüyor.
                }
            }
            catch (Exception ex) // json boş ise hata yakalanıyor ve log oluşturuluyor.
            {
                logger.CreateLog(ex.Message);
            }
            return null; // hata tutulduktan sonra null döndürülüyor.
        }
        /// <summary>
        /// Ülke CODE'una Göre Şehirleri Reader İle Getiren Fonksiyon
        /// </summary>
        /// <param name="code">ÜLKE KODU</param>
        /// <returns></returns>
        public List<City> GetCitiesByCountryCode(string code)
        {
            List<City> cities = new List<City>(); // şehirler nesnesi oluşturuluyor.
            try
            {
                database = new Database(); // database nesnesi oluşturuluyor.
                using (database)
                {
                    database.AddParameter("_countrycode", code); // girilen ülke code'u database'de bulunan ülke code'na atanıyor.
                    reader = database.Select("geo", "citiesbycountrycode"); // database'de "geo" şeması altında bulunan "citiesbycountrycode" fonksiyonundan, şehirler reader'a atılıyor.
                    if (reader != null) // reader boş değil ise...
                    {
                        while (reader.Read()) // reader'da bulunan veriler cities nesnesine ekleniyor.
                        {
                            cities.Add(
                                new City()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                                    CountryCode = reader.GetString(reader.GetOrdinal("countrycode")),
                                    Name = reader.GetString(reader.GetOrdinal("name")),
                                }
                                );
                        }
                    }
                }
            }
            catch (Exception ex) // reader boş ise hata yakalanıyor ver log oluşturuluyor.
            {
                logger.CreateLog(ex.Message);
            }
            return cities; // işlemler bittikten sonra cities nesnesi döndürülüyor.
        }
        /// <summary>
        /// Ülke CODE'una Göre Şehirleri Json Formatında Getiren Fonksiyon
        /// </summary>
        /// <param name="code">ÜLKE KODU</param>
        /// <returns></returns>
        public List<City> GetCitiesByCountryCodeJson(string code)
        {
            List<City> cities = new List<City>(); // ülkeler nesnesi oluşturuluyor.
            try
            {
                database = new Database(); // database nesnesi oluşturuluyor.
                using (database)
                {
                    database.AddParameter("_countrycode", code); // girilen ülke code'u database'de bulunan ülke code'una atanıyor.
                    string json = database.SingleSelect("geo", "citiesbycountrycodejson").ToString(); // database'de "geo" şeması altında bulunan "citiesbycountrycodejson" fonksiyonundan, şehirler alınıyor ve string'e çevirilip json'a atılıyor.
                    if (!string.IsNullOrEmpty(json)) // json boş değil ise...
                    {
                        cities = JsonSerializer.Deserialize<List<City>>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }); // gelen json veriler nesneye deserialize ediliyor ve döndürülüyor.
                    }
                }
            }
            catch (Exception ex) // json boş ise hata yakalanıyor ve log oluşturuluyor.
            {
                logger.CreateLog(ex.Message);
            }
            return cities; // işlemler bittikten sonra cities nesnesi döndürülüyor.
        }
        /// <summary>
        /// İlçeleri Reader İle Getiren Fonksiyon
        /// </summary>
        /// <returns></returns>
        public List<County> GetCounties()
        {
            List<County> counties = new List<County>(); // ilçeler nesnesi oluşturuluyor.
            try
            {
                database = new Database(); // database nesnesi oluşturuluyor.
                using (database)
                {
                    reader = database.Select("geo", "cities"); // database'de "geo" şeması altında bulunan "cities" fonksiyonundan, ilçeler reader'a atılıyor.
                    if (reader != null) // reader boş değil ise...
                    {
                        while (reader.Read()) // reader'da bulunan veriler counties nesnesine ekleniyor.
                        {
                            counties.Add(
                                new County()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                                    Name = reader.GetString(reader.GetOrdinal("name")),
                                    CityId = reader.GetInt32(reader.GetOrdinal("cityid"))
                                }
                                );
                        }
                    }
                }
            }
            catch (Exception ex) // reader boş ise hata yakalanıyor ver log oluşturuluyor.
            {
                logger.CreateLog(ex.Message);
            }
            return counties; // işlemler bittikten sonra counties nesnesi döndürülüyor.
        }
        /// <summary>
        /// İlçeleri Json Formatında Getiren Fonksiyon
        /// </summary>
        /// <returns></returns>
        public string GetCountiesJson()
        {
            database = new Database(); // database nesnesi oluşturuluyor.
            string json = string.Empty; // json adında boş string oluşturuluyor.
            using (database)
            {
                json = database.SingleSelect("geo", "countiesjson").ToString(); // database'de "geo" şeması altında bulunan "countiesjson" fonksiyonundan, ilçeler alınıyor ve string'e çevirilip json'a atılıyor.
            }
            return json; // json döndürülüyor.
        }
        /// <summary>
        /// Json Formatında Gelen İlçeleri Nesneye Dönüştüren Fonksiyon
        /// </summary>
        /// <returns></returns>
        public List<County> GetCountiesJsonList()
        {
            try
            {
                string json = GetCountiesJson(); // GetCountiesJson() fonksiyonundan gelen json veriler buradaki json değişkenine aktarılıyor.
                if (!string.IsNullOrEmpty(json)) // json boş değil ise...
                {
                    return JsonSerializer.Deserialize<List<County>>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }); // gelen json veriler nesneye deserialize ediliyor ve döndürülüyor.
                }
            }
            catch (Exception ex) // json boş ise hata yakalanıyor ve log oluşturuluyor.
            {
                logger.CreateLog(ex.Message);
            }
            return null; // hata tutulduktan sonra null döndürülüyor.
        }
        /// <summary>
        /// Şehir ID'sine Göre İlçeleri Reader İle Getiren Fonksiyon
        /// </summary>
        /// <param name="id">ŞEHİR ID</param>
        /// <returns></returns>
        public List<County> GetCountiesByCityId(int id)
        {
            List<County> counties = new List<County>(); // ilçeler nesnesi oluşturuluyor.
            try
            {
                database = new Database(); // database nesnesi oluşturuluyor.
                using (database)
                {
                    database.AddParameter("_cityid", id); // girilen ülke id'si database'de bulunan ülke id'sine atanıyor.
                    reader = database.Select("geo", "countiesbycityid"); // database'de "geo" şeması altında bulunan "countiesbycityid" fonksiyonundan, ilçeler reader'a atılıyor.
                    if (reader != null) // reader boş değil ise...
                    {
                        while (reader.Read()) // reader'da bulunan veriler counties nesnesine ekleniyor.
                        {
                            counties.Add(
                                new County()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                                    CityId=reader.GetInt32(reader.GetOrdinal("cityid")),
                                    Name = reader.GetString(reader.GetOrdinal("name")),
                                }
                                );
                        }
                    }
                }
            }
            catch (Exception ex) // reader boş ise hata yakalanıyor ver log oluşturuluyor.
            {
                logger.CreateLog(ex.Message);
            }
            return counties; // işlemler bittikten sonra counties nesnesi döndürülüyor.
        }
        /// <summary>
        /// Şehir ID'sine Göre İlçeleri Json Formatında Getiren Fonksiyon
        /// </summary>
        /// <param name="id">ŞEHİR ID</param>
        /// <returns></returns>
        public  List<County> GetCountiesByCityIdJson(int id)
        {
            List<County> counties = new List<County>(); // ilçeler nesnesi oluşturuluyor.
            try
            {
                database = new Database(); // database nesnesi oluşturuluyor.
                using (database)
                {
                    database.AddParameter("_cityid", id); // girilen şehir id'si database'de bulunan şehir id'sine atanıyor.
                    string json = database.SingleSelect("geo", "countiesbycityidjson").ToString(); // database'de "geo" şeması altında bulunan "countiesbycityidjson" fonksiyonundan, ilçeler alınıyor ve string'e çevirilip json'a atılıyor.
                    if (!string.IsNullOrEmpty(json)) // json boş değil ise...
                    {
                        counties = JsonSerializer.Deserialize<List<County>>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }); // gelen json veriler nesneye deserialize ediliyor ve döndürülüyor.
                    }
                }
            }
            catch (Exception ex) // json boş ise hata yakalanıyor ve log oluşturuluyor.
            {
                logger.CreateLog(ex.Message);
            }
            return counties; // işlemler bittikten sonra counties nesnesi döndürülüyor.
        }
    }
}


