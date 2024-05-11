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
    internal class LikeAndDislikeBlogConfiguration : IEntityTypeConfiguration<LikeAndDislikeBlog>
    {
        public void Configure(EntityTypeBuilder<LikeAndDislikeBlog> builder)
        {
            builder.Property(e => e.LikeAndDislikeBlogId).HasColumnName("LikeAndDislikeBlogId").ValueGeneratedOnAdd();

            builder.Property(e => e.BlogId).HasColumnName("BlogId").IsRequired();

            builder.Property(e => e.IsDisliked).HasColumnName("IsDisliked").IsRequired();

            builder.Property(e => e.IsLiked).HasColumnName("IsLiked").IsRequired();

            builder.Property(e => e.UserName).HasColumnName("UserName").IsRequired();
        }
    }
}
