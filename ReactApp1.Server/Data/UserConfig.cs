﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReactApp1.Server.Models;

namespace ReactApp1.Server.Data
{
    public class UserConfig: IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.HasMany(u => u.Absences)
            .WithOne()
            .HasForeignKey(a => a.Id)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

