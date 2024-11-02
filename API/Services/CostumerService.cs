using DaVinci.Models;
using API.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Services
{
    public class CostumerService
    {
        private readonly ICostumerRepository _costumerRepository;

        public CostumerService(ICostumerRepository costumerRepository)
        {
            _costumerRepository = costumerRepository;
        }

        public async Task<IEnumerable<Costumer>> GetAllAsync()
        {
            return await _costumerRepository.GetAllAsync();
        }

        public async Task<Costumer> GetByIdAsync(string id)
        {
            return await _costumerRepository.GetByIdAsync(id);
        }

        public async Task CreateAsync(Costumer costumer)
        {
            await _costumerRepository.CreateAsync(costumer);
        }

        public async Task UpdateAsync(string id, Costumer costumer)
        {
            await _costumerRepository.UpdateAsync(id, costumer);
        }

        public async Task DeleteAsync(string id)
        {
            await _costumerRepository.DeleteAsync(id);
        }
    }
}
