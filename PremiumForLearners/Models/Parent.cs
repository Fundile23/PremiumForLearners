using System.ComponentModel.DataAnnotations;

namespace PremiumForLearners.Models
{
    public class Parent
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [Display(Name = "ID/Passport Number")]
        public string IdNumber { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Physical Address")]
        public string Address { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Relationship to Child")]
        public string Relationship { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? PasswordHash { get; set; }

        // Navigation property
        public ICollection<Student> Children { get; set; } = new List<Student>();
    }
}