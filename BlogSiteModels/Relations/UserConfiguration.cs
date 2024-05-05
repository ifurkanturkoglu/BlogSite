using BlogSiteModels.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace BlogSiteModels.Relations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasMany(a => a.UserBlogs).WithOne(a => a.User).HasForeignKey(a => a.UserID).OnDelete(DeleteBehavior.Cascade).IsRequired(false);

            builder.HasMany(a => a.Comments).WithOne(a => a.User).HasForeignKey(a => a.UserId).OnDelete(DeleteBehavior.NoAction).IsRequired(false);

            builder.Property(e => e.UserID).HasColumnName("UserID").ValueGeneratedOnAdd();

            builder.Property(e => e.UserName).HasColumnName("UserName").HasMaxLength(48).IsRequired();

            builder.Property(e => e.Password).HasColumnName("UserPassword").HasMaxLength(24).IsRequired();

            builder.Property(e => e.Type).HasColumnName("UserType").HasConversion<String>().IsRequired();
        }
    }
}
