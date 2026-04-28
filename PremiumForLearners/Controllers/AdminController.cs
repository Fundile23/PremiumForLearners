using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PremiumForLearners.Data;
using PremiumForLearners.Models;
using System.Text;



namespace PremiumForLearners.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public AdminController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;  // Add this line
        }

        // GET: /Admin/Payments
        [HttpGet]
        public async Task<IActionResult> Payments(string status = "Pending")
        {
            var payments = await _context.Payments
                .Include(p => p.Student)
                .Where(p => p.Status == status)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();

            ViewBag.CurrentStatus = status;
            return View(payments);
        }

       

        // GET: /Admin/Announcements
        [HttpGet]
        public async Task<IActionResult> Announcements()
        {
            var announcements = await _context.Announcements
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
            return View(announcements);
        }

        // GET: /Admin/Announcements/Create
        [HttpGet]
        public IActionResult CreateAnnouncement()
        {
            return View();
        }

        // POST: /Admin/Announcements/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAnnouncement(Announcement announcement)
        {
            if (ModelState.IsValid)
            {
                announcement.CreatedAt = DateTime.Now;
                _context.Announcements.Add(announcement);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Announcement published!";
                return RedirectToAction("Announcements");
            }
            return View(announcement);
        }

     


        // GET: /Admin/Notifications
        [HttpGet]
        public async Task<IActionResult> Notifications()
        {
            var notifications = await _context.Notifications
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            return View(notifications);


        }

        // POST: /Admin/MarkNotificationRead
        [HttpPost]
        public async Task<IActionResult> MarkNotificationRead(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification != null)
            {
                notification.IsRead = true;
                await _context.SaveChangesAsync();
            }
            return Ok();
        }

        // GET: /Admin/Announcements/Delete/{id}
        [HttpGet]
        public async Task<IActionResult> DeleteAnnouncement(int id)
        {
            var announcement = await _context.Announcements.FindAsync(id);
            if (announcement == null) return NotFound();
            return View(announcement);
        }

        // POST: /Admin/Announcements/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAnnouncementConfirmed(int id)
        {
            var announcement = await _context.Announcements.FindAsync(id);
            if (announcement != null)
            {
                _context.Announcements.Remove(announcement);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Announcement deleted!";
            }
            return RedirectToAction("Announcements");
        }

        // GET: /Admin/Dashboard
        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            ViewBag.TotalApplications = await _context.Students.CountAsync();
            ViewBag.PendingApplications = await _context.Students.CountAsync(s => s.ApplicationStatus == "Submitted");
            ViewBag.ApprovedApplications = await _context.Students.CountAsync(s => s.ApplicationStatus == "Approved");
            ViewBag.EnrolledStudents = await _context.Students.CountAsync(s => s.ApplicationStatus == "Enrolled");
            ViewBag.RejectedApplications = await _context.Students.CountAsync(s => s.ApplicationStatus == "Rejected");
            ViewBag.PendingTransfers = await _context.TransferRequests.CountAsync(t => t.Status == "Pending");
            ViewBag.PendingPayments = await _context.Payments.CountAsync(p => p.Status == "Pending");

            // In AdminController.cs - Dashboard action
            ViewBag.UnreadNotifications = await _context.Notifications.CountAsync(n => !n.IsRead);
            

            // Get recent applications (top 10 most recent)
            var recentApplications = await _context.Students
                .Include(s => s.Parent)
                .Where(s => s.ApplicationStatus != "Draft")
                .OrderByDescending(s => s.SubmittedAt ?? s.CreatedAt)
                .Take(10)
                .ToListAsync();

            // Get pending payments list
            var pendingPayments = await _context.Payments
                .Include(p => p.Student)
                .Where(p => p.Status == "Pending")
                .OrderByDescending(p => p.PaymentDate)
                .Take(10)
                .ToListAsync();

            ViewBag.PendingPaymentsList = pendingPayments;

            return View(recentApplications);

        
        }

        [HttpGet]
        public async Task<IActionResult> VerifyPayments()
        {
            var payments = await _context.Payments
                .Include(p => p.Student)
                .Where(p => p.Status == "Pending")
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
            return View(payments);
        }

     

      

        // GET: /Admin/Applications
        [HttpGet]
        public async Task<IActionResult> Applications(string status = "Submitted")
        {
            var applications = await _context.Students
                .Include(s => s.Parent)
                .Include(s => s.SubjectSelections)
                .Where(s => s.ApplicationStatus == status)
                .ToListAsync();

            ViewBag.CurrentStatus = status;
            return View(applications);
        }

        // GET: /Admin/SubjectSelections - Only show for Grades 10-12
        // GET: /Admin/SubjectSelections - ONLY for Grades 10, 11, 12
        [HttpGet]
        public async Task<IActionResult> SubjectSelections()
        {
            var selections = await _context.SubjectSelections
                .Include(s => s.Student)
                .Where(s => s.Status == "Submitted")
                .ToListAsync();

            // Filter to ONLY show subject selections for students in Grades 10, 11, 12
            var filteredSelections = selections
                .Where(s =>
                {
                    bool isValidGrade = int.TryParse(s.Grade, out int gradeNum);
                    return isValidGrade && gradeNum >= 10 && gradeNum <= 12;
                })
                .ToList();

            return View(filteredSelections);
        }
        // GET: /Admin/ReviewApplication/{id}
        [HttpGet]
        public async Task<IActionResult> ReviewApplication(int id)
        {
            var student = await _context.Students
                .Include(s => s.Parent)
                .Include(s => s.SubjectSelections)
                .Include(s => s.TransferRequests)
                .Include(s => s.Documents)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null) return NotFound();

            return View(student);
        }

        // POST: /Admin/ApproveApplication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveApplication(int studentId)
        {
            var student = await _context.Students.FindAsync(studentId);
            if (student != null)
            {
                // Check if student is in Grade 10-12
                bool isHighSchool = int.TryParse(student.ApplyingGrade, out int gradeNum) && gradeNum >= 10 && gradeNum <= 12;

                // For lower grades, automatically mark SubjectsVerified as true
                if (!isHighSchool)
                {
                    student.SubjectsVerified = true;
                }

                student.ApplicationStatus = "Approved";
                await _context.SaveChangesAsync();
                TempData["Success"] = "Application approved!";
            }
            return RedirectToAction("Applications");
        }
        // POST: /Admin/RejectApplication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectApplication(int studentId, string rejectionReason)
        {
            var student = await _context.Students.FindAsync(studentId);
            if (student != null)
            {
                student.ApplicationStatus = "Rejected";
                await _context.SaveChangesAsync();
                TempData["Success"] = "Application rejected.";
            }
            return RedirectToAction("Applications");
        }

        // GET: /Admin/Transfers
        [HttpGet]
        public async Task<IActionResult> Transfers(string status = "Pending")
        {
            var transfers = await _context.TransferRequests
                .Include(t => t.Student)
                .Where(t => t.Status == status)
                .ToListAsync();

            ViewBag.CurrentStatus = status;
            return View(transfers);
        }

        // POST: /Admin/ApproveTransfer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveTransfer(int transferId)
        {
            var transfer = await _context.TransferRequests.FindAsync(transferId);
            if (transfer != null)
            {
                transfer.Status = "Approved";
                transfer.ReviewedDate = DateTime.Now;
                await _context.SaveChangesAsync();
                TempData["Success"] = "Transfer approved!";
            }
            return RedirectToAction("Transfers");
        }

        // GET: /Admin/SubjectSelections


        // POST: /Admin/ApproveSubjectSelection
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveSubjectSelection(int selectionId)
        {
            var selection = await _context.SubjectSelections
                .Include(s => s.Student)
                .FirstOrDefaultAsync(s => s.Id == selectionId);

            if (selection != null)
            {
                // Check if student is in Grade 10-12 before approving
                bool isValidGrade = int.TryParse(selection.Grade, out int gradeNum);
                if (isValidGrade && gradeNum >= 10 && gradeNum <= 12)
                {
                    selection.Status = "Approved";
                    selection.ReviewedDate = DateTime.Now;

                    // Also update the student's SubjectsVerified flag
                    var student = await _context.Students.FindAsync(selection.StudentId);
                    if (student != null)
                    {
                        student.SubjectsVerified = true;
                    }

                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Subject selection approved!";
                }
                else
                {
                    TempData["Error"] = "Subject selection is only available for Grades 10-12.";
                }
            }

            return RedirectToAction("SubjectSelections");
        }



        // Verify Documents
        [HttpPost]
        public async Task<IActionResult> VerifyDocuments(int id)
        {
            var student = await _context.Students
                .Include(s => s.Documents)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student != null)
            {
                foreach (var doc in student.Documents)
                {
                    doc.VerificationStatus = "Verified";
                }
                student.DocumentsVerified = true;
                await _context.SaveChangesAsync();

                // Notify parent
                await CreateNotification(student.ParentId, student.Id,
                    "Documents Verified",
                    $"All documents for {student.FullName} have been verified.",
                    "Success");

                TempData["Success"] = "Documents verified successfully!";
            }

            return RedirectToAction("StudentDetails", new { id = id });
        }

        // Verify Subjects
        [HttpPost]
        public async Task<IActionResult> VerifySubjects(int id)
        {
            var student = await _context.Students
                .Include(s => s.SubjectSelections)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student != null)
            {
                var subjectSelection = student.SubjectSelections
                    .OrderByDescending(s => s.SelectionDate)
                    .FirstOrDefault();

                if (subjectSelection != null)
                {
                    subjectSelection.Status = "Approved";
                    student.SubjectsVerified = true;
                    await _context.SaveChangesAsync();

                    // Notify parent
                    await CreateNotification(student.ParentId, student.Id,
                        "Subjects Approved",
                        $"The subject selection for {student.FullName} has been approved.",
                        "Success");

                    TempData["Success"] = "Subjects verified successfully!";
                }
                else
                {
                    TempData["Error"] = "No subject selection found for this student.";
                }
            }

            return RedirectToAction("StudentDetails", new { id = id });
        }


        // Verify Documents for a student
        [HttpPost]
        public async Task<IActionResult> VerifyDocument(int id)
        {
            try
            {
                var student = await _context.Students
                    .Include(s => s.Documents)
                    .FirstOrDefaultAsync(s => s.Id == id);

                if (student == null)
                {
                    TempData["Error"] = "Student not found.";
                    return RedirectToAction("Applications");
                }

                // Verify ALL documents for this student
                foreach (var doc in student.Documents)
                {
                    doc.VerificationStatus = "Verified";
                }

                // Check if ALL documents are now verified (they should be after the loop)
                var allDocumentsVerified = student.Documents.All(d => d.VerificationStatus == "Verified");

                if (allDocumentsVerified)
                {
                    student.DocumentsVerified = true;

                    // Update application status
                    if (student.ApplicationStatus == "Submitted")
                    {
                        student.ApplicationStatus = "DocumentsVerified";
                    }
                }

                await _context.SaveChangesAsync();

                // Notify parent
                var parentNotification = new Notification
                {
                    ParentId = student.ParentId,
                    StudentId = student.Id,
                    Title = "Documents Verified ✅",
                    Message = $"All documents for {student.FullName} have been verified by admin.",
                    NotificationType = "Success",
                    CreatedAt = DateTime.Now,
                    IsRead = false
                };
                _context.Notifications.Add(parentNotification);
                await _context.SaveChangesAsync();

                TempData["Success"] = $"All documents for {student.FullName} have been verified!";
                return RedirectToAction("ReviewApplication", new { id = id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error verifying documents: {ex.Message}");
                TempData["Error"] = "An error occurred while verifying documents.";
                return RedirectToAction("Applications");
            }
        }

        [HttpPost]
        public async Task<IActionResult> VerifySingleDocument(int documentId, int studentId)
        {
            try
            {
                var document = await _context.Documents.FindAsync(documentId);
                if (document != null)
                {
                    document.VerificationStatus = "Verified";
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Document verified successfully!";
                }

                // Check if all documents are now verified
                var student = await _context.Students
                    .Include(s => s.Documents)
                    .FirstOrDefaultAsync(s => s.Id == studentId);

                if (student != null && student.Documents.All(d => d.VerificationStatus == "Verified"))
                {
                    student.DocumentsVerified = true;
                    if (student.ApplicationStatus == "Submitted")
                    {
                        student.ApplicationStatus = "DocumentsVerified";
                    }
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction("ReviewApplication", new { id = studentId });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error verifying document: {ex.Message}");
                TempData["Error"] = "An error occurred while verifying the document.";
                return RedirectToAction("ReviewApplication", new { id = studentId });
            }
        }

        // GET: /Admin/ViewDocument/{id}
        [HttpGet]
        public async Task<IActionResult> ViewDocument(int id)
        {
            var document = await _context.Documents
                .Include(d => d.Student)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (document == null) return NotFound();

            // Admin can view any document (no parent check)
            if (string.IsNullOrEmpty(document.FilePath))
                return NotFound();

            var fullPath = Path.Combine(_environment.WebRootPath, document.FilePath.TrimStart('/'));
            if (!System.IO.File.Exists(fullPath))
                return NotFound();

            var fileBytes = await System.IO.File.ReadAllBytesAsync(fullPath);
            var contentType = GetContentType(document.FilePath);

            return File(fileBytes, contentType, document.FileName);
        }

        private string GetContentType(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            return extension switch
            {
                ".pdf" => "application/pdf",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".doc" or ".docx" => "application/msword",
                ".xls" or ".xlsx" => "application/vnd.ms-excel",
                _ => "application/octet-stream"
            };
        }

        // Verify Subjects for a student
        [HttpPost]
        public async Task<IActionResult> VerifySubject(int id)
        {
            try
            {
                var student = await _context.Students
                    .Include(s => s.SubjectSelections)
                    .FirstOrDefaultAsync(s => s.Id == id);

                if (student == null)
                {
                    TempData["Error"] = "Student not found.";
                    return RedirectToAction("ReviewApplication", new { id = id });
                }

                // Get the latest subject selection
                var subjectSelection = student.SubjectSelections
                    .OrderByDescending(s => s.SelectionDate)
                    .FirstOrDefault();

                if (subjectSelection != null)
                {
                    subjectSelection.Status = "Approved";
                    student.SubjectsVerified = true;

                    // Update application status
                    if (student.ApplicationStatus == "DocumentsVerified")
                    {
                        student.ApplicationStatus = "SubjectsVerified";
                    }
                    else if (student.ApplicationStatus == "Submitted")
                    {
                        student.ApplicationStatus = "SubjectsVerified";
                    }

                    await _context.SaveChangesAsync();

                    // Notify parent
                    var parentNotification = new Notification
                    {
                        ParentId = student.ParentId,
                        StudentId = student.Id,
                        Title = "Subjects Approved ✅",
                        Message = $"The subject selection for {student.FullName} has been approved by admin.",
                        NotificationType = "Success",
                        CreatedAt = DateTime.Now,
                        IsRead = false
                    };
                    _context.Notifications.Add(parentNotification);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = $"Subject selection for {student.FullName} has been approved!";
                }
                else
                {
                    TempData["Error"] = "No subject selection found for this student.";
                }

                return RedirectToAction("ReviewApplication", new { id = id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error verifying subjects: {ex.Message}");
                TempData["Error"] = "An error occurred while verifying subjects.";
                return RedirectToAction("ReviewApplication", new { id = id });
            }
        }

        // Verify Payment - Uses int id for student
        [HttpPost]
        public async Task<IActionResult> VerifyPayment(int paymentId, int id, bool approve, string notes)
        {
            try
            {
                var payment = await _context.Payments.FindAsync(paymentId);
                if (payment == null)
                {
                    TempData["Error"] = "Payment not found.";
                    return RedirectToAction("VerifyPayments");
                }

                var student = await _context.Students.FindAsync(id);
                if (student == null)
                {
                    TempData["Error"] = "Student not found.";
                    return RedirectToAction("VerifyPayments");
                }

                if (approve)
                {
                    payment.Status = "Verified";
                    payment.VerifiedAt = DateTime.Now;
                    payment.VerifiedBy = User.Identity?.Name ?? "Admin";
                    payment.AdminNotes = notes;

                    // Mark student payment as verified
                    student.PaymentVerified = true;

                    TempData["Success"] = $"Payment of R{payment.Amount:N2} for {student.FullName} has been verified!";
                }
                else
                {
                    payment.Status = "Rejected";
                    payment.AdminNotes = notes;
                    TempData["Error"] = $"Payment for {student.FullName} has been rejected: {notes}";
                }

              

                // Notify parent
                var notification = new Notification
                {
                    ParentId = student.ParentId,
                    StudentId = student.Id,
                    Title = approve ? "Payment Verified ✅" : "Payment Rejected ❌",
                    Message = approve ?
                        $"Your payment of R{payment.Amount:N2} (Ref: {payment.Reference}) has been verified." :
                        $"Your payment of R{payment.Amount:N2} (Ref: {payment.Reference}) was rejected. Reason: {notes}",
                    NotificationType = approve ? "Success" : "Error",
                    CreatedAt = DateTime.Now,
                    IsRead = false
                };
                _context.Notifications.Add(notification);
                await _context.SaveChangesAsync();

                return RedirectToAction("VerifyPayments", new { status = "Pending" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error verifying payment: {ex.Message}");
                TempData["Error"] = "An error occurred while verifying payment.";
                return RedirectToAction("VerifyPayments");
            }
        }
        private async Task CreateNotification(int parentId, int studentId, string title, string message, string type)
        {
            var notification = new Notification
            {
                ParentId = parentId,
                StudentId = studentId,
                Title = title,
                Message = message,
                NotificationType = type,
                CreatedAt = DateTime.Now,
                IsRead = false
            };
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }
    }
}