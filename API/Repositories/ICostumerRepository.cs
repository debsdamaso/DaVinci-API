using DaVinci.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Repositories
{
    public interface ICostumerRepository
    {
        Task<IEnumerable<Costumer>> GetAllAsync();
        Task<Costumer> GetByIdAsync(string id);
        Task CreateAsync(Costumer costumer);
        Task UpdateAsync(string id, Costumer costumer);
        Task DeleteAsync(string id);
    }
}
