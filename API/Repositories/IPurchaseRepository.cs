using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models;

namespace API.Repositories
{
    public interface IPurchaseRepository
    {
        Task<List<Purchase>> GetPurchasesWithoutFeedbackAsync();
    }
}
