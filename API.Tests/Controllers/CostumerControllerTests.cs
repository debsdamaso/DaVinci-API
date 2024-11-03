using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Controllers;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using API.Repositories;

namespace API.Tests.Controllers
{
    public class CostumerControllerTests
    {
        private readonly Mock<ICostumerRepository> _mockCostumerRepository;
        private readonly CostumerController _controller;

        public CostumerControllerTests()
        {
            _mockCostumerRepository = new Mock<ICostumerRepository>();
            _controller = new CostumerController(_mockCostumerRepository.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnListOfCostumers()
        {
            // Arrange
            var costumers = new List<Costumer>
            {
                new Costumer("1", "John Doe", "john.doe@example.com"),
                new Costumer("2", "Jane Smith", "jane.smith@example.com")
            };
            _mockCostumerRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(costumers);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<Costumer>>(okResult.Value);
            Assert.Equal(costumers.Count, returnValue.Count());
        }

        [Fact]
        public async Task GetById_ShouldReturnCostumer_WhenCostumerExists()
        {
            // Arrange
            var costumer = new Costumer("1", "John Doe", "john.doe@example.com");
            _mockCostumerRepository.Setup(repo => repo.GetByIdAsync(costumer.Id)).ReturnsAsync(costumer);

            // Act
            var result = await _controller.GetById(costumer.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Costumer>(okResult.Value);
            Assert.Equal(costumer.Id, returnValue.Id);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenCostumerDoesNotExist()
        {
            // Arrange
            var costumerId = "99";
            _mockCostumerRepository.Setup(repo => repo.GetByIdAsync(costumerId)).ReturnsAsync((Costumer)null);

            // Act
            var result = await _controller.GetById(costumerId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateCostumer_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var newCostumer = new Costumer("3", "Alice Johnson", "alice.johnson@example.com");
            _mockCostumerRepository.Setup(repo => repo.CreateAsync(It.IsAny<Costumer>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(newCostumer);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<Costumer>(createdAtActionResult.Value);
            Assert.Equal(newCostumer.Id, returnValue.Id);
        }

        [Fact]
        public async Task UpdateCostumer_ShouldReturnNoContent()
        {
            // Arrange
            var existingCostumer = new Costumer("1", "John Doe", "john.doe@example.com");
            _mockCostumerRepository.Setup(repo => repo.GetByIdAsync(existingCostumer.Id)).ReturnsAsync(existingCostumer);
            _mockCostumerRepository.Setup(repo => repo.UpdateAsync(existingCostumer.Id, existingCostumer)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(existingCostumer.Id, existingCostumer);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateCostumer_ShouldReturnNotFound_WhenCostumerDoesNotExist()
        {
            // Arrange
            var nonExistentCostumer = new Costumer("99", "Non Existent", "non.existent@example.com");
            _mockCostumerRepository.Setup(repo => repo.GetByIdAsync(nonExistentCostumer.Id)).ReturnsAsync((Costumer)null);

            // Act
            var result = await _controller.Update(nonExistentCostumer.Id, nonExistentCostumer);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteCostumer_ShouldReturnNoContent()
        {
            // Arrange
            var costumerId = "1";
            var existingCostumer = new Costumer(costumerId, "John Doe", "john.doe@example.com");
            _mockCostumerRepository.Setup(repo => repo.GetByIdAsync(costumerId)).ReturnsAsync(existingCostumer);
            _mockCostumerRepository.Setup(repo => repo.DeleteAsync(costumerId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(costumerId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteCostumer_ShouldReturnNotFound_WhenCostumerDoesNotExist()
        {
            // Arrange
            var costumerId = "99";
            _mockCostumerRepository.Setup(repo => repo.GetByIdAsync(costumerId)).ReturnsAsync((Costumer)null);

            // Act
            var result = await _controller.Delete(costumerId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
