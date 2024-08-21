using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReactApp1.Server.Models;

namespace ReactApp1.Server.Data
{
    namespace ReactApp1.Server.Data
    {
        public class CustomerConfig : IEntityTypeConfiguration<Customer>
        {
            public void Configure(EntityTypeBuilder<Customer> builder)
            {
                builder.HasKey(c => c.Id);

                builder.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                builder.Property(c => c.NIP)
                    .IsRequired()
                    .HasMaxLength(20);

                builder.Property(c => c.ContactPerson)
                    .HasMaxLength(100);

                builder.Property(c => c.PhoneNumber)
                    .HasMaxLength(20);

                builder.HasMany(c => c.Interventions)
                    .WithOne(i => i.Customer)
                    .HasForeignKey(i => i.CustomerId)
                    .OnDelete(DeleteBehavior.Cascade);

                builder.HasMany(c => c.Addresses)
                    .WithOne(a => a.Customer)
                    .HasForeignKey(a => a.CustomerId)
                    .OnDelete(DeleteBehavior.Cascade);
            }
        }
    }
}
