using Npgsql;
using ShippingCalculator.CommonLayer.Logger.Concrete;
using ShippingCalculator.DataAccessLayer.PostgreSQL;
using ShippingCalculator.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShippingCalculator.BusinessLogicLayer.Concrete
{
    public class CorporationManager
    {
        private readonly Logger logger;
        private Database database;
        private NpgsqlDataReader reader;
        public CorporationManager(Logger _logger)
        {
            logger = _logger;
        }
        public List<Corporation> GetCorporations()
        {
            List<Corporation> corporations = new List<Corporation>();
            try
            {
                database = new Database();
                using (database)
                {
                    reader = database.Select("public", "corporations");
                    if (reader!=null)
                    {
                        while (reader.Read())
                        {
                            corporations.Add(
                                new Corporation()
                                {
                                    Id=reader.GetInt32(reader.GetOrdinal("id")),
                                    Password=reader.GetString(reader.GetOrdinal("password")),
                                    Discount=reader.GetDecimal(reader.GetOrdinal("discount"))
                                }
                                );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.CreateLog(ex.Message);
            }
            return corporations;
        }
        public string GetCorporationsJson()
        {
            database = new Database();
            string json = string.Empty;
            using (database)
            {
                json = database.SingleSelect("public", "corporationsjson").ToString();
            }
            return json;
        }
        public List<Corporation> GetCorporationsJsonList()
        {
            try
            {
                string json = GetCorporationsJson();
                if (!string.IsNullOrEmpty(json))
                {
                    return JsonSerializer.Deserialize<List<Corporation>>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                }
            }
            catch (Exception ex)
            {
                logger.CreateLog(ex.Message);
            }
            return null;
        }

    }
}
