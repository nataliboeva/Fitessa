using Xunit;
using Moq;
using Fitessa.Services.Services;
using Fitessa.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

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
        public async Task SendEmailAsync_WithValidParameters_DoesNotThrow()
        {
            var to = "test@example.com";
            var subject = "Test Subject";
            var body = "Test Body";

            await _service.SendEmailAsync(to, subject, body);

        }

        [Fact]
        public async Task SendEmailAsync_WithEmptyTo_DoesNotThrow()
        {
            var to = "";
            var subject = "Test Subject";
            var body = "Test Body";

            await _service.SendEmailAsync(to, subject, body);


        }

        [Fact]
        public async Task SendEmailAsync_WithEmptySubject_DoesNotThrow()
        {
            var to = "test@example.com";
            var subject = "";
            var body = "Test Body";

            await _service.SendEmailAsync(to, subject, body);

        }

        [Fact]
        public async Task SendEmailAsync_WithEmptyBody_DoesNotThrow()
        {
            var to = "test@example.com";
            var subject = "Test Subject";
            var body = "";

            await _service.SendEmailAsync(to, subject, body);

        }

        [Fact]
        public async Task SendWelcomeEmailAsync_WithValidEmail_DoesNotThrow()
        {
            var email = "test@example.com";
            var name = "Test User";

            await _service.SendWelcomeEmailAsync(email, name);

        }

        [Fact]
        public async Task SendPasswordResetEmailAsync_WithValidEmail_DoesNotThrow()
        {
            var email = "test@example.com";
            var resetLink = "https://example.com/reset";

            await _service.SendPasswordResetEmailAsync(email, resetLink);

        }

        [Fact]
        public async Task SendWorkoutReminderEmailAsync_WithValidParameters_DoesNotThrow()
        {
            var email = "test@example.com";
            var workoutName = "Morning Workout";

            await _service.SendWorkoutReminderEmailAsync(email, "TestUser", workoutName);

        }
    }
} 