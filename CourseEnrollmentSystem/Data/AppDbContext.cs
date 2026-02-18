using Microsoft.EntityFrameworkCore;
using CourseEnrollmentSystem.Models;
using System.IO;

namespace CourseEnrollmentSystem.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Course> Courses => Set<Course>();

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var dbPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "courses.db"
            );

            options.UseSqlite($"Data Source={dbPath}");
        }
    }
}
