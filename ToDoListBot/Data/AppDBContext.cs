using System;
using Microsoft.EntityFrameworkCore;
using ToDoListBot.Models;

namespace ToDoListBot.Data;

public class AppDBContext : DbContext
{
    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite(
            "Data Source=DailyTaskBot.db");
    }
}
