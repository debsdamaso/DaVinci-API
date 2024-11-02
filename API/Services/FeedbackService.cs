using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using DaVinci.Models;

namespace API.Services
{
    public class FeedbackService
    {
        private readonly IMongoCollection<Feedback> _feedbackCollection;

        public FeedbackService(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("DaVinciDB");
            _feedbackCollection = database.GetCollection<Feedback>("Feedback");
        }

        public async Task CreateFeedbackAsync(Feedback feedback)
        {
            await _feedbackCollection.InsertOneAsync(feedback);
        }

        public async Task<Feedback> GetFeedbackByIdAsync(string id)
        {
            return await _feedbackCollection.Find(f => f.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Feedback>> GetAllFeedbacksAsync()
        {
            return await _feedbackCollection.Find(_ => true).ToListAsync();
        }

        public async Task UpdateFeedbackAsync(Feedback feedback)
        {
            await _feedbackCollection.ReplaceOneAsync(f => f.Id == feedback.Id, feedback);
        }

        public async Task DeleteFeedbackAsync(string id)
        {
            await _feedbackCollection.DeleteOneAsync(f => f.Id == id);
        }
    }
}
