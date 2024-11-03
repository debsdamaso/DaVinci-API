using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using Xunit;
using API.Models;
using API.Repositories;
using MongoDB.Bson;

namespace API.Tests.Integration.Repositories
{
    public class FeedbackRepositoryIntegrationTests
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;

        public FeedbackRepositoryIntegrationTests()
        {
            // Configuração de conexão com o MongoDB para teste
            var client = new MongoClient("mongodb+srv://rm97836:160994@davinci.8gubj.mongodb.net/test?retryWrites=true&w=majority");
            _client = client;
            _database = _client.GetDatabase("DaVinciTestDB");
            _feedbackRepository = new FeedbackRepository(_client);
        }

        [Fact]
        public async Task AddFeedback_ShouldAddFeedbackToDatabase()
        {
            // Arrange
            var feedback = new Feedback { Id = ObjectId.GenerateNewId().ToString(), CustomerId = "cust1", ProductId = "prod1", Comment = "Great product!", Sentiment = "Positive", SentimentScore = 0.9f };

            // Act
            await _feedbackRepository.CreateFeedbackAsync(feedback);
            var retrievedFeedback = await _feedbackRepository.GetByIdAsync(feedback.Id);

            // Assert
            Assert.NotNull(retrievedFeedback);
            Assert.Equal(feedback.Comment, retrievedFeedback.Comment);
        }

        [Fact]
        public async Task UpdateFeedback_ShouldUpdateFeedbackInDatabase()
        {
            // Arrange
            var feedback = new Feedback { Id = ObjectId.GenerateNewId().ToString(), CustomerId = "cust2", ProductId = "prod2", Comment = "Good product", Sentiment = "Neutral", SentimentScore = 0.5f };
            await _feedbackRepository.CreateFeedbackAsync(feedback);

            // Act
            feedback.Comment = "Excellent product!";
            feedback.Sentiment = "Positive";
            feedback.SentimentScore = 0.95f;
            await _feedbackRepository.UpdateAsync(feedback.Id, feedback);
            var updatedFeedback = await _feedbackRepository.GetByIdAsync(feedback.Id);

            // Assert
            Assert.NotNull(updatedFeedback);
            Assert.Equal("Excellent product!", updatedFeedback.Comment);
            Assert.Equal("Positive", updatedFeedback.Sentiment);
        }

        [Fact]
        public async Task DeleteFeedback_ShouldRemoveFeedbackFromDatabase()
        {
            // Arrange
            var feedback = new Feedback { Id = ObjectId.GenerateNewId().ToString(), CustomerId = "cust3", ProductId = "prod3", Comment = "Not satisfied", Sentiment = "Negative", SentimentScore = 0.2f };
            await _feedbackRepository.CreateFeedbackAsync(feedback);

            // Act
            await _feedbackRepository.DeleteAsync(feedback.Id);
            var deletedFeedback = await _feedbackRepository.GetByIdAsync(feedback.Id);

            // Assert
            Assert.Null(deletedFeedback);
        }
    }
}
