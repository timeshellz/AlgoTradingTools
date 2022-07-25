using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlgoTrading.Stocks;

namespace AlgoTrading.Stocks.Persistence.Database
{
    public class StockDataDTO
    {
        public string FIGI { get; set; }
        public StockInfoDTO Info { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan Interval { get; set; }
        public ICollection<StockBarDTO> Bars { get; set; }
    }
}
