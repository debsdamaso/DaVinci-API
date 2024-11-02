using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using API.Controllers;
using DaVinci.Models;
using API.Repositories;

namespace API.Tests.Controllers
{
    public class FeedbackControllerTests
    {
        private readonly FeedbackController _controller;
        private readonly Mock<IFeedbackRepository> _mockRepository;

        public FeedbackControllerTests()
        {
            _mockRepository = new Mock<IFeedbackRepository>();
            _controller = new FeedbackController(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAllFeedbacks_ShouldReturnOkResult_WithListOfFeedbacks()
        {
            // Arrange
            var feedbacks = new List<Feedback>
            {
                new Feedback { Id = "1", CustomerId = "123", ProductId = "456", Comment = "Great product!", Sentiment = "Positive", SentimentScore = 0.95f },
                new Feedback { Id = "2", CustomerId = "124", ProductId = "457", Comment = "Not bad", Sentiment = "Neutral", SentimentScore = 0.5f }
            };

            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(feedbacks);

            // Act
            var result = await _controller.GetAllFeedbacks();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Feedback>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnedFeedbacks = Assert.IsAssignableFrom<IEnumerable<Feedback>>(okResult.Value);
            Assert.Equal(feedbacks.Count, returnedFeedbacks.Count());
        }

        [Fact]
        public async Task GetFeedbackById_ExistingId_ShouldReturnOkResult_WithFeedback()
        {
            // Arrange
            var feedbackId = "1";
            var feedback = new Feedback { Id = feedbackId, CustomerId = "123", ProductId = "456", Comment = "Great product!", Sentiment = "Positive", SentimentScore = 0.95f };
            _mockRepository.Setup(repo => repo.GetByIdAsync(feedbackId)).ReturnsAsync(feedback);

            // Act
            var result = await _controller.GetFeedbackById(feedbackId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Feedback>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnedFeedback = Assert.IsType<Feedback>(okResult.Value);
            Assert.Equal(feedbackId, returnedFeedback.Id);
        }

        [Fact]
        public async Task GetFeedbackById_NonExistingId_ShouldReturnNotFound()
        {
            // Arrange
            var feedbackId = "99";
            _mockRepository.Setup(repo => repo.GetByIdAsync(feedbackId)).ReturnsAsync((Feedback)null);

            // Act
            var result = await _controller.GetFeedbackById(feedbackId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Feedback>>(result);
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async Task CreateFeedback_ValidFeedback_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var feedback = new Feedback
            {
                Id = "1",
                CustomerId = "123",
                ProductId = "456",
                Comment = "Great product!",
                Sentiment = "Positive",
                SentimentScore = 0.95f
            };

            _mockRepository.Setup(repo => repo.CreateAsync(feedback)).Returns(Task.CompletedTask);
            _mockRepository.Setup(repo => repo.GetByIdAsync(feedback.Id)).ReturnsAsync(feedback); // Simulating retrieval for CreatedAtAction

            // Act
            var result = await _controller.CreateFeedback(feedback);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Feedback>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            Assert.Equal("GetFeedbackById", createdAtActionResult.ActionName);
            Assert.Equal(feedback.Id, createdAtActionResult.RouteValues["id"]);
            Assert.Equal(feedback, createdAtActionResult.Value);
        }

        [Fact]
        public async Task UpdateFeedback_ExistingId_ShouldReturnNoContent()
        {
            // Arrange
            var feedbackId = "1";
            var feedback = new Feedback { Id = feedbackId, CustomerId = "123", ProductId = "456", Comment = "Updated feedback", Sentiment = "Positive", SentimentScore = 0.99f };
            _mockRepository.Setup(repo => repo.GetByIdAsync(feedbackId)).ReturnsAsync(feedback);
            _mockRepository.Setup(repo => repo.UpdateAsync(feedbackId, feedback)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateFeedback(feedbackId, feedback);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateFeedback_NonExistingId_ShouldReturnNotFound()
        {
            // Arrange
            var feedbackId = "99";
            var feedback = new Feedback { Id = feedbackId, CustomerId = "123", ProductId = "456", Comment = "Updated feedback", Sentiment = "Positive", SentimentScore = 0.99f };
            _mockRepository.Setup(repo => repo.GetByIdAsync(feedbackId)).ReturnsAsync((Feedback)null);

            // Act
            var result = await _controller.UpdateFeedback(feedbackId, feedback);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteFeedback_ExistingId_ShouldReturnNoContent()
        {
            // Arrange
            var feedbackId = "1";
            var feedback = new Feedback { Id = feedbackId, CustomerId = "123", ProductId = "456", Comment = "Great product!", Sentiment = "Positive", SentimentScore = 0.95f };
            _mockRepository.Setup(repo => repo.GetByIdAsync(feedbackId)).ReturnsAsync(feedback);
            _mockRepository.Setup(repo => repo.DeleteAsync(feedbackId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteFeedback(feedbackId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteFeedback_NonExistingId_ShouldReturnNotFound()
        {
            // Arrange
            var feedbackId = "99";
            _mockRepository.Setup(repo => repo.GetByIdAsync(feedbackId)).ReturnsAsync((Feedback)null);

            // Act
            var result = await _controller.DeleteFeedback(feedbackId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
