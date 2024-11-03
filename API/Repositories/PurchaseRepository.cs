using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using API.Models;

namespace API.Repositories
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly IMongoCollection<Purchase> _purchases;

        public PurchaseRepository(IMongoDatabase database)
        {
            _purchases = database.GetCollection<Purchase>("Purchases");
        }

        public async Task<List<Purchase>> GetAllPurchasesAsync()
        {
            return await _purchases.Find(p => true).ToListAsync();
        }

        public async Task<Purchase> GetPurchaseByIdAsync(string id)
        {
            return await _purchases.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddPurchaseAsync(Purchase purchase)
        {
            await _purchases.InsertOneAsync(purchase);
        }

        public async Task UpdatePurchaseAsync(Purchase purchase)
        {
            await _purchases.ReplaceOneAsync(p => p.Id == purchase.Id, purchase);
        }

        public async Task<List<Purchase>> GetPurchasesWithoutFeedbackAsync()
        {
            return await _purchases.Find(p => p.FeedbackId == null).ToListAsync();
        }

        public async Task DeletePurchaseAsync(string id)
        {
            await _purchases.DeleteOneAsync(p => p.Id == id);
        }

        public async Task AssociateFeedbackToPurchaseAsync(string purchaseId, string feedbackId)
        {
            var purchase = await GetPurchaseByIdAsync(purchaseId);
            if (purchase != null)
            {
                purchase.FeedbackId = feedbackId;
                await UpdatePurchaseAsync(purchase);
            }
        }
    }
}
