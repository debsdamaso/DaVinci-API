using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using API.Controllers;
using API.Models;
using API.Repositories;
using API.Services;

namespace API.Tests.Controllers
{
    public class PurchaseControllerTests
    {
        private readonly Mock<IPurchaseRepository> _mockPurchaseRepository;
        private readonly Mock<IFeedbackRepository> _mockFeedbackRepository;
        private readonly Mock<FeedbackReminderService> _mockFeedbackReminderService;
        private readonly PurchaseController _controller;

        public PurchaseControllerTests()
        {
            _mockPurchaseRepository = new Mock<IPurchaseRepository>();
            _mockFeedbackRepository = new Mock<IFeedbackRepository>();
            _mockFeedbackReminderService = new Mock<FeedbackReminderService>(null, null);
            _controller = new PurchaseController(
                _mockPurchaseRepository.Object,
                _mockFeedbackReminderService.Object,
                _mockFeedbackRepository.Object);
        }

        [Fact]
        public async Task GetAllPurchases_ShouldReturnListOfPurchases()
        {
            // Arrange
            var purchases = new List<Purchase>
            {
                new Purchase { Id = "1", CustomerId = "123", ProductId = "456", PurchaseDate = System.DateTime.Now },
                new Purchase { Id = "2", CustomerId = "789", ProductId = "101", PurchaseDate = System.DateTime.Now }
            };
            _mockPurchaseRepository.Setup(repo => repo.GetAllPurchasesAsync()).ReturnsAsync(purchases);

            // Act
            var result = await _controller.GetAllPurchases();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedPurchases = Assert.IsAssignableFrom<List<Purchase>>(okResult.Value);
            Assert.Equal(2, returnedPurchases.Count);
        }

        [Fact]
        public async Task GetPurchaseById_ShouldReturnPurchase_WhenPurchaseExists()
        {
            // Arrange
            var purchase = new Purchase { Id = "1", CustomerId = "123", ProductId = "456", PurchaseDate = System.DateTime.Now };
            _mockPurchaseRepository.Setup(repo => repo.GetPurchaseByIdAsync("1")).ReturnsAsync(purchase);

            // Act
            var result = await _controller.GetPurchaseById("1");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedPurchase = Assert.IsType<Purchase>(okResult.Value);
            Assert.Equal("1", returnedPurchase.Id);
        }

        [Fact]
        public async Task GetPurchaseById_ShouldReturnNotFound_WhenPurchaseDoesNotExist()
        {
            // Arrange
            _mockPurchaseRepository.Setup(repo => repo.GetPurchaseByIdAsync("1")).ReturnsAsync((Purchase)null);

            // Act
            var result = await _controller.GetPurchaseById("1");

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreatePurchase_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var purchase = new Purchase { Id = "1", CustomerId = "123", ProductId = "456", PurchaseDate = System.DateTime.Now };
            _mockPurchaseRepository.Setup(repo => repo.AddPurchaseAsync(purchase)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreatePurchase(purchase);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnedPurchase = Assert.IsType<Purchase>(createdAtActionResult.Value);
            Assert.Equal("1", returnedPurchase.Id);
        }

        [Fact]
        public async Task UpdatePurchase_ShouldReturnNoContent_WhenPurchaseIsUpdated()
        {
            // Arrange
            var purchase = new Purchase { Id = "1", CustomerId = "123", ProductId = "456", PurchaseDate = System.DateTime.Now };
            _mockPurchaseRepository.Setup(repo => repo.GetPurchaseByIdAsync("1")).ReturnsAsync(purchase);
            _mockPurchaseRepository.Setup(repo => repo.UpdatePurchaseAsync(purchase)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdatePurchase("1", purchase);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdatePurchase_ShouldReturnNotFound_WhenPurchaseDoesNotExist()
        {
            // Arrange
            _mockPurchaseRepository.Setup(repo => repo.GetPurchaseByIdAsync("1")).ReturnsAsync((Purchase)null);

            // Act
            var result = await _controller.UpdatePurchase("1", new Purchase { Id = "1", CustomerId = "123", ProductId = "456", PurchaseDate = System.DateTime.Now });

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeletePurchase_ShouldReturnNoContent_WhenPurchaseIsDeleted()
        {
            // Arrange
            var purchase = new Purchase { Id = "1", CustomerId = "123", ProductId = "456", PurchaseDate = System.DateTime.Now };
            _mockPurchaseRepository.Setup(repo => repo.GetPurchaseByIdAsync("1")).ReturnsAsync(purchase);
            _mockPurchaseRepository.Setup(repo => repo.DeletePurchaseAsync("1")).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeletePurchase("1");

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeletePurchase_ShouldReturnNotFound_WhenPurchaseDoesNotExist()
        {
            // Arrange
            _mockPurchaseRepository.Setup(repo => repo.GetPurchaseByIdAsync("1")).ReturnsAsync((Purchase)null);

            // Act
            var result = await _controller.DeletePurchase("1");

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
