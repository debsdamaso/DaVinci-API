using DaVinci.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Repositories
{
    public interface IFeedbackRepository
    {
        Task<IEnumerable<Feedback>> GetAllAsync();
        Task<Feedback> GetByIdAsync(string id);
        Task CreateAsync(Feedback feedback);
        Task UpdateAsync(string id, Feedback feedback);
        Task DeleteAsync(string id);
    }
}
