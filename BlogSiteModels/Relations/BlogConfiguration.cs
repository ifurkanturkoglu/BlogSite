using BlogSiteModels.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSiteModels.Relations
{
    public class BlogConfiguration : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        {
            builder.Property(e => e.BlogId).HasColumnName("BlogID").ValueGeneratedOnAdd();

            builder.Property(e => e.BlogText).HasColumnName("BlogText").HasMaxLength(3000).IsRequired();

            builder.Property(e => e.BlogTitle).HasColumnName("BlogTitle").HasMaxLength(100).IsRequired();

            builder.Property(e => e.BlogDescription).HasColumnName("BlogDescription").HasMaxLength(300).IsRequired();

            builder.Property(e => e.ImageUrl).HasColumnName("ImageUrl");
        }
    }
}
