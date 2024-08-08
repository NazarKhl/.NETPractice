using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReactApp1.Server.Models;

namespace ReactApp1.Server.Data
{
    public class AbsenceConfig : IEntityTypeConfiguration<Absence>
    {
        public void Configure(EntityTypeBuilder<Absence> builder)
        {
            builder.HasKey(a => a.Id);

            builder.HasOne(i => i.User)
                .WithMany(u => u.Absences)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
