// Models/FeeStructure.cs
using System.ComponentModel.DataAnnotations;

namespace PremiumForLearners.Models
{
    public class FeeStructure
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FeeType { get; set; } = string.Empty; // Registration, School Fees, Uniform, Transport, Extramural

        [Required]
        public string Grade { get; set; } = string.Empty; // R, 1-12, or All

        [Required]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime EffectiveFrom { get; set; } = DateTime.Now;

        public DateTime? EffectiveTo { get; set; }

        public string? PaymentFrequency { get; set; } // One-time, Monthly, Termly, Annually
    }
}