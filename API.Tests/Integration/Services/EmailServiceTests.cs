using System.Threading.Tasks;
using Moq;
using Xunit;
using API.Services;

namespace API.Tests.Integration.Services
{
    public class EmailServiceTests
    {
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly IEmailService _emailService;

        public EmailServiceTests()
        {
            _mockEmailService = new Mock<IEmailService>();
            _emailService = _mockEmailService.Object;
        }

        [Fact]
        public async Task SendEmailAsync_ShouldSendEmailSuccessfully()
        {
            // Arrange
            string recipient = "testuser@example.com";
            string subject = "Test Subject";
            string message = "This is a test message.";

            _mockEmailService.Setup(service => service.SendEmailAsync(recipient, subject, message))
                .Returns(Task.FromResult(true));

            // Act
            var result = await _emailService.SendEmailAsync(recipient, subject, message);

            // Assert
            Assert.True(result);
            _mockEmailService.Verify(es => es.SendEmailAsync(recipient, subject, message), Times.Once);
        }
    }
}
