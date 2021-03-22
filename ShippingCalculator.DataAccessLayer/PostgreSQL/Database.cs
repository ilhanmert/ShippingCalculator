using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using ShippingCalculator.Entities.Concrete;

namespace ShippingCalculator.DataAccessLayer.PostgreSQL
{
    public class Database : IDisposable
	{
		private IConfiguration configuration = null;
		NpgsqlConnection connection = null;
		NpgsqlCommand command = null;
		Dictionary<string, object> parameters = null;
		NpgsqlDataReader reader = null;
		NpgsqlTransaction transaction = null;
		DatabaseLogger logger = new DatabaseLogger();
		private NpgsqlConnection GetConnection(bool isselect = true)
		{
			if (configuration == null)
			{
				try
				{
					// string path = AppDomain.CurrentDomain.BaseDirectory+"appsettings.json";
					configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", false).Build();
					//configuration = new ConfigurationBuilder().SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)).AddJsonFile("appsettingsdal.json", optional: true, reloadOnChange: true).Build();

				}
				catch (Exception ex)
				{
					logger.CreateLog(ex);
					return null;
				}
			}
			if (connection == null)
				connection = new NpgsqlConnection(isselect ? configuration["ConnectionStrings:DefaultConnection"] : configuration["ConnectionStrings:ManipulateConnection"]);
			return connection;
		}

		//private string GetBasePath()
		//{
		//    var realPath = Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).FullName;

		//    using var processModule = Process.GetCurrentProcess().MainModule;
		//   // return Path.GetDirectoryName(processModule?.FileName);
		//    return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		//}

		private void OpenConnection(NpgsqlConnection con)
		{
			if (!con.State.HasFlag(ConnectionState.Open))
				con.Open();
		}

		public void AddParameter(string name, object value)
		{
			if (parameters == null)
				parameters = new Dictionary<string, object>();
			if (parameters.ContainsKey(name.Trim()))
				parameters[name.Trim()] = value;
			else
				parameters.Add(name.Trim(), value);
		}

		private void PassParameters(string schema, string funcname, bool type = true, string action = "Select")
		{
			if (command == null)
				command = new NpgsqlCommand();
			else
				command.Parameters.Clear();
			if (transaction != null)
				command.Transaction = transaction;
			command.Connection = GetConnection(action == "Select");
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("SELECT {0}{1}.{2}{3}(", type ? "* FROM " : string.Empty, schema, funcname, action);
			int counter = 0;
			string parametername;
			if (parameters != null)
				foreach (KeyValuePair<string, object> item in parameters)
				{
					if (counter > 0)
						sb.Append(",");
					parametername = string.Format("@{0}", item.Key);
					sb.Append(parametername);
					command.Parameters.AddWithValue(parametername, item.Value);
					counter++;
				}
			sb.Append(")");
			command.CommandText = sb.ToString();
			OpenConnection(command.Connection);
		}

		public void OpenTransaction()
		{
			OpenConnection(GetConnection(false));
			if (transaction == null)
				transaction = GetConnection().BeginTransaction();
			else
			{
#if DEBUG
				throw new Exception("Bir önceki işleminiz yarım kalmış olabilir");
#endif
				logger.CreateLog("Database-> OpenTransaction-->Hata: Bir önceki işleminiz yarım kalmış olabilir.");
			}
		}
		public void CommitTransaction()
		{
			if (transaction != null)
			{
				transaction.Commit();
				transaction = null;
			}
			else
			{
#if DEBUG
				throw new Exception("İşlem düzgün başlatılmadığı için onaylanamaz.");
#endif
			}
		}
		public void RollbackTransaction()
		{
			if (transaction != null)
			{
				transaction.Rollback();
				transaction = null;
			}
			else
			{
#if DEBUG
				throw new Exception("İşlem düzgün başlatılmadığı için geri alınamaz.");
#endif
			}
		}
		public NpgsqlDataReader Select(string schema, string funcname, bool clearparameters = false)
		{
			try
			{
				PassParameters(schema, funcname);
				reader = command.ExecuteReader(CommandBehavior.CloseConnection);
			}
			catch (Exception ex)
			{
#if debug
                throw ex;
#endif
				logger.CreateLog(ex);
				if (reader != null)
				{
					reader.Close();
					reader = null;
				}
			}
			if (clearparameters)
				this.parameters.Clear();
			return reader;
		}

		public NPGResult Insert(string schema, string funcname)
		{
			PassParameters(schema, funcname, false, "Insert");
			int rowsaffected = 0;
			NPGResult returnresult = new NPGResult();
			try
			{
				bool result = int.TryParse(command.ExecuteScalar().ToString(), out rowsaffected);
				returnresult.IsSuccess = result;
				returnresult.RowsAffected = rowsaffected;
			}
			catch (NpgsqlException npgex)
			{
				try
				{
					PostgresException pex = npgex.GetBaseException() as PostgresException;
					if (pex != null && short.TryParse(pex.Code, out short code))
						returnresult.ErrorCode = code;
					else
						returnresult.ErrorCode = 0;
					returnresult.ErrorMessage = npgex.Message.Replace("P0001", string.Empty);
					logger.CreateLog(npgex);

				}
				catch (Exception ex)
				{
					logger.CreateLog(ex);
				}
			}
			CloseConnection();
			return returnresult;
		}
		public NPGResult InsertAfter(string schema, string funcname)
		{
			PassParameters(schema, funcname, false, "Insert");
			NPGResult returnresult = new NPGResult();
			try
			{
				returnresult.Data = command.ExecuteScalar();
				if (returnresult.Data != null)
				{
					returnresult.IsSuccess = true;
					returnresult.RowsAffected = 1;
				}
				else
				{
					returnresult.IsSuccess = false;
					returnresult.RowsAffected = 0;
				}
				return returnresult;
			}
			catch (NpgsqlException npgex)
			{
				try
				{
					PostgresException pex = npgex.GetBaseException() as PostgresException;
					if (pex != null && short.TryParse(pex.Code, out short code))
						returnresult.ErrorCode = code;
					else
						returnresult.ErrorCode = 0;
					returnresult.ErrorMessage = npgex.Message.Replace("P0001", string.Empty);
					logger.CreateLog(npgex);

				}
				catch (Exception ex)
				{
					logger.CreateLog(ex);
				}
			}
			CloseConnection();
			return returnresult;
		}

		public NPGResult Update(string schema, string funcname, bool clearparameters = false)
		{
			PassParameters(schema, funcname, false, "Update");
			int rowsaffected = 0;
			NPGResult returnresult = new NPGResult();
			try
			{
				bool result = int.TryParse(command.ExecuteScalar().ToString(), out rowsaffected);
				returnresult.IsSuccess = result;
				returnresult.RowsAffected = rowsaffected;
			}
			catch (NpgsqlException npgex)
			{
				returnresult.ErrorMessage = npgex.Message.Replace("P0001", string.Empty);
				logger.CreateLog(npgex);
			}
			CloseConnection();
			if (clearparameters)
				this.parameters.Clear();
			return returnresult;
		}

		public NpgsqlDataReader UpdateAndSelect(string schema, string funcname)
		{
			try
			{
				PassParameters(schema, funcname, true, "Update");
				reader = command.ExecuteReader(CommandBehavior.CloseConnection);
			}
			catch (Exception ex)
			{
#if debug
                throw ex;
#endif
				if (reader != null)
				{
					reader.Close();
					reader = null;
				}
				logger.CreateLog(ex);
			}
			return reader;
		}

		public NPGResult Delete(string schema, string funcname)
		{
			PassParameters(schema, funcname, false, "Delete");
			int rowsaffected = 0;
			NPGResult returnresult = new NPGResult();
			try
			{
				bool result = int.TryParse(command.ExecuteScalar().ToString(), out rowsaffected);
				returnresult.IsSuccess = result;
				returnresult.RowsAffected = rowsaffected;
			}
			catch (NpgsqlException npgex)
			{
				returnresult.ErrorMessage = npgex.Message.Replace("P0001", string.Empty);
				logger.CreateLog(npgex);
			}
			CloseConnection();
			return returnresult;
		}

		public void Dispose()
		{
			this.parameters = null;
			if (reader != null)
			{
				reader.Close();
				reader = null;
			}
			CloseConnection();
		}
		private void CloseConnection()
		{
			if (connection != null && connection.State.HasFlag(ConnectionState.Open))
				connection.Close();
		}

		public object SingleSelect(string schema, string funcname, bool clearparameters = false)
		{
			try
			{
				PassParameters(schema, funcname);
				object result = command.ExecuteScalar();
				if (clearparameters)
					this.parameters.Clear();
				return result;
			}
			catch (Exception ex)
			{
				logger.CreateLog(ex);
				return null;
			}
		}

		public object SingleSelectWithException(string schema, string funcname)
		{
			try
			{
				PassParameters(schema, funcname);
				return command.ExecuteScalar();
			}
			catch (NpgsqlException npgex)
			{
				NPGResult result = new NPGResult();
				try
				{
					logger.CreateLog(npgex);
					PostgresException pex = npgex.GetBaseException() as PostgresException;
					if (pex != null && short.TryParse(pex.Code, out short code))
					{
						result.ErrorMessage = pex.Message;
						result.ErrorCode = code;
					}
					else
					{
						result.ErrorCode = 700;
						result.ErrorMessage = npgex.Message.Replace("P0001", string.Empty); ;
						return result;
					}

				}
				catch (Exception ex)
				{

					result.ErrorCode = 700;
					result.ErrorMessage = ex.Message;
					logger.CreateLog(ex);
					return result;

				}
				return result;
			}
		}
	}
}

