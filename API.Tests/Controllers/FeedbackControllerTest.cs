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
    public class FeedbackControllerTests
    {
        private readonly Mock<IFeedbackRepository> _mockFeedbackRepository;
        private readonly FeedbackController _controller;

        public FeedbackControllerTests()
        {
            _mockFeedbackRepository = new Mock<IFeedbackRepository>();
            _controller = new FeedbackController(_mockFeedbackRepository.Object);
        }

        [Fact]
        public async Task GetAllFeedbacks_ShouldReturnListOfFeedbacks()
        {
            // Arrange
            var feedbacks = new List<Feedback>
            {
                new Feedback { Id = "1", CustomerId = "1", ProductId = "1", Comment = "Great product!", Sentiment = "positive", SentimentScore = 0.9F },
                new Feedback { Id = "2", CustomerId = "2", ProductId = "2", Comment = "Not satisfied.", Sentiment = "negative", SentimentScore = 0.2F }
            };
            _mockFeedbackRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(feedbacks);

            // Act
            var result = await _controller.GetAllFeedbacks();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<Feedback>>(okResult.Value);
            Assert.Equal(feedbacks.Count, returnValue.Count());
        }

        [Fact]
        public async Task GetFeedbackById_ShouldReturnFeedback_WhenFeedbackExists()
        {
            // Arrange
            var feedback = new Feedback { Id = "1", CustomerId = "1", ProductId = "1", Comment = "Great product!", Sentiment = "positive", SentimentScore = 0.9F };
            _mockFeedbackRepository.Setup(repo => repo.GetByIdAsync(feedback.Id)).ReturnsAsync(feedback);

            // Act
            var result = await _controller.GetFeedbackById(feedback.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Feedback>(okResult.Value);
            Assert.Equal(feedback.Id, returnValue.Id);
        }

        [Fact]
        public async Task GetFeedbackById_ShouldReturnNotFound_WhenFeedbackDoesNotExist()
        {
            // Arrange
            var feedbackId = "99";
            _mockFeedbackRepository.Setup(repo => repo.GetByIdAsync(feedbackId)).ReturnsAsync((Feedback?)null);

            // Act
            var result = await _controller.GetFeedbackById(feedbackId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateFeedback_ShouldReturnOk_WhenFeedbackIsValid()
        {
            // Arrange
            var newFeedback = new Feedback { Id = "3", CustomerId = "1", ProductId = "3", Comment = "Amazing service!", Sentiment = "positive", SentimentScore = 0.95F };
            _mockFeedbackRepository.Setup(repo => repo.CreateFeedbackAsync(It.IsAny<Feedback>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateFeedback(newFeedback);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<object>(okResult.Value);
        }

        [Fact]
        public async Task UpdateFeedback_ShouldReturnNoContent_WhenFeedbackExists()
        {
            // Arrange
            var existingFeedback = new Feedback { Id = "1", CustomerId = "1", ProductId = "1", Comment = "Great product!", Sentiment = "positive", SentimentScore = 0.9F };
            _mockFeedbackRepository.Setup(repo => repo.GetByIdAsync(existingFeedback.Id)).ReturnsAsync(existingFeedback);
            _mockFeedbackRepository.Setup(repo => repo.UpdateAsync(existingFeedback.Id, existingFeedback)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateFeedback(existingFeedback.Id, existingFeedback);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateFeedback_ShouldReturnNotFound_WhenFeedbackDoesNotExist()
        {
            // Arrange
            var nonExistentFeedback = new Feedback { Id = "99", CustomerId = "1", ProductId = "99", Comment = "Not great", Sentiment = "negative", SentimentScore = 0.3F };
            _mockFeedbackRepository.Setup(repo => repo.GetByIdAsync(nonExistentFeedback.Id)).ReturnsAsync((Feedback?)null);

            // Act
            var result = await _controller.UpdateFeedback(nonExistentFeedback.Id, nonExistentFeedback);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteFeedback_ShouldReturnNoContent_WhenFeedbackExists()
        {
            // Arrange
            var feedbackId = "1";
            var existingFeedback = new Feedback { Id = feedbackId, CustomerId = "1", ProductId = "1", Comment = "Great product!", Sentiment = "positive", SentimentScore = 0.9F };
            _mockFeedbackRepository.Setup(repo => repo.GetByIdAsync(feedbackId)).ReturnsAsync(existingFeedback);
            _mockFeedbackRepository.Setup(repo => repo.DeleteAsync(feedbackId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteFeedback(feedbackId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteFeedback_ShouldReturnNotFound_WhenFeedbackDoesNotExist()
        {
            // Arrange
            var feedbackId = "99";
            _mockFeedbackRepository.Setup(repo => repo.GetByIdAsync(feedbackId)).ReturnsAsync((Feedback?)null);

            // Act
            var result = await _controller.DeleteFeedback(feedbackId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
