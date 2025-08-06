using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Fitessa.Data.Entities;
using Fitessa.Services.Interfaces;
using Fitessa.Models;
using AutoMapper;

namespace Fitessa.Web.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;

        public NotificationController(
            UserManager<ApplicationUser> userManager,
            INotificationService notificationService,
            IMapper mapper)
        {
            _userManager = userManager;
            _notificationService = notificationService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var notifications = _notificationService.GetByUser(user.Id);
            var notificationViewModels = _mapper.Map<List<NotificationViewModel>>(notifications);

            return View(notificationViewModels);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleRead(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Json(new { success = false });

            var notification = _notificationService.GetById(id);
            if (notification == null || notification.UserId != user.Id)
            {
                return Json(new { success = false, message = "Notification not found" });
            }

            return Json(new { success = true, isRead = true });
        }

        [HttpGet]
        public async Task<IActionResult> GetUnreadCount()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Json(0);

            var unreadCount = _notificationService.GetByUser(user.Id).Count();

            return Json(unreadCount);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Json(new { success = false });

            return Json(new { success = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Json(new { success = false });

            var notification = _notificationService.GetById(id);
            if (notification == null || notification.UserId != user.Id)
            {
                return Json(new { success = false, message = "Notification not found" });
            }

            _notificationService.Delete(id);

            return Json(new { success = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAll()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Json(new { success = false });

            var notifications = _notificationService.GetByUser(user.Id);
            foreach (var notification in notifications)
            {
                _notificationService.Delete(notification.Id);
            }

            return Json(new { success = true });
        }
    }
} 