using Microsoft.EntityFrameworkCore;
using Webapp_Dieselskandal.Models;

namespace Webapp_Dieselskandal.Data
{
    public class AppDbContext : DbContext       // Kein IdentityDBContext, da nur einfaches Anmelden nötig
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Auftrag> Auftraege { get; set; }
    }
}