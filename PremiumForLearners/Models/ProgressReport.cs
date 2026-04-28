using System.ComponentModel.DataAnnotations;

namespace PremiumForLearners.Models
{
    public class ProgressReport
    {
        [Key]
        public int Id { get; set; }

        public int StudentId { get; set; }
        public Student? Student { get; set; }

        [Required]
        public string Term { get; set; } = string.Empty; // Term 1, 2, 3, 4

        [Required]
        public string SchoolYear { get; set; } = string.Empty;

        // Store subjects progress as JSON
        public string SubjectsProgressJson { get; set; } = "[]";

        public string TeacherComments { get; set; } = string.Empty;

        public string? ReportCardPath { get; set; }

        public DateTime IssuedDate { get; set; } = DateTime.Now;

        // Helper property
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public List<SubjectProgress> SubjectsProgress
        {
            get => string.IsNullOrEmpty(SubjectsProgressJson) ? new List<SubjectProgress>() : System.Text.Json.JsonSerializer.Deserialize<List<SubjectProgress>>(SubjectsProgressJson) ?? new List<SubjectProgress>();
            set => SubjectsProgressJson = System.Text.Json.JsonSerializer.Serialize(value);
        }
    }

    public class SubjectProgress
    {
        public string SubjectName { get; set; } = string.Empty;
        public int Score { get; set; }
        public string Grade { get; set; } = string.Empty; // A, B, C, D, E, F
        public string TeacherComment { get; set; } = string.Empty;
    }
}