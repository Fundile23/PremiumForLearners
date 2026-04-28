using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PremiumForLearners.Data;
using PremiumForLearners.Models;
using System.Security.Claims;

namespace PremiumForLearners.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Parent parent, string password)
        {
            if (ModelState.IsValid)
            {
                // Check if email already exists
                var existing = await _context.Parents.FirstOrDefaultAsync(p => p.Email == parent.Email);
                if (existing != null)
                {
                    ModelState.AddModelError("Email", "Email already registered");
                    return View(parent);
                }

                // Simple password hash (in production, use proper hashing)
                parent.PasswordHash = password; // TEMP - use BCrypt in real app

                _context.Parents.Add(parent);
                await _context.SaveChangesAsync();

                // Auto login after registration - always as Parent role
                await LoginUser(parent.Email, parent.FullName, "Parent");

                return RedirectToAction("Dashboard", "Parent");
            }
            return View(parent);
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login - FIXED VERSION
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            var parent = await _context.Parents.FirstOrDefaultAsync(p => p.Email == email);

            // TEMP: Simple password check (use proper hashing in production)
            if (parent != null && parent.PasswordHash == password)
            {
                // Determine role based on Relationship field
                string role = parent.Relationship == "Admin" ? "Admin" : "Parent";

                await LoginUser(parent.Email, parent.FullName, role);

                // Redirect based on role
                if (role == "Admin")
                {
                    return RedirectToAction("Dashboard", "Admin");
                }
                else
                {
                    return RedirectToAction("Dashboard", "Parent");
                }
            }

            ViewBag.Error = "Invalid email or password";
            return View();
        }

        // GET: /Account/Logout
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Index", "Home");
        }

        // UPDATED: Now accepts role parameter
        private async Task LoginUser(string email, string name, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, email),
                new Claim(ClaimTypes.GivenName, name),
                new Claim(ClaimTypes.Role, role) // Now uses the passed role
            };

            var identity = new ClaimsIdentity(claims, "CookieAuth");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("CookieAuth", principal);
        }

        //  for handling unauthorized access
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}