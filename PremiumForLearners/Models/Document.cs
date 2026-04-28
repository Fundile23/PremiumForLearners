using System.ComponentModel.DataAnnotations;


namespace PremiumForLearners.Models
{
    public class Document
    {
        [Key]
        public int Id { get; set; }

        public int StudentId { get; set; }
        public Student? Student { get; set; }

        [Required]
        public string DocumentType { get; set; } = string.Empty; // Birth Certificate, Report Card, Medical Certificate, Transfer Card, ID

        public string? FilePath { get; set; } // Relative path to uploaded file

        public string? FileName { get; set; }

        public long FileSize { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.Now;

        public string VerificationStatus { get; set; } = "Pending"; // Pending, Verified, Rejected

        public string? VerificationNotes { get; set; }

        public DateTime? VerifiedAt { get; set; }

        public string? VerifiedBy { get; set; }

        public DateTime? ExpiryDate { get; set; } // For documents that expire (ID, Medical)

        public bool IsActive { get; set; } = true;
    }
}