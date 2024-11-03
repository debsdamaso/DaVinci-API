using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using Xunit;
using API.Models;
using API.Repositories;
using MongoDB.Bson;

namespace API.Tests.Integration.Repositories
{
    public class ProductRepositoryIntegrationTests
    {
        private readonly IProductRepository _productRepository;
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;

        public ProductRepositoryIntegrationTests()
        {
            // Configuração de conexão com o MongoDB para teste
            var client = new MongoClient("mongodb+srv://rm97836:160994@davinci.8gubj.mongodb.net/test?retryWrites=true&w=majority");
            _client = client;
            _database = _client.GetDatabase("DaVinciTestDB");
            _productRepository = new ProductRepository(_client);
        }

        [Fact]
        public async Task AddProduct_ShouldAddProductToDatabase()
        {
            // Arrange
            var product = new Product(ObjectId.GenerateNewId().ToString(), "Test Product", "A product for testing", 99.99m);

            // Act
            await _productRepository.CreateAsync(product);
            var retrievedProduct = await _productRepository.GetByIdAsync(product.Id);

            // Assert
            Assert.NotNull(retrievedProduct);
            Assert.Equal(product.Name, retrievedProduct.Name);
        }

        [Fact]
        public async Task UpdateProduct_ShouldUpdateProductInDatabase()
        {
            // Arrange
            var product = new Product(ObjectId.GenerateNewId().ToString(), "Old Product", "Old description", 49.99m);
            await _productRepository.CreateAsync(product);

            // Act
            product.Name = "Updated Product";
            product.Description = "Updated description";
            product.Price = 79.99m;
            await _productRepository.UpdateAsync(product.Id, product);
            var updatedProduct = await _productRepository.GetByIdAsync(product.Id);

            // Assert
            Assert.NotNull(updatedProduct);
            Assert.Equal("Updated Product", updatedProduct.Name);
        }

        [Fact]
        public async Task DeleteProduct_ShouldRemoveProductFromDatabase()
        {
            // Arrange
            var product = new Product(ObjectId.GenerateNewId().ToString(), "Product to Delete", "This product will be deleted", 19.99m);
            await _productRepository.CreateAsync(product);

            // Act
            await _productRepository.DeleteAsync(product.Id);
            var deletedProduct = await _productRepository.GetByIdAsync(product.Id);

            // Assert
            Assert.Null(deletedProduct);
        }
    }
}