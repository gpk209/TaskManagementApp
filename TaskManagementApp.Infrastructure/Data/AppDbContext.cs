using Microsoft.EntityFrameworkCore;
using TaskManagementApp.Core.Entities;

namespace TaskManagementApp.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<TaskItem> Tasks { get; set; } = null!;
        public DbSet<AppUser> Users { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }

    public static class DbSeeder
    {
        public static void Seed(AppDbContext db)
        {
            if (!db.Users.Any())
            {
                db.Users.Add(new AppUser { Username = "admin", PasswordHash = "" });
            }
            if (!db.Tasks.Any())
            {
                db.Tasks.AddRange(new TaskItem[] {
                    new TaskItem { Title = "Buy groceries", Description = "Milk, eggs", DueDate = DateTime.UtcNow.AddDays(2), Priority = Priority.Medium },
                    new TaskItem { Title = "Send report", Description = "Monthly sales", DueDate = DateTime.UtcNow.AddDays(1), Priority = Priority.High }
                });
            }
            db.SaveChanges();
        }
    }
}
