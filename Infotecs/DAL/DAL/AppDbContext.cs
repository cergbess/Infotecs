using Infotecs.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Infotecs.DAL
{
    public class AppDbContext : DbContext
    {
        public DbSet<Value> values { get; set; }
        public DbSet<Result> results { get; set; }
        
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
