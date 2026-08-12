using Microsoft.EntityFrameworkCore;
using PremiumForLearners.Data;
using PremiumForLearners.Models;
using PremiumForLearners.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();


builder.Services.AddScoped<FileUploadService>();

// Add DbContext with SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add session support
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(7);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add authentication cookies
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
    });

builder.Services.AddScoped<NotificationService>();

var app = builder.Build();

// Configure pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Create database on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
    // Create admin user if not exists
    if (!dbContext.Parents.Any(p => p.Email == "admin@premiumlearners.com"))
    {
        dbContext.Parents.Add(new Parent
        {
            FullName = "System Administrator",
            Email = "admin@premiumlearners.com",
            PasswordHash = "admin123", //
            PhoneNumber = "0000000000",
            IdNumber = "ADMIN001",
            Address = "System",
            Relationship = "Admin"
        });
        dbContext.SaveChanges();
    }
}

app.Run();