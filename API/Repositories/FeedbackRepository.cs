using MongoDB.Driver;
using API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly IMongoCollection<Feedback> _feedbacks;

        public FeedbackRepository(IMongoClient client)
        {
            var database = client.GetDatabase("DaVinciDB");
            _feedbacks = database.GetCollection<Feedback>("Feedbacks");
        }

        public async Task<IEnumerable<Feedback>> GetAllAsync()
        {
            return await _feedbacks.Find(feedback => true).ToListAsync();
        }

        public async Task<Feedback> GetByIdAsync(string id)
        {
            return await _feedbacks.Find(feedback => feedback.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateFeedbackAsync(Feedback feedback)
        {
            await _feedbacks.InsertOneAsync(feedback);
        }

        public async Task UpdateAsync(string id, Feedback feedback)
        {
            await _feedbacks.ReplaceOneAsync(f => f.Id == id, feedback);
        }

        public async Task DeleteAsync(string id)
        {
            await _feedbacks.DeleteOneAsync(f => f.Id == id);
        }
    }
}