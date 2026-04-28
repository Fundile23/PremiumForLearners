using System.ComponentModel.DataAnnotations;

namespace PremiumForLearners.Models
{
    public class SubjectSelection
    {
        [Key]
        public int Id { get; set; }

        public int StudentId { get; set; }
        public Student? Student { get; set; }

        public string AcademicYear { get; set; } = DateTime.Now.Year.ToString();

        public string Grade { get; set; } = string.Empty; // 10, 11, 12

        public DateTime SelectionDate { get; set; } = DateTime.Now;

        public string Status { get; set; } = "Draft"; // Draft, Submitted, Approved, Rejected

        // Core Subjects (selected automatically based on grade)
        public string CoreSubjects { get; set; } = string.Empty; // Store as JSON or comma-separated

        // Electives - student chooses 3-4
        public string Elective1 { get; set; } = string.Empty;
        public string Elective2 { get; set; } = string.Empty;
        public string Elective3 { get; set; } = string.Empty;
        public string? Elective4 { get; set; }

        // Backup choices in case first choices are full
        public string? BackupElective1 { get; set; }
        public string? BackupElective2 { get; set; }

        public string? CounselorComments { get; set; }
        public DateTime? ReviewedDate { get; set; }
    }
}