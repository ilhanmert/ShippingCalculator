using Npgsql;
using ShippingCalculator.CommonLayer.Logger.Concrete;
using ShippingCalculator.DataAccessLayer.PostgreSQL;

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

    }
}
