using Microsoft.EntityFrameworkCore;
using System.IO;
using DailyTaskBot.Models;

namespace DailyTaskBot.Data
{
    public class DailyTaskBotContext : DbContext
    {
        public DbSet<EmployeeDailyReport> Reports { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string dbPath = Path.Combine(folderPath, "DailyTaskBot.db");

            // ✅ This works if EF Core SQLite package is installed
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }
    }
}
