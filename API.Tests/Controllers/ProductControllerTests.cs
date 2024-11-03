using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Controllers;
using API.Services;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using API.Repositories;

namespace API.Tests.Controllers
{
    public class ProductControllerTests
    {
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly ProductService _productService;
        private readonly ProductController _controller;

        public ProductControllerTests()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _productService = new ProductService(_mockProductRepository.Object);
            _controller = new ProductController(_productService);
        }

        [Fact]
        public async Task GetAll_ShouldReturnListOfProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product("1", "Product A", "Description A", 10.0m),
                new Product("2", "Product B", "Description B", 20.0m)
            };
            _mockProductRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(products);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Product>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnProducts = Assert.IsType<List<Product>>(okResult.Value);

            Assert.Equal(2, returnProducts.Count);
        }

        [Fact]
        public async Task GetById_ExistingId_ShouldReturnProduct()
        {
            // Arrange
            var productId = "1";
            var product = new Product(productId, "Product A", "Description A", 10.0m);
            _mockProductRepository.Setup(repo => repo.GetByIdAsync(productId)).ReturnsAsync(product);

            // Act
            var result = await _controller.GetById(productId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Product>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnProduct = Assert.IsType<Product>(okResult.Value);

            Assert.Equal(productId, returnProduct.Id);
        }

        [Fact]
        public async Task GetById_NonExistingId_ShouldReturnNotFound()
        {
            // Arrange
            var productId = "999";
            _mockProductRepository.Setup(repo => repo.GetByIdAsync(productId)).ReturnsAsync((Product)null);

            // Act
            var result = await _controller.GetById(productId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Create_ValidProduct_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var newProduct = new Product("3", "Product C", "Description C", 30.0m);
            _mockProductRepository.Setup(repo => repo.CreateAsync(newProduct)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(newProduct);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result); // Verificação do tipo correto
            var returnProduct = Assert.IsType<Product>(createdAtActionResult.Value);

            Assert.Equal(newProduct.Id, returnProduct.Id);
        }

        [Fact]
        public async Task Update_ExistingProduct_ShouldReturnNoContent()
        {
            // Arrange
            var productId = "1";
            var product = new Product(productId, "Updated Product", "Updated Description", 15.0m);
            _mockProductRepository.Setup(repo => repo.UpdateAsync(productId, product)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(productId, product);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ExistingId_ShouldReturnNoContent()
        {
            // Arrange
            var productId = "1";
            _mockProductRepository.Setup(repo => repo.DeleteAsync(productId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(productId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
