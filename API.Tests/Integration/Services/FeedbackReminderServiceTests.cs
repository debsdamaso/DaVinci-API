using System;
using System.Threading.Tasks;
using Moq;
using Xunit;
using API.Services;
using API.Repositories;
using API.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Collections.Generic;

namespace API.Tests.Integration.Services
{
    public class FeedbackReminderServiceTests
    {
        private readonly FeedbackReminderService _feedbackReminderService;
        private readonly Mock<IPurchaseRepository> _mockPurchaseRepository;
        private readonly Mock<IEmailService> _mockEmailService;

        public FeedbackReminderServiceTests()
        {
            _mockPurchaseRepository = new Mock<IPurchaseRepository>();
            _mockEmailService = new Mock<IEmailService>();

            _feedbackReminderService = new FeedbackReminderService(
                _mockPurchaseRepository.Object,
                _mockEmailService.Object
            );
        }

        [Fact]
        public async Task SendFeedbackRemindersAsync_ShouldSendEmailForPurchasesWithoutFeedback()
        {
            // Arrange
            var purchases = new List<Purchase>
            {
                new Purchase
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    CustomerId = "cust1",
                    ProductId = "prod1",
                    PurchaseDate = DateTime.UtcNow,
                    CustomerEmail = "testuser@example.com"
                }
            };

            _mockPurchaseRepository.Setup(repo => repo.GetPurchasesWithoutFeedbackAsync()).ReturnsAsync(purchases);
            _mockEmailService.Setup(service => service.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(true));


            // Act
            await _feedbackReminderService.SendFeedbackRemindersAsync();

            // Assert
            _mockEmailService.Verify(es => es.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}
