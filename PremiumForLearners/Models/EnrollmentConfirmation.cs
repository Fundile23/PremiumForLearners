using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PremiumForLearners.Models
{
    public class EnrollmentConfirmation
    {
        [Key]
        public int Id { get; set; }

        public int StudentId { get; set; }
        public Student? Student { get; set; }

        [Required]
        public string DigitalSignature { get; set; } = string.Empty;

        [Required]
        public bool TermsAccepted { get; set; }

        public DateTime ConfirmedAt { get; set; } = DateTime.Now;

        public decimal RegistrationFeePaid { get; set; }

        public string? FeeReceiptPath { get; set; }

        public bool IsConfirmed { get; set; } = false;

        // Medical information
        public string? MedicalConditions { get; set; }
        public string? Allergies { get; set; }
        public string? EmergencyContactName { get; set; }
        public string? EmergencyContactPhone { get; set; }

        // Transportation
        public bool NeedsTransport { get; set; }
        public string? TransportRoute { get; set; }
    }
}