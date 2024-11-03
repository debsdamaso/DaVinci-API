using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using Xunit;
using API.Models;
using API.Repositories;
using MongoDB.Bson;

namespace API.Tests.Integration.Repositories
{
    public class PurchaseRepositoryIntegrationTests
    {
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;

        public PurchaseRepositoryIntegrationTests()
        {
            // Configuração de conexão com o MongoDB para teste
            var client = new MongoClient("mongodb+srv://rm97836:160994@davinci.8gubj.mongodb.net/test?retryWrites=true&w=majority");
            _client = client;
            _database = _client.GetDatabase("DaVinciTestDB");
            _purchaseRepository = new PurchaseRepository(_database);
        }

        [Fact]
        public async Task AddPurchase_ShouldAddPurchaseToDatabase()
        {
            // Arrange
            var purchase = new Purchase
            {
                Id = ObjectId.GenerateNewId().ToString(),
                CustomerId = "cust1",
                ProductId = "prod1",
                PurchaseDate = DateTime.UtcNow
            };

            // Act
            await _purchaseRepository.AddPurchaseAsync(purchase);
            var retrievedPurchase = await _purchaseRepository.GetPurchaseByIdAsync(purchase.Id);

            // Assert
            Assert.NotNull(retrievedPurchase);
            Assert.Equal(purchase.CustomerId, retrievedPurchase.CustomerId);
        }

        [Fact]
        public async Task UpdatePurchase_ShouldUpdatePurchaseInDatabase()
        {
            // Arrange
            var purchase = new Purchase
            {
                Id = ObjectId.GenerateNewId().ToString(),
                CustomerId = "cust2",
                ProductId = "prod2",
                PurchaseDate = DateTime.UtcNow
            };
            await _purchaseRepository.AddPurchaseAsync(purchase);

            // Act
            purchase.ProductId = "prod3";
            await _purchaseRepository.UpdatePurchaseAsync(purchase);
            var updatedPurchase = await _purchaseRepository.GetPurchaseByIdAsync(purchase.Id);

            // Assert
            Assert.NotNull(updatedPurchase);
            Assert.Equal("prod3", updatedPurchase.ProductId);
        }

        [Fact]
        public async Task DeletePurchase_ShouldRemovePurchaseFromDatabase()
        {
            // Arrange
            var purchase = new Purchase
            {
                Id = ObjectId.GenerateNewId().ToString(),
                CustomerId = "cust3",
                ProductId = "prod3",
                PurchaseDate = DateTime.UtcNow
            };
            await _purchaseRepository.AddPurchaseAsync(purchase);

            // Act
            await _purchaseRepository.DeletePurchaseAsync(purchase.Id);
            var deletedPurchase = await _purchaseRepository.GetPurchaseByIdAsync(purchase.Id);

            // Assert
            Assert.Null(deletedPurchase);
        }
    }
}
