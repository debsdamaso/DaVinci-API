using System.Threading.Tasks;
using Xunit;
using MongoDB.Driver;
using API.Models;
using MongoDB.Bson;
using API.Repositories;

namespace API.Tests.Integration.Repositories
{
    public class CostumerRepositoryIntegrationTests
    {
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;
        private readonly CostumerRepository _costumerRepository;

        public CostumerRepositoryIntegrationTests()
        {
            // Configuração de conexão com o MongoDB para teste
            var client = new MongoClient("mongodb+srv://rm97836:160994@davinci.8gubj.mongodb.net/test?retryWrites=true&w=majority");
            _client = client;
            _database = _client.GetDatabase("DaVinciTestDB");
            var mongoClient = client;
            _costumerRepository = new CostumerRepository(_client);
        }

        [Fact]
        public async Task AddCostumer_ShouldAddCostumerToDatabase()
        {
            // Arrange
            var newCostumer = new Costumer(ObjectId.GenerateNewId().ToString(), "Test User", "testuser@example.com");

            // Act
            await _costumerRepository.CreateAsync(newCostumer);
            var retrievedCostumer = await _costumerRepository.GetByIdAsync(newCostumer.Id);

            // Assert
            Assert.NotNull(retrievedCostumer);
            Assert.Equal(newCostumer.Name, retrievedCostumer.Name);
            Assert.Equal(newCostumer.Email, retrievedCostumer.Email);
        }

        [Fact]
        public async Task UpdateCostumer_ShouldUpdateCostumerInDatabase()
        {
            // Arrange
            var existingCostumer = new Costumer(ObjectId.GenerateNewId().ToString(), "Original User", "original@example.com");
            await _costumerRepository.CreateAsync(existingCostumer);

            // Act
            existingCostumer.Name = "Updated User";
            existingCostumer.Email = "updated@example.com";
            await _costumerRepository.UpdateAsync(existingCostumer.Id, existingCostumer);
            var updatedCostumer = await _costumerRepository.GetByIdAsync(existingCostumer.Id);

            // Assert
            Assert.NotNull(updatedCostumer);
            Assert.Equal("Updated User", updatedCostumer.Name);
            Assert.Equal("updated@example.com", updatedCostumer.Email);
        }

        [Fact]
        public async Task DeleteCostumer_ShouldRemoveCostumerFromDatabase()
        {
            // Arrange
            var costumerToDelete = new Costumer(ObjectId.GenerateNewId().ToString(), "User to Delete", "delete@example.com");
            await _costumerRepository.CreateAsync(costumerToDelete);

            // Act
            await _costumerRepository.DeleteAsync(costumerToDelete.Id);
            var deletedCostumer = await _costumerRepository.GetByIdAsync(costumerToDelete.Id);

            // Assert
            Assert.Null(deletedCostumer);
        }
    }
}
