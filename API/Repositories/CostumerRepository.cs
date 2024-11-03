using MongoDB.Driver;
using API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Repositories
{
    public class CostumerRepository : ICostumerRepository
    {
        private readonly IMongoCollection<Costumer> _costumers;

        public CostumerRepository(IMongoClient client)
        {
            var database = client.GetDatabase("DaVinciDB"); // Nome do banco
            _costumers = database.GetCollection<Costumer>("Costumers");
        }

        public async Task<IEnumerable<Costumer>> GetAllAsync()
        {
            return await _costumers.Find(costumer => true).ToListAsync();
        }

        public async Task<Costumer> GetByIdAsync(string id)
        {
            return await _costumers.Find(costumer => costumer.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Costumer costumer)
        {
            await _costumers.InsertOneAsync(costumer);
        }

        public async Task UpdateAsync(string id, Costumer costumer)
        {
            await _costumers.ReplaceOneAsync(c => c.Id == id, costumer);
        }

        public async Task DeleteAsync(string id)
        {
            await _costumers.DeleteOneAsync(costumer => costumer.Id == id);
        }
    }
}
