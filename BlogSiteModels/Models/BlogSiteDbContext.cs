using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BlogSiteModels.Models
{
    public partial class BlogSiteDbContext : DbContext
    {
        public BlogSiteDbContext(DbContextOptions<BlogSiteDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Blog> Blogs { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<LikeAndDislikeBlog> LikeAndDislikeBlogs { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=BlogDB;Trusted_Connection=True;Encrypt=false");
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}