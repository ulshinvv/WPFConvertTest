using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValuteConverter.Model.Interfaces
{
    /// <summary>
    /// You may want to pull data from different providers. Here it's interface ;D
    /// </summary>
    public interface ICourseConverter
    {
        public Task<DataTable> GetExchangeRateOnDateAsync(DateTime dateTime);
        public DataTable GetExchangeRate(DateTime dateTime);
    }
}
