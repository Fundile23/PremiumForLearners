using System.ComponentModel.DataAnnotations;

namespace PremiumForLearners.Models
{
    public class Subject
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        [Required]
        public string Grade { get; set; } = string.Empty; // "10", "11", "12"

        public bool IsCore { get; set; } // Required subject vs elective

        public string Description { get; set; } = string.Empty;

        public string? Prerequisites { get; set; } // e.g., "60% in Grade 9 Math"

        public int Credits { get; set; } = 4;

        public string Category { get; set; } = "Elective"; // Sciences, Humanities, Commerce, etc.

        public bool IsActive { get; set; } = true;
    }
}