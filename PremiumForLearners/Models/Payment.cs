using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PremiumForLearners.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int StudentId { get; set; }

        [ForeignKey("StudentId")]
        public Student? Student { get; set; }

        [Required]
        [Display(Name = "Payment Type")]
        public string PaymentType { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Payment Date")]
        public DateTime PaymentDate { get; set; }

        [Display(Name = "Payment Method")]
        public string PaymentMethod { get; set; } = string.Empty;

        public string? Reference { get; set; }

        public string? Notes { get; set; }

        [Display(Name = "Receipt")]
        public string? ReceiptPath { get; set; }

        public string Status { get; set; } = "Pending";

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Verified At")]
        public DateTime? VerifiedAt { get; set; }

        [Display(Name = "Verified By")]
        public string? VerifiedBy { get; set; }

        [Display(Name = "Admin Notes")]
        public string? AdminNotes { get; set; }

        [Display(Name = "Is Verified")]
        public bool IsVerified { get; set; } = false;
    }
}