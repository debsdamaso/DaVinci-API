using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models;

namespace API.Repositories
{
    public interface IPurchaseRepository
    {
        Task<List<Purchase>> GetAllPurchasesAsync();
        Task<Purchase> GetPurchaseByIdAsync(string id);
        Task AddPurchaseAsync(Purchase purchase);
        Task UpdatePurchaseAsync(Purchase purchase);
        Task<List<Purchase>> GetPurchasesWithoutFeedbackAsync(); // Novo método para obter compras sem feedback
        Task DeletePurchaseAsync(string id); // Novo método para deletar uma compra
        Task AssociateFeedbackToPurchaseAsync(string purchaseId, string feedbackId); // Novo método para associar feedback a uma compra
    }
}