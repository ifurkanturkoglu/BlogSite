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
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasMany(a => a.CommentAnswers).WithOne(a => a.ParentComment).HasForeignKey(a => a.ParentCommentId).IsRequired(false);
            builder.Property(a => a.CommentId).HasColumnName("CommentId").ValueGeneratedOnAdd().IsRequired();
            builder.Property(a => a.CommentText).HasColumnName("CommentText").HasMaxLength(250).IsRequired();
            builder.Property(a => a.CommentAddTime).HasColumnName("CommentAddTime").IsRequired();
        }
    }
}
