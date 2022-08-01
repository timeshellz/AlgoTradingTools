﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoTrading.Stocks
{
    public class StockIdentifier

    {
        public string Name { get; }
        public string FIGI { get; }
        public string Currency { get; set; }
        public string Sector { get; set; }
        public string Country { get; set; }

        public StockIdentifier(string name, string figi, string currency, string sector, string country)
        {
            Name = name;
            FIGI = figi;
            Currency = currency;
            Sector = sector;
            Country = country;
        }
    }
}