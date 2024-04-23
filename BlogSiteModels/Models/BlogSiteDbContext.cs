using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BlogSiteModels.Models;

public partial class BlogSiteDbContext : DbContext
{
    public BlogSiteDbContext(DbContextOptions<BlogSiteDbContext> options)
        : base(options)
    {
    }
    public virtual DbSet<Blog> Blogs { get; set; }
    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
       optionsBuilder.UseSqlServer("Server=.;Database=BlogDB;Trusted_Connection=True;Encrypt=false");
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>(entity =>
        {
            entity.Property(e => e.BlogId).HasColumnName("BlogID").ValueGeneratedOnAdd();

            entity.Property(e => e.BlogText).HasColumnName("BlogText").HasMaxLength(3000).IsRequired();
            
            entity.Property(e => e.BlogTitle).HasColumnName("BlogTitle").HasMaxLength(100).IsRequired();
            
            entity.Property(e => e.BlogDescription).HasColumnName("BlogDescription").HasMaxLength(300).IsRequired();

            entity.Property(e => e.ImageUrl).HasColumnName("ImageUrl");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.UserID).HasColumnName("UserID").ValueGeneratedOnAdd();

            entity.Property(e => e.UserName).HasColumnName("UserName").HasMaxLength(48).IsRequired();

            entity.Property(e => e.Password).HasColumnName("UserPassword").HasMaxLength(24).IsRequired();
        });
    }
}
