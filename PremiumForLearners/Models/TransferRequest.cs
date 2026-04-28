using System.ComponentModel.DataAnnotations;

namespace PremiumForLearners.Models
{
    public class TransferRequest
    {
        [Key]
        public int Id { get; set; }

        public int StudentId { get; set; }
        public Student? Student { get; set; }

        [Required]
        [Display(Name = "Current School")]
        public string FromSchool { get; set; } = string.Empty;

        [Required]
        [Display(Name = "New School")]
        public string ToSchool { get; set; } = string.Empty;

        [Required]
        public string Reason { get; set; } = string.Empty; // Change of address, academic, etc.

        public DateTime RequestDate { get; set; } = DateTime.Now;

        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected, Completed

        [Display(Name = "Expected Start Date")]
        [DataType(DataType.Date)]
        public DateTime? ExpectedStartDate { get; set; }

        // Documents will be attached
        public bool HasPreviousGrades { get; set; }
        public bool HasAttendanceRecord { get; set; }
        public bool HasDisciplineRecord { get; set; }

        public string? AdminNotes { get; set; }
        public DateTime? ReviewedDate { get; set; }
    }
}