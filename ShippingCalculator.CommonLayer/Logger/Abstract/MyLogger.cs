using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ShippingCalculator.CommonLayer.Logger.Abstract
{
    internal class LoggerItem
	{
		public DateTime DateTime
		{
			get; set;
		}
		public string LogMessage
		{
			get; set;
		}
	}
	public abstract class MyLogger
	{
		const int timeout = 600000;//10dk
		List<LoggerItem> previouslogs = new List<LoggerItem>();
		protected abstract string logpath //log tutulacak ana klasörün path'i
		{
			get;
			set;
		}
		object filelocker = new object(); //klasörü oluştururken kitlenmemesi için filelocker objesi oluşturuluyor
		/// <summary>
		/// Gelen log mesajı daha önceden listede var mı diye kontrol eder
		/// </summary>
		/// <param name="message">hata mesajı</param>
		/// <param name="now">tarih</param>
		/// <returns></returns>
		bool HasLog(string message, DateTime now)
		{
			previouslogs.RemoveAll(i => (now - i.DateTime).TotalMilliseconds > timeout);
			return previouslogs.FirstOrDefault(x => x.LogMessage == message) != null;
		}
		/// <summary>
		/// Hatanın logunu txt dosyasına tutar
		/// </summary>
		/// <param name="ex">hata</param>
		/// <param name="extrainformation">ektra bilgi</param>
		public void CreateLog(Exception ex, string extrainformation = null)
		{

			Log(ex.Message, ex.StackTrace, extrainformation);
		}
		/// <summary>
		/// Girilen mesajın logunu tutar
		/// </summary>
		/// <param name="message">mesaj</param>
		public void CreateLog(string message)
		{
			Log(message);
		}
		/// <summary>
		/// Gelen hata bilgisini txt dosyasına yazar
		/// </summary>
		/// <param name="path">hata dosyasının yazılacağı yer</param>
		/// <param name="message">yazılacak hata mesajı</param>
		/// <param name="stacktrace">hatanın stacktrace'i</param>
		/// <param name="extrainfo">ekstra bilgi</param>
		/// <param name="now">hatanın zamanı</param>
		void Append(string path, string message, string stacktrace, string extrainfo, DateTime now)
		{
			using (FileStream fs = new FileStream(Path.Combine(path, "log.txt"), FileMode.Append))
			{
				using (StreamWriter sw = new StreamWriter(fs))
				{
					if (!string.IsNullOrEmpty(stacktrace))
						sw.WriteLine(string.Format("Saat:{0}\nMesaj:{1}{3}\nTrace:{2}\n\n", now.ToString("HH:mm:ss"), message, stacktrace, string.IsNullOrEmpty(extrainfo) ? string.Empty : "\nBilgi : " + extrainfo + "\n\n"));
					else
						sw.WriteLine(string.Format("Saat:{0}\nMesaj:{1}{2}\n\n", now.ToString("HH:mm:ss"), message, string.IsNullOrEmpty(extrainfo) ? string.Empty : "\nBilgi : " + extrainfo + "\n\n"));
				}
			}
		}

		void Log(string message, string stacktrace = null, string extrainfo = null)
		{
			if (string.IsNullOrEmpty(message)) //gelen mesajın doluluğu kontrol ediliyor
				return;
			DateTime now = DateTime.Now;
			if (HasLog(message, now)) //Gelen log mesajı daha önceden listede var mı diye kontrol eder
				return;
			string path = Path.Combine(logpath, now.Year.ToString(), now.Month.ToString().PadLeft(2, '0'), now.Day.ToString().PadLeft(2, '0')); //log tutulacak ana path, yıl, ay, gün şeklinde path oluşturur mainpath/yy/mm/dd
			try
			{
				lock (filelocker)
				{
					// oluşturulan pathlerde klasörler yoksa klasörler oluşturulur
					if (!Directory.Exists(logpath))
						Directory.CreateDirectory(logpath);
					if (!Directory.Exists(Path.Combine(logpath, now.Year.ToString())))
						Directory.CreateDirectory(Path.Combine(logpath, now.Year.ToString()));
					if (!Directory.Exists(path))
						Directory.CreateDirectory(path);
					Append(path, message, stacktrace, extrainfo, now); //hata mesajı yazdırılır
				}
			}
			catch { }
		}
	}
}
