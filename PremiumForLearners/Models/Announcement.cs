using System.ComponentModel.DataAnnotations;

namespace PremiumForLearners.Models
{
    public class Announcement
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        public string? TargetAudience { get; set; } = "All"; // All, Parents, Students, Staff

        public bool IsUrgent { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? ExpiresAt { get; set; }

        public string? CreatedBy { get; set; }
    }
}