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
            _mockCostumerRepository.Setup(repo => repo.GetAllAsync())
                                   .ReturnsAsync(costumers);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Costumer>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnCostumers = Assert.IsType<List<Costumer>>(okResult.Value);

            Assert.Equal(2, returnCostumers.Count);
        }

        [Fact]
        public async Task GetById_ExistingId_ShouldReturnCostumer()
        {
            // Arrange
            var costumerId = "1";
            var costumer = new Costumer(costumerId, "John Doe", "john.doe@example.com");
            _mockCostumerRepository.Setup(repo => repo.GetByIdAsync(costumerId))
                                   .ReturnsAsync(costumer);

            // Act
            var result = await _controller.GetById(costumerId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Costumer>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnCostumer = Assert.IsType<Costumer>(okResult.Value);

            Assert.Equal(costumerId, returnCostumer.Id);
        }

        [Fact]
        public async Task GetById_NonExistingId_ShouldReturnNotFound()
        {
            // Arrange
            var costumerId = "999";
            _mockCostumerRepository.Setup(repo => repo.GetByIdAsync(costumerId))
                                   .ReturnsAsync((Costumer)null);

            // Act
            var result = await _controller.GetById(costumerId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Create_ValidCostumer_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var newCostumer = new Costumer("3", "Sarah Connor", "sarah.connor@example.com");
            _mockCostumerRepository.Setup(repo => repo.CreateAsync(newCostumer))
                                   .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(newCostumer);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnCostumer = Assert.IsType<Costumer>(createdAtActionResult.Value);

            Assert.Equal(newCostumer.Id, returnCostumer.Id);
        }

        [Fact]
        public async Task Update_ExistingCostumer_ShouldReturnNoContent()
        {
            // Arrange
            var costumerId = "1";
            var costumer = new Costumer(costumerId, "Updated Costumer", "updated@example.com");
            _mockCostumerRepository.Setup(repo => repo.GetByIdAsync(costumerId))
                                   .ReturnsAsync(costumer);
            _mockCostumerRepository.Setup(repo => repo.UpdateAsync(costumerId, costumer))
                                   .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(costumerId, costumer);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_NonExistingCostumer_ShouldReturnNotFound()
        {
            // Arrange
            var costumerId = "999";
            var costumer = new Costumer(costumerId, "Updated Costumer", "updated@example.com");
            _mockCostumerRepository.Setup(repo => repo.GetByIdAsync(costumerId))
                                   .ReturnsAsync((Costumer)null);

            // Act
            var result = await _controller.Update(costumerId, costumer);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ExistingId_ShouldReturnNoContent()
        {
            // Arrange
            var costumerId = "1";
            _mockCostumerRepository.Setup(repo => repo.GetByIdAsync(costumerId))
                                   .ReturnsAsync(new Costumer(costumerId, "John Doe", "john.doe@example.com"));
            _mockCostumerRepository.Setup(repo => repo.DeleteAsync(costumerId))
                                   .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(costumerId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_NonExistingId_ShouldReturnNotFound()
        {
            // Arrange
            var costumerId = "999";
            _mockCostumerRepository.Setup(repo => repo.GetByIdAsync(costumerId))
                                   .ReturnsAsync((Costumer)null);

            // Act
            var result = await _controller.Delete(costumerId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
