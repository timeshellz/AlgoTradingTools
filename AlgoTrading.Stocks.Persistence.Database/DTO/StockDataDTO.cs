using System;
using System.Collections.Generic;

namespace AlgoTrading.Stocks.Persistence.Database
{
    public class StockDataDTO
    {
        public int Id { get; set; }
        public string FIGI { get; set; }
        public StockInfoDTO Info { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<StockBarDTO> Bars { get; set; }
    }
}
