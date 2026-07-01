using _00_TaskManager.Models;
using Microsoft.EntityFrameworkCore;

namespace _00_TaskManager.Utils
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<TaskItem> TaskItems { get; set; }
    }
}
