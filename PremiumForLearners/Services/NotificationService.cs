using Microsoft.EntityFrameworkCore;
using PremiumForLearners.Data;
using PremiumForLearners.Models;

namespace PremiumForLearners.Services
{
    public class NotificationService
    {
        private readonly AppDbContext _context;

        public NotificationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task SendNotificationAsync(int? parentId, int? studentId, string title, string message, string type = "Info", string? link = null)
        {
            var notification = new Notification
            {
                ParentId = parentId,
                StudentId = studentId,
                Title = title,
                Message = message,
                NotificationType = type,
                Link = link,
                CreatedAt = DateTime.Now,
                IsRead = false
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Notification>> GetUnreadNotificationsAsync(int? parentId = null)
        {
            var query = _context.Notifications.AsQueryable();

            if (parentId.HasValue)
            {
                query = query.Where(n => n.ParentId == parentId);
            }

            return await query.Where(n => !n.IsRead)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}