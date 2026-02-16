using GoogleNegotiator.Models;
using Microsoft.EntityFrameworkCore;

namespace GoogleNegotiator.Data;

public class AppDBContext: DbContext
{
    public DbSet<Replies> Replies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite(
            "Data Source=GoogleNegotiator.db");
    }
}
