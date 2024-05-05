using System;
using System.Data;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ValuteConverter.Model;
using ValuteConverter.Model.Interfaces;

[assembly : InternalsVisibleTo("ConverterTests")]
namespace ValuteConverter
{
    /// <summary>
    /// Class for SOAP request to www.cbr.ru
    /// </summary>
    
    internal class SoapRequest : ICourseConverter
    {
        /// <summary>
        /// Wrapper for <see cref="M:ValuteConverter.SoapRequest.GetCursOnDate(System.DateTime)"/> to asynchronous call
        /// </summary>
        /// <param name="dateTime">Parameter for getting exchange rates for a given <see cref="System.DateTime"/></param>
        /// <returns>An asynchronous operation that can return a value</returns>
        public async Task<DataTable> GetExchangeRateOnDateAsync(DateTime dateTime)
        {
            return await Task.Run(() => GetExchangeRate(dateTime));
        }
        /// <summary>
        /// Get exchange rates from <see href="https://cbr.ru/"/>
        /// </summary>
        /// <param name="dateTime">Parameter for getting exchange rates for a given <see cref="System.DateTime"/></param>
        /// <returns>DataTable with exchange rates</returns>
        public DataTable GetExchangeRate(DateTime dateTime)
        {
            string requestText = @"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
  <soap:Body>
    <GetCursOnDate xmlns=""http://web.cbr.ru/"">
      <On_date>" + dateTime.ToString("yyyy-MM-ddThh:mm:ss") + @"</On_date>
    </GetCursOnDate>
  </soap:Body>
</soap:Envelope>";
            byte[] postData = Encoding.UTF8.GetBytes(requestText);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.cbr.ru/DailyInfoWebServ/DailyInfo.asmx");
            request.Method = "POST";
            request.ContentType = "text/xml; charset=utf-8";
            request.ContentLength = postData.Length;
            request.Headers.Add("SOAPAction", "http://web.cbr.ru/GetCursOnDate");

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.WriteAsync(postData, 0, postData.Length);
            }

            DataSet ds = new("CursValute");
            try
            {
                WebResponse response = request.GetResponseAsync().Result;
                using (Stream responseStream = response.GetResponseStream())
                using (XmlReader xmlReader = XmlReader.Create(responseStream))
                {
                    xmlReader.ReadToFollowing("schema", "http://www.w3.org/2001/XMLSchema");
                    ds.ReadXmlSchema(xmlReader);
                    ds.ReadXml(xmlReader);
                }
            }
            catch { }
            return ds?.Tables[0] ?? new DataTable();
        }
    }
}
