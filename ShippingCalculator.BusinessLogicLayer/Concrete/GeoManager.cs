using ShippingCalculator.CommonLayer.Logger.Concrete;
using ShippingCalculator.DataAccessLayer.PostgreSQL;
using Npgsql;
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
    }
}
