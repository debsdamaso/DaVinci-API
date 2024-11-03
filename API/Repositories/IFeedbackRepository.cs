using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models;

namespace API.Repositories
{
    public interface IFeedbackRepository
    {
        Task<IEnumerable<Feedback>> GetAllAsync();
        Task<Feedback> GetByIdAsync(string id);
        Task CreateFeedbackAsync(Feedback feedback);  // Alterado o nome do método para criar feedbacks
        Task UpdateAsync(string id, Feedback feedback);
        Task DeleteAsync(string id);
    }
}
