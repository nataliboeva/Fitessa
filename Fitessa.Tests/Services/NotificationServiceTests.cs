using Xunit;
using Moq;
using Fitessa.Services.Services;
using Fitessa.Services.Interfaces;
using Fitessa.Data;
using Microsoft.EntityFrameworkCore;
using Fitessa.Data.Entities;
using System.Collections.Generic;


namespace Fitessa.Tests.Services
{
    public class NotificationServiceTests
    {
        private readonly ApplicationDbContext _context;
        private readonly NotificationService _service;

        public NotificationServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _service = new NotificationService(_context);
        }

        [Fact]
        public void GetAll_ReturnsAllNotifications()
        {
            var user1 = new ApplicationUser { Id = "user1", UserName = "user1@example.com", FirstName = "User", LastName = "One", Email = "user1@example.com", Gender = "Other", ProfilePictureUrl = "/images/default-profile.png" };
            var user2 = new ApplicationUser { Id = "user2", UserName = "user2@example.com", FirstName = "User", LastName = "Two", Email = "user2@example.com", Gender = "Other", ProfilePictureUrl = "/images/default-profile.png" };
            var notifications = new List<Notification>
            {
                new Notification { Id = 1, UserId = "user1", Message = "Test 1", Type = "info", Title = "Test 1", User = user1 },
                new Notification { Id = 2, UserId = "user2", Message = "Test 2", Type = "warning", Title = "Test 2", User = user2 }
            };

            _context.Notifications.AddRange(notifications);
            _context.SaveChanges();

            var result = _service.GetAll();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void GetByUser_WithValidUserId_ReturnsUserNotifications()
        {
            var userId = "test-user";
            var testUser = new ApplicationUser { Id = userId, UserName = "test@example.com", FirstName = "Test", LastName = "User", Email = "test@example.com", Gender = "Other", ProfilePictureUrl = "/images/default-profile.png" };
            var otherUser = new ApplicationUser { Id = "other-user", UserName = "other@example.com", FirstName = "Other", LastName = "User", Email = "other@example.com", Gender = "Other", ProfilePictureUrl = "/images/default-profile.png" };
            var notifications = new List<Notification>
            {
                new Notification { Id = 1, UserId = userId, Message = "Test 1", Type = "info", Title = "Test 1", User = testUser },
                new Notification { Id = 2, UserId = userId, Message = "Test 2", Type = "warning", Title = "Test 2", User = testUser },
                new Notification { Id = 3, UserId = "other-user", Message = "Test 3", Type = "info", Title = "Test 3", User = otherUser }
            };

            _context.Notifications.AddRange(notifications);
            _context.SaveChanges();

            var result = _service.GetByUser(userId);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, n => Assert.Equal(userId, n.UserId));
        }

        [Fact]
        public void GetById_WithValidId_ReturnsNotification()
        {
            var testUser = new ApplicationUser { Id = "test-user", UserName = "test@example.com", FirstName = "Test", LastName = "User", Email = "test@example.com", Gender = "Other", ProfilePictureUrl = "/images/default-profile.png" };
            var notification = new Notification
            {
                Id = 1,
                UserId = "test-user",
                Message = "Test Message",
                Type = "info",
                Title = "Test Message",
                User = testUser
            };

            _context.Notifications.Add(notification);
            _context.SaveChanges();

            var result = _service.GetById(1);

            Assert.NotNull(result);
            Assert.Equal("Test Message", result.Message);
        }

        [Fact]
        public void GetById_WithInvalidId_ReturnsNull()
        {
            var result = _service.GetById(999);

            Assert.Null(result);
        }

        [Fact]
        public void Create_WithValidNotification_AddsToDatabase()
        {
            var testUser = new ApplicationUser { Id = "test-user", UserName = "test@example.com", FirstName = "Test", LastName = "User", Email = "test@example.com", Gender = "Other", ProfilePictureUrl = "/images/default-profile.png" };
            var notification = new Notification
            {
                UserId = "test-user",
                Message = "Test Message",
                Type = "info",
                Title = "Test Message",
                User = testUser
            };

            _service.Create(notification);

            var savedNotification = _context.Notifications.FirstOrDefault(n => n.Message == "Test Message");
            Assert.NotNull(savedNotification);
            Assert.Equal("test-user", savedNotification.UserId);
        }

        [Fact]
        public void Update_WithValidNotification_UpdatesDatabase()
        {
            var testUser = new ApplicationUser { Id = "test-user", UserName = "test@example.com", FirstName = "Test", LastName = "User", Email = "test@example.com", Gender = "Other", ProfilePictureUrl = "/images/default-profile.png" };
            var notification = new Notification
            {
                Id = 1,
                UserId = "test-user",
                Message = "Original Message",
                Type = "info",
                Title = "Original Message",
                User = testUser
            };

            _context.Notifications.Add(notification);
            _context.SaveChanges();

            notification.Message = "Updated Message";
            _service.Update(notification);

            var updatedNotification = _context.Notifications.Find(1);
            Assert.Equal("Updated Message", updatedNotification.Message);
        }

        [Fact]
        public void Delete_WithValidId_RemovesFromDatabase()
        {
            var testUser = new ApplicationUser { Id = "test-user", UserName = "test@example.com", FirstName = "Test", LastName = "User", Email = "test@example.com", Gender = "Other", ProfilePictureUrl = "/images/default-profile.png" };
            var notification = new Notification
            {
                Id = 1,
                UserId = "test-user",
                Message = "Test Message",
                Type = "info",
                Title = "Test Message",
                User = testUser
            };

            _context.Notifications.Add(notification);
            _context.SaveChanges();

            _service.Delete(1);

            var deletedNotification = _context.Notifications.Find(1);
            Assert.Null(deletedNotification);
        }

        [Fact]
        public void Delete_WithInvalidId_DoesNothing()
        {
            var initialCount = _context.Notifications.Count();
            _service.Delete(999);
            var finalCount = _context.Notifications.Count();

            Assert.Equal(initialCount, finalCount);
        }
    }
} 