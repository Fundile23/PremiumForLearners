using Microsoft.EntityFrameworkCore;
using PremiumForLearners.Models;

namespace PremiumForLearners.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Core DbSets - KEEP ONLY THESE
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<SubjectSelection> SubjectSelections { get; set; }
        public DbSet<TransferRequest> TransferRequests { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<FeeStructure> FeeStructures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Student - Parent Relationship
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Parent)
                .WithMany(p => p.Children)
                .HasForeignKey(s => s.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Student - SubjectSelection Relationship
            modelBuilder.Entity<SubjectSelection>()
                .HasOne(ss => ss.Student)
                .WithMany(s => s.SubjectSelections)
                .HasForeignKey(ss => ss.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Student - TransferRequest Relationship
            modelBuilder.Entity<TransferRequest>()
                .HasOne(tr => tr.Student)
                .WithMany(s => s.TransferRequests)
                .HasForeignKey(tr => tr.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Student - Document Relationship
            modelBuilder.Entity<Document>()
                .HasOne(d => d.Student)
                .WithMany(s => s.Documents)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Payment - Student Relationship
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Student)
                .WithMany()
                .HasForeignKey(p => p.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Notification - Parent Relationship
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Parent)
                .WithMany()
                .HasForeignKey(n => n.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Notification - Student Relationship (optional)
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Student)
                .WithMany()
                .HasForeignKey(n => n.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            // FeeStructure configuration
            modelBuilder.Entity<FeeStructure>()
                .Property(f => f.Amount)
                .HasPrecision(18, 2);

            // Payment configuration
            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2);
        }
    }
}