using ShippingCalculator.CommonLayer.Logger.Abstract;

namespace ShippingCalculator.DataAccessLayer.PostgreSQL
{
    public class DatabaseLogger : MyLogger
	{
		protected override string logpath
		{
			get => "DatabaseLogs";
			set => throw new System.NotImplementedException();
		}
	}
}
