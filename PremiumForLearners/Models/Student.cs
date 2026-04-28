using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace PremiumForLearners.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Display(Name = "Birth Certificate Number")]
        public string BirthCertificateNumber { get; set; } = string.Empty;

        [Display(Name = "Home Language")]
        public string HomeLanguage { get; set; } = "English";

        public string Citizenship { get; set; } = "South African";

        [Display(Name = "Previous School")]
        public string? PreviousSchool { get; set; }

        [Required]
        [Display(Name = "Grade Applying For")]
        public string ApplyingGrade { get; set; } = string.Empty;

        [Display(Name = "Special Needs/Medical Conditions")]
        public string? SpecialNeeds { get; set; }

        // Foreign key
        public int ParentId { get; set; }
        public Parent? Parent { get; set; }

        // Status
       
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? SubmittedAt { get; set; }

        // Age calculator
        public int Age => DateTime.Today.Year - DateOfBirth.Year -
            (DateTime.Today.DayOfYear < DateOfBirth.DayOfYear ? 1 : 0);

        // Navigation properties
        public ICollection<SubjectSelection> SubjectSelections { get; set; } = new List<SubjectSelection>();
        public ICollection<TransferRequest> TransferRequests { get; set; } = new List<TransferRequest>();
        
        public ICollection<Document> Documents { get; set; } = new List<Document>();


        public string ApplicationStatus { get; set; } = "Draft"; // Draft, Submitted, DocumentsVerified, SubjectsVerified, PaymentVerified, Enrolled

        // Add these new properties
        public bool DocumentsVerified { get; set; } = false;
        public bool SubjectsVerified { get; set; } = false;
        public bool PaymentVerified { get; set; } = false;
        public DateTime? EnrollmentConfirmedAt { get; set; }
        public string? EnrollmentConfirmedBy { get; set; }

    }



}