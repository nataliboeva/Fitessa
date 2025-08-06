using Xunit;
using Moq;
using Fitessa.Services.Services;
using Fitessa.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Fitessa.Tests.Services
{
    public class EmailServiceTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<ILogger<EmailService>> _mockLogger;
        private readonly EmailService _service;

        public EmailServiceTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockLogger = new Mock<ILogger<EmailService>>();
            _service = new EmailService(_mockConfiguration.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task SendEmailAsync_WithValidParameters_ReturnsTrue()
        {
            var to = "test@example.com";
            var subject = "Test Subject";
            var body = "Test Body";

            var result = await _service.SendEmailAsync(to, subject, body);

            Assert.True(result);
        }

        [Fact]
        public async Task SendEmailAsync_WithEmptyTo_ReturnsFalse()
        {
            var to = "";
            var subject = "Test Subject";
            var body = "Test Body";

            var result = await _service.SendEmailAsync(to, subject, body);

            Assert.False(result);
        }

        [Fact]
        public async Task SendEmailAsync_WithEmptySubject_ReturnsFalse()
        {
            var to = "test@example.com";
            var subject = "";
            var body = "Test Body";

            var result = await _service.SendEmailAsync(to, subject, body);

            Assert.False(result);
        }

        [Fact]
        public async Task SendEmailAsync_WithEmptyBody_ReturnsFalse()
        {
            var to = "test@example.com";
            var subject = "Test Subject";
            var body = "";

            var result = await _service.SendEmailAsync(to, subject, body);

            Assert.False(result);
        }

        [Fact]
        public async Task SendWelcomeEmailAsync_WithValidEmail_ReturnsTrue()
        {
            var email = "test@example.com";
            var name = "Test User";

            var result = await _service.SendWelcomeEmailAsync(email, name);

            Assert.True(result);
        }

        [Fact]
        public async Task SendPasswordResetEmailAsync_WithValidEmail_ReturnsTrue()
        {
            var email = "test@example.com";
            var resetLink = "https://example.com/reset";

            var result = await _service.SendPasswordResetEmailAsync(email, resetLink);

            Assert.True(result);
        }

        [Fact]
        public async Task SendWorkoutReminderEmailAsync_WithValidParameters_ReturnsTrue()
        {
            var email = "test@example.com";
            var workoutName = "Morning Workout";

            var result = await _service.SendWorkoutReminderEmailAsync(email, workoutName);

            Assert.True(result);
        }
    }
} 