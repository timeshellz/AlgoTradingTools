using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AlgoTrading.Neural.Persistence.Database.DTO;

namespace AlgoTrading.Neural.Persistence.Database
{
    public class DatabaseNeuralPersistenceManager : INeuralPersistenceManager
    {
        IDbContextFactory<NeuralDataContext> dbFactory;

        public DatabaseNeuralPersistenceManager(IDbContextFactory<NeuralDataContext> factory)
        {
            dbFactory = factory;
        }

        public async Task<NeuralNetwork> LoadNeuralNetwork(string networkName)
        {
            try
            {
                using (var db = dbFactory.CreateDbContext())
                {
                    var networkDTO = await db.NeuralNetworks
                    .Include(n => n.Configuration)
                    .Include(n => n.Inputs)
                    .Include(n => n.Neurons)
                    .Include(n => n.Outputs)
                    .FirstOrDefaultAsync(n => n.Configuration.NetworkName == networkName);

                    return networkDTO.GetModel();
                }
            }
            catch (Exception e)
            { }

            return null;
        }

        public async Task SaveNeuralNetwork(NeuralNetwork network)
        {
            try
            {
                using (var db = dbFactory.CreateDbContext())
                {
                    var existingNetwork = await db.NeuralNetworks
                        .Include(n => n.Configuration)
                        .Include(n => n.Connections)
                        .FirstOrDefaultAsync(n => n.Configuration.NetworkName == network.Settings.NetworkName);

                    if (existingNetwork != null)
                    {
                        existingNetwork.PasteConnectionWeights(network.GetDTO().Connections);
                        db.NeuralNetworks.Update(existingNetwork);
                    }
                    else
                        db.NeuralNetworks.Add(network.GetDTO());

                    await db.SaveChangesAsync();
                }
            }
            catch (Exception e)
            { }
            
        }
    }
}
