using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReactApp1.Server.Models;

namespace ReactApp1.Server.Data
{
    public class InterventionConfig : IEntityTypeConfiguration<Intervention>
    {
        public void Configure(EntityTypeBuilder<Intervention> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(i => i.Date)
                .IsRequired();

            builder.HasOne(i => i.User)
                .WithMany(u => u.Interventions)
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(i => i.Customer)
                .WithMany(c => c.Interventions)
                .HasForeignKey(i => i.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(i => i.Addresses)
                .WithOne(a => a.Intervention)
                .HasForeignKey(a => a.InterventionId)
                .OnDelete(DeleteBehavior.Cascade); 
        }
    }
}
