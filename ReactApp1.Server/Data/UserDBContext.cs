using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Models;
using System.Reflection;

namespace ReactApp1.Server.Data
{
    public class UserDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Absence> Absences { get; set; }    
        public DbSet<Address> Address { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Intervention> Intervention { get; set; }
        public DbSet<MonthlyIntervention> MonthlyInterventions { get; set; }

        public UserDBContext(DbContextOptions<UserDBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.Entity<MonthlyIntervention>(entity =>
            {
                entity.HasNoKey(); 
                entity.ToView("MonthlyInterventions");
            });
        }
    }
}

