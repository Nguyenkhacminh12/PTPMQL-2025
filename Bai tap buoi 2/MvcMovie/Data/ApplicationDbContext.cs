using Microsoft.EntityFrameworkCore;
using MvcMovie.Models;

namespace MvcMovie.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<MvcMovie.Models.Movie> Movie { get; set; } = default!;

        public DbSet<MvcMovie.Models.Person> Person { get; set; }

        public DbSet<MvcMovie.Models.Employee> Employee { get; set; }

        public DbSet<MvcMovie.Models.HeThongPhanPhoi> HeThongPhanPhoi { get; set; }

        public DbSet<MvcMovie.Models.DaiLy> DaiLy { get; set; }
    }
}