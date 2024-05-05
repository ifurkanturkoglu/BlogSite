using System;
using System.Collections.Generic;
using System.Reflection;
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
    public virtual DbSet<Comment> Comments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
       optionsBuilder.UseSqlServer("Server=.;Database=BlogDB;Trusted_Connection=True;Encrypt=false");
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
