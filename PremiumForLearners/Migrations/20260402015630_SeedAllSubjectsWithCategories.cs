using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PremiumForLearners.Migrations
{
    /// <inheritdoc />
    public partial class SeedAllSubjectsWithCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        -- Grade 10 Subjects
        INSERT INTO Subjects (Name, Code, Grade, IsCore, Description, Prerequisites, Credits, Category, IsActive) VALUES
        ('Home Language', 'HL10', '10', 1, 'Home Language subject focusing on language proficiency', NULL, 4, 'Languages', 1),
        ('First Additional Language', 'FAL10', '10', 1, 'First Additional Language subject', NULL, 4, 'Languages', 1),
        ('Mathematics', 'MATH10', '10', 1, 'Core Mathematics subject', 'Pass Grade 9 Mathematics', 4, 'Mathematics', 1),
        ('Mathematical Literacy', 'MLIT10', '10', 1, 'Mathematical Literacy for practical applications', NULL, 4, 'Mathematics', 1),
        ('Life Orientation', 'LO10', '10', 1, 'Life skills and personal development', NULL, 2, 'Life Skills', 1),
        ('Physical Sciences', 'PHYS10', '10', 0, 'Physics and Chemistry fundamentals', 'Minimum 60% in Grade 9 Mathematics', 4, 'Sciences', 1),
        ('Life Sciences', 'LIFE10', '10', 0, 'Biology and life processes', 'Pass Grade 9 Natural Sciences', 4, 'Sciences', 1),
        ('Accounting', 'ACC10', '10', 0, 'Basic accounting principles', 'Pass Grade 9 Mathematics', 4, 'Commerce', 1),
        ('Business Studies', 'BS10', '10', 0, 'Business principles and entrepreneurship', NULL, 4, 'Commerce', 1),
        ('Geography', 'GEO10', '10', 0, 'Physical and human geography', NULL, 4, 'Humanities', 1),
        ('History', 'HIST10', '10', 0, 'World and South African history', NULL, 4, 'Humanities', 1),
        ('Tourism', 'TOUR10', '10', 0, 'Tourism industry and travel', NULL, 4, 'Services', 1),
        ('Computer Applications Tech (CAT)', 'CAT10', '10', 0, 'Computer applications and digital literacy', NULL, 4, 'Technology', 1),
        ('Engineering Graphics & Design (EGD)', 'EGD10', '10', 0, 'Technical drawing and design', NULL, 4, 'Technology', 1),
        ('Agricultural Sciences', 'AGRI10', '10', 0, 'Agricultural principles and practices', NULL, 4, 'Sciences', 1);

        -- Grade 11 Subjects
        INSERT INTO Subjects (Name, Code, Grade, IsCore, Description, Prerequisites, Credits, Category, IsActive) VALUES
        ('Home Language', 'HL11', '11', 1, 'Home Language subject focusing on language proficiency', 'Pass Grade 10 Home Language', 4, 'Languages', 1),
        ('First Additional Language', 'FAL11', '11', 1, 'First Additional Language subject', 'Pass Grade 10 First Additional Language', 4, 'Languages', 1),
        ('Mathematics', 'MATH11', '11', 1, 'Core Mathematics subject', 'Pass Grade 10 Mathematics', 4, 'Mathematics', 1),
        ('Mathematical Literacy', 'MLIT11', '11', 1, 'Mathematical Literacy for practical applications', 'Pass Grade 10 Mathematical Literacy', 4, 'Mathematics', 1),
        ('Life Orientation', 'LO11', '11', 1, 'Life skills and personal development', NULL, 2, 'Life Skills', 1),
        ('Physical Sciences', 'PHYS11', '11', 0, 'Advanced Physics and Chemistry', 'Pass Grade 10 Physical Sciences with 50%', 4, 'Sciences', 1),
        ('Life Sciences', 'LIFE11', '11', 0, 'Advanced Biology and life processes', 'Pass Grade 10 Life Sciences with 50%', 4, 'Sciences', 1),
        ('Accounting', 'ACC11', '11', 0, 'Advanced accounting principles', 'Pass Grade 10 Accounting with 50%', 4, 'Commerce', 1),
        ('Business Studies', 'BS11', '11', 0, 'Advanced business principles', 'Pass Grade 10 Business Studies', 4, 'Commerce', 1),
        ('Geography', 'GEO11', '11', 0, 'Advanced physical and human geography', 'Pass Grade 10 Geography', 4, 'Humanities', 1),
        ('History', 'HIST11', '11', 0, 'Advanced world and South African history', 'Pass Grade 10 History', 4, 'Humanities', 1),
        ('Tourism', 'TOUR11', '11', 0, 'Advanced tourism industry', 'Pass Grade 10 Tourism', 4, 'Services', 1),
        ('Computer Applications Tech (CAT)', 'CAT11', '11', 0, 'Advanced computer applications', 'Pass Grade 10 CAT', 4, 'Technology', 1),
        ('Engineering Graphics & Design (EGD)', 'EGD11', '11', 0, 'Advanced technical drawing and design', 'Pass Grade 10 EGD', 4, 'Technology', 1),
        ('Agricultural Sciences', 'AGRI11', '11', 0, 'Advanced agricultural principles', 'Pass Grade 10 Agricultural Sciences', 4, 'Sciences', 1);

        -- Grade 12 Subjects
        INSERT INTO Subjects (Name, Code, Grade, IsCore, Description, Prerequisites, Credits, Category, IsActive) VALUES
        ('Home Language', 'HL12', '12', 1, 'Home Language subject focusing on language proficiency', 'Pass Grade 11 Home Language', 4, 'Languages', 1),
        ('First Additional Language', 'FAL12', '12', 1, 'First Additional Language subject', 'Pass Grade 11 First Additional Language', 4, 'Languages', 1),
        ('Mathematics', 'MATH12', '12', 1, 'Core Mathematics subject', 'Pass Grade 11 Mathematics', 4, 'Mathematics', 1),
        ('Mathematical Literacy', 'MLIT12', '12', 1, 'Mathematical Literacy for practical applications', 'Pass Grade 11 Mathematical Literacy', 4, 'Mathematics', 1),
        ('Life Orientation', 'LO12', '12', 1, 'Life skills and personal development', NULL, 2, 'Life Skills', 1),
        ('Physical Sciences', 'PHYS12', '12', 0, 'Advanced Physics and Chemistry for final exams', 'Pass Grade 11 Physical Sciences with 50%', 4, 'Sciences', 1),
        ('Life Sciences', 'LIFE12', '12', 0, 'Advanced Biology for final exams', 'Pass Grade 11 Life Sciences with 50%', 4, 'Sciences', 1),
        ('Accounting', 'ACC12', '12', 0, 'Advanced accounting for final exams', 'Pass Grade 11 Accounting with 50%', 4, 'Commerce', 1),
        ('Business Studies', 'BS12', '12', 0, 'Advanced business for final exams', 'Pass Grade 11 Business Studies', 4, 'Commerce', 1),
        ('Geography', 'GEO12', '12', 0, 'Advanced geography for final exams', 'Pass Grade 11 Geography', 4, 'Humanities', 1),
        ('History', 'HIST12', '12', 0, 'Advanced history for final exams', 'Pass Grade 11 History', 4, 'Humanities', 1),
        ('Tourism', 'TOUR12', '12', 0, 'Advanced tourism for final exams', 'Pass Grade 11 Tourism', 4, 'Services', 1),
        ('Computer Applications Tech (CAT)', 'CAT12', '12', 0, 'Advanced CAT for final exams', 'Pass Grade 11 CAT', 4, 'Technology', 1),
        ('Engineering Graphics & Design (EGD)', 'EGD12', '12', 0, 'Advanced EGD for final exams', 'Pass Grade 11 EGD', 4, 'Technology', 1),
        ('Agricultural Sciences', 'AGRI12', '12', 0, 'Advanced agricultural for final exams', 'Pass Grade 11 Agricultural Sciences', 4, 'Sciences', 1);
    ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        DELETE FROM Subjects WHERE Grade IN ('10', '11', '12');
    ");
        }
    }
}
