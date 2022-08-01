using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AlgoTrading.Neural.Persistence.Database
{
    public class DatabaseNeuralPersistenceManager : INeuralPersistenceManager
    {
        IDbContextFactory<StockDataContext> dbFactory;

        public DatabaseStockPersistenceManager(IDbContextFactory<StockDataContext> factory)
        {
            dbFactory = factory;
        }

        public Task<NeuralNetwork> LoadNeuralNetwork(string networkName)
        {
            throw new NotImplementedException();
        }

        public Task SaveNeuralNetwok(NeuralNetwork network)
        {
            throw new NotImplementedException();
        }
    }
}
