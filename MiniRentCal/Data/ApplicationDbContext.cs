using Microsoft.EntityFrameworkCore;
using MiniRentCal.Models;

namespace MiniRentCal.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<UnitModel> Units { get; set; }
        public DbSet<UtilityModel> Utilities { get; set; }
        public DbSet<CalculationSession> CalculationSessions { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (var property in builder.Model.GetEntityTypes()
                         .SelectMany(t => t.GetProperties())
                         .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,2)");
            }
        }
    }
}
