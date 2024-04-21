using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BlogSiteModels.Models;

public partial class BlogSiteDbContext : DbContext
{
    public BlogSiteDbContext()
    {
    }

    public BlogSiteDbContext(DbContextOptions<BlogSiteDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Blog> Blogs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
       optionsBuilder.UseSqlServer("Server=.;Database=BlogDB;Trusted_Connection=True;Encrypt=false");
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>(entity =>
        {
            entity.Property(e => e.BlogId).HasColumnName("BlogID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
