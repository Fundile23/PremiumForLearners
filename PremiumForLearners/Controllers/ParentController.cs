using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PremiumForLearners.Data;
using PremiumForLearners.Models;
using PremiumForLearners.Services;
using System.Security.Claims;

namespace PremiumForLearners.Controllers
{
    [Authorize]
    public class ParentController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly FileUploadService _fileUploadService;

        public ParentController(AppDbContext context, IWebHostEnvironment environment, FileUploadService fileUploadService)
        {
            _context = context;
            _environment = environment;
            _fileUploadService = fileUploadService;
        }

        private int GetCurrentParentId()
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(email)) return 0;
            var parent = _context.Parents.FirstOrDefault(p => p.Email == email);
            return parent?.Id ?? 0;
        }

        // Helper method to check if student has paid verified payment
        private bool HasPaid(int studentId)
        {
            return _context.Payments.Any(p => p.StudentId == studentId && p.Status == "Verified");
        }

        // GET: /Parent/Loading
        [HttpGet]
        public IActionResult Loading()
        {
            return View();
        }


        // GET: /Parent/AddStudent
        [HttpGet]
        public IActionResult AddStudent()
        {
            return View(new Student());
        }

        // POST: /Parent/AddStudent
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddStudent(Student student)
        {
            if (ModelState.IsValid)
            {
                var parentId = GetCurrentParentId();
                if (parentId == 0) return RedirectToAction("Login", "Account");

                student.ParentId = parentId;
                student.ApplicationStatus = "Draft";

                _context.Students.Add(student);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Student added successfully!";
                return RedirectToAction("StudentDetails", new { id = student.Id });
            }
            return View(student);
        }

        // GET: /Parent/StudentDetails/{id}
        [HttpGet]
        public async Task<IActionResult> StudentDetails(int id)
        {
            Console.WriteLine($"StudentDetails called with id: {id}");

            var parentId = GetCurrentParentId();
            var student = await _context.Students
                .Include(s => s.SubjectSelections)
                .Include(s => s.TransferRequests)
                .Include(s => s.Documents)
                .FirstOrDefaultAsync(s => s.Id == id && s.ParentId == parentId);

            if (student == null)
            {
                Console.WriteLine($"Student not found - id: {id}, parentId: {parentId}");
                return NotFound();
            }

            Console.WriteLine($"Student found: {student.FullName}, ID: {student.Id}");
            return View(student);
        }
        // POST: /Parent/SubmitApplication/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitApplication(int id)
        {
            var parentId = GetCurrentParentId();
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.Id == id && s.ParentId == parentId);

            if (student == null) return NotFound();

            student.ApplicationStatus = "Submitted";
            student.SubmittedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            TempData["Success"] = "Application submitted successfully!";

            return RedirectToAction("StudentDetails", new { id = id });
        }

        [HttpGet]
        public async Task<IActionResult> SubjectSelection(int id)
        {
            try
            {
                var parentId = GetCurrentParentId();
                if (parentId == 0)
                {
                    return RedirectToAction("Login", "Account");
                }

                var student = await _context.Students
                    .Include(s => s.SubjectSelections)
                    .FirstOrDefaultAsync(s => s.Id == id && s.ParentId == parentId);

                if (student == null)
                {
                    TempData["Error"] = "Student not found.";
                    return RedirectToAction("Dashboard");
                }

                // Check if student is in Grade 10-12
                int gradeNum = 0;
                int.TryParse(student.ApplyingGrade, out gradeNum);

                if (gradeNum < 10 || gradeNum > 12)
                {
                    TempData["Error"] = "Subject selection is only available for students in Grade 10, 11, or 12.";
                    return RedirectToAction("StudentDetails", new { id = id });
                }

                // Check if already submitted
                var existingSelection = await _context.SubjectSelections
                    .FirstOrDefaultAsync(s => s.StudentId == id && s.Status != "Rejected");

                if (existingSelection != null && existingSelection.Status == "Submitted")
                {
                    TempData["Info"] = "You have already submitted subject selections. Waiting for admin approval.";
                    return RedirectToAction("StudentDetails", new { id = id });
                }

                if (existingSelection != null && existingSelection.Status == "Approved")
                {
                    TempData["Success"] = "Your subject selections have been approved!";
                    return RedirectToAction("StudentDetails", new { id = id });
                }

                // Get available subjects
                var subjects = await _context.Subjects
                    .Where(s => s.Grade == student.ApplyingGrade && s.IsActive)
                    .OrderBy(s => s.IsCore ? 0 : 1)
                    .ThenBy(s => s.Name)
                    .ToListAsync();

                if (!subjects.Any())
                {
                    TempData["Warning"] = "No subjects are available for this grade yet. Please contact admin.";
                    return RedirectToAction("StudentDetails", new { id = id });
                }

                ViewBag.Subjects = subjects;
                ViewBag.Student = student;

                var selection = new SubjectSelection
                {
                    StudentId = id,
                    Grade = student.ApplyingGrade
                };

                return View(selection);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading SubjectSelection: {ex.Message}");
                TempData["Error"] = "An error occurred. Please try again.";
                return RedirectToAction("StudentDetails", new { id = id });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubjectSelection(SubjectSelection selection, string[] selectedElectives)
        {
            try
            {
                // ADD THIS DEBUGGING
                Console.WriteLine("========== POST SUBJECT SELECTION CALLED ==========");
                Console.WriteLine($"StudentId: {selection?.StudentId}");
                Console.WriteLine($"Grade: {selection?.Grade}");
                Console.WriteLine($"Selected Electives: {(selectedElectives != null ? string.Join(", ", selectedElectives) : "NULL")}");

                var parentId = GetCurrentParentId();
                Console.WriteLine($"ParentId: {parentId}");

                if (parentId == 0)
                {
                    TempData["Error"] = "Please login again.";
                    return RedirectToAction("Login", "Account");
                }

                var student = await _context.Students
                    .FirstOrDefaultAsync(s => s.Id == selection.StudentId && s.ParentId == parentId);

                if (student == null)
                {
                    TempData["Error"] = "Student not found or you don't have permission.";
                    return RedirectToAction("Dashboard");
                }

                // Validate electives
                if (selectedElectives == null || selectedElectives.Length < 3)
                {
                    TempData["Error"] = "Please select at least 3 elective subjects.";
                    return RedirectToAction("SubjectSelection", new { id = selection.StudentId });
                }

                if (selectedElectives.Length > 4)
                {
                    TempData["Error"] = "You can only select up to 4 elective subjects.";
                    return RedirectToAction("SubjectSelection", new { id = selection.StudentId });
                }

                // Check if student already has a subject selection
                var existingSelection = await _context.SubjectSelections
                    .FirstOrDefaultAsync(s => s.StudentId == selection.StudentId && s.Status != "Rejected");

                if (existingSelection != null)
                {
                    TempData["Error"] = "You have already submitted subject selections. Contact admin if you need to make changes.";
                    return RedirectToAction("StudentDetails", new { id = selection.StudentId });
                }

                // Get core subjects for this grade
                var coreSubjects = await _context.Subjects
                    .Where(s => s.Grade == student.ApplyingGrade && s.IsCore && s.IsActive)
                    .Select(s => s.Name)
                    .ToListAsync();

                // Create NEW subject selection object
                var newSelection = new SubjectSelection
                {
                    StudentId = selection.StudentId,
                    Grade = student.ApplyingGrade,
                    CoreSubjects = coreSubjects.Any() ? string.Join(", ", coreSubjects) : "Mathematics, English, Life Orientation",
                    Status = "Submitted",
                    SelectionDate = DateTime.Now,
                    AcademicYear = DateTime.Now.Year.ToString(),
                    Elective1 = selectedElectives.Length > 0 ? selectedElectives[0] : "",
                    Elective2 = selectedElectives.Length > 1 ? selectedElectives[1] : "",
                    Elective3 = selectedElectives.Length > 2 ? selectedElectives[2] : "",
                    Elective4 = selectedElectives.Length > 3 ? selectedElectives[3] : ""
                };

                _context.SubjectSelections.Add(newSelection);
                await _context.SaveChangesAsync();

                // Send notification to parent
                var parentNotification = new Notification
                {
                    ParentId = parentId,
                    StudentId = student.Id,
                    Title = "Subject Selection Submitted",
                    Message = $"Subject selection for {student.FullName} (Grade {student.ApplyingGrade}) has been submitted for review.",
                    NotificationType = "Success",
                    CreatedAt = DateTime.Now,
                    IsRead = false
                };
                _context.Notifications.Add(parentNotification);

                // Send notification to admin
                var adminNotification = new Notification
                {
                    Title = "📚 New Subject Selection",
                    Message = $"Parent {parentId} submitted subject selection for {student.FullName} (Grade {student.ApplyingGrade}). Electives: {string.Join(", ", selectedElectives)}",
                    NotificationType = "Warning",
                    CreatedAt = DateTime.Now,
                    IsRead = false,
                    Link = "/Admin/SubjectSelections"
                };
                _context.Notifications.Add(adminNotification);

                await _context.SaveChangesAsync();

                TempData["Success"] = $"Subject selection for {student.FullName} submitted successfully! Admin will review and confirm.";
                return RedirectToAction("StudentDetails", new { id = selection.StudentId });
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error in SubjectSelection POST: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                TempData["Error"] = "An error occurred while submitting subject selection. Please try again.";
                return RedirectToAction("SubjectSelection", new { id = selection.StudentId });
            }
        }
        // GET: /Parent/TransferRequest/{id}
        [HttpGet]
        public async Task<IActionResult> TransferRequest(int id)
        {
            var parentId = GetCurrentParentId();
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.Id == id && s.ParentId == parentId);

            if (student == null) return NotFound();

            ViewBag.Student = student;
            return View(new TransferRequest { StudentId = id });
        }

        // POST: /Parent/TransferRequest
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TransferRequest(TransferRequest request)
        {
            var parentId = GetCurrentParentId();
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.Id == request.StudentId && s.ParentId == parentId);

            if (student == null) return NotFound();

            // Create a NEW object instead of using the one from the form
            var transferRequest = new TransferRequest
            {
                StudentId = request.StudentId,
                FromSchool = request.FromSchool,
                ToSchool = request.ToSchool,
                Reason = request.Reason,
                ExpectedStartDate = request.ExpectedStartDate,
                RequestDate = DateTime.Now,
                Status = "Pending"
            };

            _context.TransferRequests.Add(transferRequest);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Transfer request submitted successfully!";
            return RedirectToAction("StudentDetails", new { id = request.StudentId });
        }

       

      
       

        // GET: /Parent/DocumentVault/{id}
        [HttpGet]
        public async Task<IActionResult> DocumentVault(int id)
        {
            var parentId = GetCurrentParentId();
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.Id == id && s.ParentId == parentId);

            if (student == null) return NotFound();

            var documents = await _context.Documents
                .Where(d => d.StudentId == id)
                .OrderByDescending(d => d.UploadedAt)
                .ToListAsync();

            ViewBag.Student = student;
            return View(documents);
        }

        // GET: /Parent/UploadDocument/{id}
        [HttpGet]
        public async Task<IActionResult> UploadDocument(int id)
        {
            var parentId = GetCurrentParentId();
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.Id == id && s.ParentId == parentId);

            if (student == null) return NotFound();

            ViewBag.Student = student;
            ViewBag.DocumentTypes = new List<string>
            {
                "Birth Certificate",
                "Immunization Records",
                "Proof of Address",
                "Previous Report Cards",
                "Medical Certificate",
                "ID Document"
            };

            return View();
        }

     

        // POST: /Parent/UploadDocument
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadDocument(int id, string documentType, IFormFile file)
        {
            var parentId = GetCurrentParentId();
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.Id == id && s.ParentId == parentId);

            if (student == null) return NotFound();

            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("", "Please select a file");
                ViewBag.Student = student;
                ViewBag.DocumentTypes = new List<string>
                {
                    "Birth Certificate",
                    "Immunization Records",
                    "Proof of Address",
                    "Previous Report Cards",
                    "Medical Certificate",
                    "ID Document"
                };
                return View();
            }

            try
            {
                var filePath = await _fileUploadService.UploadFileAsync(file, $"students/{id}");

                var document = new Document
                {
                    StudentId = id,
                    DocumentType = documentType,
                    FilePath = filePath,
                    FileName = file.FileName,
                    FileSize = file.Length,
                    UploadedAt = DateTime.Now,
                    VerificationStatus = "Pending"
                };

                _context.Documents.Add(document);
                await _context.SaveChangesAsync();

                TempData["Success"] = $"{documentType} uploaded successfully!";
                return RedirectToAction("DocumentVault", new { id = id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                ViewBag.Student = student;
                ViewBag.DocumentTypes = new List<string>
                {
                    "Birth Certificate",
                    "Immunization Records",
                    "Proof of Address",
                    "Previous Report Cards",
                    "Medical Certificate",
                    "ID Document"
                };
                return View();
            }
        }



        // GET: /Parent/ViewDocument/{id}
        // GET: /Parent/ViewDocument/{id}
        [HttpGet]
        public async Task<IActionResult> ViewDocument(int id)
        {
            var document = await _context.Documents
                .Include(d => d.Student)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (document == null) return NotFound();

            var parentId = GetCurrentParentId();
            if (document.Student?.ParentId != parentId)
                return Unauthorized();

            if (string.IsNullOrEmpty(document.FilePath))
                return NotFound();

            var fullPath = Path.Combine(_environment.WebRootPath, document.FilePath.TrimStart('/'));
            if (!System.IO.File.Exists(fullPath))
                return NotFound();

            var fileBytes = await System.IO.File.ReadAllBytesAsync(fullPath);
            var contentType = GetContentType(document.FilePath);

            return File(fileBytes, contentType, document.FileName);
        }

        // POST: /Parent/DeleteDocument
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDocument(int id, int studentId)
        {
            var document = await _context.Documents.FindAsync(id);
            if (document != null && !string.IsNullOrEmpty(document.FilePath))
            {
                _fileUploadService.DeleteFile(document.FilePath);
                _context.Documents.Remove(document);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Document deleted successfully!";
            }
            return RedirectToAction("DocumentVault", new { id = studentId });
        }

        private string GetContentType(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            return extension switch
            {
                ".pdf" => "application/pdf",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                _ => "application/octet-stream"
            };
        }

        // GET: /Parent/Notifications
        [HttpGet]
        public async Task<IActionResult> Notifications()
        {
            var parentId = GetCurrentParentId();
            var notifications = await _context.Notifications
                .Where(n => n.ParentId == parentId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            return View(notifications);
        }

        // POST: /Parent/MarkNotificationRead
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkNotificationRead(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Notifications");
        }

        // GET: /Parent/ReRegister/{id}
        [HttpGet]
        public async Task<IActionResult> ReRegister(int id)
        {
            var parentId = GetCurrentParentId();
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.Id == id && s.ParentId == parentId);

            if (student == null) return NotFound();

            ViewBag.Student = student;
            return View(student);
        }

        // POST: /Parent/ReRegister
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReRegister(int id, string confirmSignature)
        {
            var parentId = GetCurrentParentId();
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.Id == id && s.ParentId == parentId);

            if (student == null) return NotFound();

            if (string.IsNullOrEmpty(confirmSignature))
            {
                TempData["Error"] = "Please provide your digital signature.";
                return RedirectToAction("ReRegister", new { id = id });
            }

            int currentGrade = int.TryParse(student.ApplyingGrade, out var grade) ? grade : 0;
            string nextGrade = (currentGrade + 1).ToString();

            var newStudent = new Student
            {
                FullName = student.FullName,
                DateOfBirth = student.DateOfBirth,
                BirthCertificateNumber = student.BirthCertificateNumber,
                HomeLanguage = student.HomeLanguage,
                Citizenship = student.Citizenship,
                PreviousSchool = student.PreviousSchool,
                ApplyingGrade = nextGrade,
                SpecialNeeds = student.SpecialNeeds,
                ParentId = parentId,
                ApplicationStatus = "Submitted",
                CreatedAt = DateTime.Now,
                SubmittedAt = DateTime.Now
            };

            _context.Students.Add(newStudent);
            await _context.SaveChangesAsync();

            var notification = new Notification
            {
                ParentId = parentId,
                StudentId = newStudent.Id,
                Title = "Re-registration Confirmed",
                Message = $"{newStudent.FullName} has been re-registered for Grade {nextGrade} for the upcoming academic year.",
                NotificationType = "Success",
                CreatedAt = DateTime.Now
            };
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"{student.FullName} has been successfully re-registered for Grade {nextGrade}!";
            return RedirectToAction("StudentDetails", new { id = newStudent.Id });
        }

        // GET: /Parent/ConfirmEnrollment/{id}
        [HttpGet]
        public async Task<IActionResult> ConfirmEnrollment(int id)
        {
            try
            {
                Console.WriteLine($"ConfirmEnrollment called with id: {id}");

                var parentId = GetCurrentParentId();
                if (parentId == 0)
                {
                    TempData["Error"] = "Please login again.";
                    return RedirectToAction("Login", "Account");
                }

                var student = await _context.Students
                    .Include(s => s.SubjectSelections)
                    .Include(s => s.Documents)
                    .FirstOrDefaultAsync(s => s.Id == id && s.ParentId == parentId);

                if (student == null)
                {
                    TempData["Error"] = "Student not found.";
                    return RedirectToAction("Dashboard");
                }

                // Check if student is in Grade 10-12
                bool isHighSchool = int.TryParse(student.ApplyingGrade, out int gradeNum) && gradeNum >= 10 && gradeNum <= 12;

                // For lower grades, SubjectsVerified is automatically considered true
                var subjectsRequirementMet = isHighSchool ? student.SubjectsVerified : true;

                // Check if all requirements are met
                var documentsVerified = student.Documents.All(d => d.VerificationStatus == "Verified");
                var paymentVerified = student.PaymentVerified;

                if (!documentsVerified || !subjectsRequirementMet || !paymentVerified)
                {
                    TempData["Error"] = "Cannot confirm enrollment. Please ensure all documents are verified, and payment is confirmed.";
                    return RedirectToAction("StudentDetails", new { id = id });
                }

                // Update student status
                student.ApplicationStatus = "Enrolled";
                student.EnrollmentConfirmedAt = DateTime.Now;

                var parent = await _context.Parents.FindAsync(parentId);
                student.EnrollmentConfirmedBy = parent?.Email ?? "Parent";

                await _context.SaveChangesAsync();

                // Send notification to parent
                var notification = new Notification
                {
                    ParentId = parentId,
                    StudentId = student.Id,
                    Title = "Enrollment Confirmed! 🎉",
                    Message = $"Congratulations! {student.FullName} has been successfully enrolled for Grade {student.ApplyingGrade} at Premium For Learners.",
                    NotificationType = "Success",
                    CreatedAt = DateTime.Now,
                    IsRead = false
                };
                _context.Notifications.Add(notification);
                await _context.SaveChangesAsync();

                TempData["Success"] = $"✅ Enrollment confirmed for {student.FullName}! Welcome to Premium For Learners!";
                return RedirectToAction("StudentDetails", new { id = id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error confirming enrollment: {ex.Message}");
                TempData["Error"] = "An error occurred while confirming enrollment. Please try again.";
                return RedirectToAction("StudentDetails", new { id = id });
            }
        }

        // GET: /Parent/Payments
        [HttpGet]
        public async Task<IActionResult> Payments(int studentId = 0)
        {
            var parentId = GetCurrentParentId();

            List<Payment> payments;
            if (studentId > 0)
            {
                payments = await _context.Payments
                    .Include(p => p.Student)
                    .Where(p => p.StudentId == studentId && p.Student != null && p.Student.ParentId == parentId)
                    .OrderByDescending(p => p.PaymentDate)
                    .ToListAsync();
                ViewBag.Student = await _context.Students.FindAsync(studentId);
            }
            else
            {
                payments = await _context.Payments
                    .Include(p => p.Student)
                    .Where(p => p.Student != null && p.Student.ParentId == parentId)
                    .OrderByDescending(p => p.PaymentDate)
                    .ToListAsync();
            }

            ViewBag.FeeStructure = await _context.FeeStructures
                .Where(f => f.IsActive)
                .ToListAsync();

            return View(payments);
        }

        // GET: /Parent/MakePayment/{id}
        [HttpGet]
        public async Task<IActionResult> MakePayment(int id)
        {
            var parentId = GetCurrentParentId();
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.Id == id && s.ParentId == parentId);

            if (student == null) return NotFound();

            var feeTypes = await _context.FeeStructures
                .Where(f => f.IsActive && (f.Grade == student.ApplyingGrade || f.Grade == "All"))
                .ToListAsync();

            ViewBag.Student = student;
            ViewBag.FeeTypes = feeTypes ?? new List<FeeStructure>();
            return View(new Payment { StudentId = id, PaymentDate = DateTime.Now });
        }

       
        // POST: /Parent/MakePayment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MakePayment(Payment payment, IFormFile? receipt)
        {
            var parentId = GetCurrentParentId();
            var parent = await _context.Parents.FindAsync(parentId);
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.Id == payment.StudentId && s.ParentId == parentId);

            if (student == null) return NotFound();

            string receiptPath = null;
            if (receipt != null && receipt.Length > 0)
            {
                receiptPath = await _fileUploadService.UploadFileAsync(receipt, $"receipts/{payment.StudentId}");
            }

            // Generate unique reference
            string reference = $"PAY-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";

            // Create the payment record
            var newPayment = new Payment
            {
                StudentId = payment.StudentId,
                PaymentType = payment.PaymentType,
                Amount = payment.Amount,
                PaymentDate = payment.PaymentDate,
                PaymentMethod = payment.PaymentMethod,
                Notes = payment.Notes,
                ReceiptPath = receiptPath,
                Status = "Pending",
                CreatedAt = DateTime.Now,
                Reference = reference,
                IsVerified = false
            };

            _context.Payments.Add(newPayment);
            await _context.SaveChangesAsync();

            // ========== SEND NOTIFICATION TO ADMIN ==========

            // Create a detailed message for admin
            string adminMessage = $@"
    <div style='font-family: monospace;'>
        <strong>💰 NEW PAYMENT RECEIVED</strong><br><br>
        <strong>Parent:</strong> {parent?.FullName} ({parent?.Email})<br>
        <strong>Student:</strong> {student.FullName} (Grade {student.ApplyingGrade})<br>
        <strong>Amount:</strong> R {payment.Amount:N2}<br>
        <strong>Payment Type:</strong> {payment.PaymentType}<br>
        <strong>Payment Method:</strong> {payment.PaymentMethod}<br>
        <strong>Reference:</strong> {reference}<br>
        <strong>Date:</strong> {DateTime.Now:dd MMM yyyy, HH:mm}<br>
        <strong>Status:</strong> ⏳ Pending Verification<br><br>
        <a href='/Admin/VerifyPayments' style='background: #000; color: white; padding: 8px 16px; text-decoration: none; border-radius: 20px;'>Verify Payment →</a>
    </div>
    ";

         
           // notification for the admin (if you have a Notification model)
            var adminAlert = new Notification
            {
                Title = "💰 New Payment Received",
                Message = $"{parent?.FullName} made a payment of R{payment.Amount:N2} for {student.FullName}. Reference: {reference}",
                NotificationType = "Warning",
                CreatedAt = DateTime.Now,
                IsRead = false,
                Link = "/Admin/VerifyPayments"
            };
            _context.Notifications.Add(adminAlert);

            // Send notification to parent (confirmation)
            var parentNotification = new Notification
            {
                ParentId = parentId,
                StudentId = student.Id,
                Title = "Payment Submitted",
                Message = $"Your payment of R{payment.Amount:N2} for {payment.PaymentType} has been submitted. Reference: {reference}. Admin will verify within 24 hours.",
                NotificationType = "Info",
                CreatedAt = DateTime.Now,
                IsRead = false,
                Link = $"/Parent/Payments?studentId={student.Id}"
            };
            _context.Notifications.Add(parentNotification);

            await _context.SaveChangesAsync();

            // Store in session or TempData for success message
            TempData["Success"] = $"Payment of R{payment.Amount:N2} submitted! Reference: {reference}. Admin has been notified and will verify your payment.";

            return RedirectToAction("Payments", new { studentId = payment.StudentId });
        }
        // GET: /Parent/PaymentReceipt/{id}
        [HttpGet]
        public async Task<IActionResult> PaymentReceipt(int id)
        {
            var payment = await _context.Payments
                .Include(p => p.Student)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (payment == null) return NotFound();

            // Verify parent owns this student
            var parentId = GetCurrentParentId();
            if (payment.Student?.ParentId != parentId)
                return Unauthorized();

            return View(payment);
        }

       
        // GET: /Parent/FixStudent/{id}
        [HttpGet]
        public async Task<IActionResult> FixStudent(int id)
        {
            var parentId = GetCurrentParentId();
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return Content($"Student {id} not found");
            }

            var oldParentId = student.ParentId;
            student.ParentId = parentId;
            await _context.SaveChangesAsync();

            return Content($"Student {student.FullName} reassigned from parent {oldParentId} to parent {parentId}");
        }

        // GET: /Parent/Dashboard
        // GET: /Parent/Dashboard
        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var parentId = GetCurrentParentId();
            if (parentId == 0) return RedirectToAction("Login", "Account");

            var parent = await _context.Parents
                .Include(p => p.Children)
                .FirstOrDefaultAsync(p => p.Id == parentId);

            if (parent == null) return RedirectToAction("Login", "Account");

            var unreadCount = await _context.Notifications
                .CountAsync(n => n.ParentId == parentId && !n.IsRead);
            ViewBag.UnreadCount = unreadCount;

            var recentNotifications = await _context.Notifications
                .Where(n => n.ParentId == parentId)
                .OrderByDescending(n => n.CreatedAt)
                .Take(5)
                .ToListAsync();
            ViewBag.Notifications = recentNotifications;

            // Get all payments for this parent to check verification status
            var payments = await _context.Payments
                .Include(p => p.Student)
                .Where(p => p.Student != null && p.Student.ParentId == parentId)
                .ToListAsync();
            ViewBag.Payments = payments;

            var totalPaid = payments.Where(p => p.Status == "Verified").Sum(p => p.Amount);
            var pendingVerification = payments.Where(p => p.Status == "Pending").Sum(p => p.Amount);

            ViewBag.PaymentSummary = new
            {
                TotalPaid = totalPaid,
                PendingVerification = pendingVerification,
                Outstanding = 0
            };

            return View(parent);
        }
        // GET: /Parent/Announcements
        [HttpGet]
        public async Task<IActionResult> Announcements()
        {
            var announcements = await _context.Announcements
                .Where(a => a.ExpiresAt == null || a.ExpiresAt > DateTime.Now)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return View(announcements);
        }

   
    }
}