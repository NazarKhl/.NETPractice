using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Models;
using System.Reflection;

namespace ReactApp1.Server.Data
{
    public class UserDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Absence> Absences { get; set; }    

        public UserDBContext(DbContextOptions<UserDBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
