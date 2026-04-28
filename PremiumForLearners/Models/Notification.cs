using System.ComponentModel.DataAnnotations;

namespace PremiumForLearners.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        public int? ParentId { get; set; }
        public Parent? Parent { get; set; }

        public int? StudentId { get; set; }
        public Student? Student { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Message { get; set; } = string.Empty;

        public string NotificationType { get; set; } = "Info"; // Info, Success, Warning, Error

        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string? Link { get; set; } // Link to related page
    }
}