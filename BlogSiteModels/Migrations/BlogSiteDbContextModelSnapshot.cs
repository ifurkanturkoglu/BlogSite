﻿// <auto-generated />
using System;
using BlogSiteModels.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BlogSiteModels.Migrations
{
    [DbContext(typeof(BlogSiteDbContext))]
    partial class BlogSiteDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BlogSiteModels.Models.Blog", b =>
                {
                    b.Property<int>("BlogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("BlogID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BlogId"));

                    b.Property<DateTime>("BlogAddDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("BlogAddDate");

                    b.Property<string>("BlogDescription")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)")
                        .HasColumnName("BlogDescription");

                    b.Property<string>("BlogText")
                        .IsRequired()
                        .HasMaxLength(3000)
                        .HasColumnType("nvarchar(3000)")
                        .HasColumnName("BlogText");

                    b.Property<string>("BlogTitle")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("BlogTitle");

                    b.Property<int>("DislikeCount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0)
                        .HasColumnName("DislikeCount");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("ImageUrl");

                    b.Property<int>("LikeCount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0)
                        .HasColumnName("LikeCount");

                    b.Property<int>("MyProperty")
                        .HasColumnType("int");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("BlogId");

                    b.HasIndex("UserID");

                    b.ToTable("Blogs");
                });

            modelBuilder.Entity("BlogSiteModels.Models.Comment", b =>
                {
                    b.Property<int>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("CommentId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CommentId"));

                    b.Property<int>("BlogId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CommentAddTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("CommentAddTime");

                    b.Property<string>("CommentText")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)")
                        .HasColumnName("CommentText");

                    b.Property<int?>("ParentCommentId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("CommentId");

                    b.HasIndex("BlogId");

                    b.HasIndex("ParentCommentId");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("BlogSiteModels.Models.LikeAndDislikeBlog", b =>
                {
                    b.Property<int>("LikeAndDislikeBlogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("LikeAndDislikeBlogId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LikeAndDislikeBlogId"));

                    b.Property<int>("BlogId")
                        .HasColumnType("int")
                        .HasColumnName("BlogId");

                    b.Property<bool>("IsLiked")
                        .HasColumnType("bit")
                        .HasColumnName("IsLiked");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("UserName");

                    b.HasKey("LikeAndDislikeBlogId");

                    b.HasIndex("BlogId");

                    b.ToTable("LikeAndDislikeBlogs");
                });

            modelBuilder.Entity("BlogSiteModels.Models.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("UserID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserID"));

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(24)
                        .HasColumnType("nvarchar(24)")
                        .HasColumnName("UserPassword");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("UserType");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(48)
                        .HasColumnType("nvarchar(48)")
                        .HasColumnName("UserName");

                    b.HasKey("UserID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BlogSiteModels.Models.Blog", b =>
                {
                    b.HasOne("BlogSiteModels.Models.User", "User")
                        .WithMany("UserBlogs")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("User");
                });

            modelBuilder.Entity("BlogSiteModels.Models.Comment", b =>
                {
                    b.HasOne("BlogSiteModels.Models.Blog", "Blog")
                        .WithMany("Comments")
                        .HasForeignKey("BlogId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BlogSiteModels.Models.Comment", "ParentComment")
                        .WithMany("CommentAnswers")
                        .HasForeignKey("ParentCommentId");

                    b.HasOne("BlogSiteModels.Models.User", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Blog");

                    b.Navigation("ParentComment");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BlogSiteModels.Models.LikeAndDislikeBlog", b =>
                {
                    b.HasOne("BlogSiteModels.Models.Blog", "Blog")
                        .WithMany("BlogLikeAndDislike")
                        .HasForeignKey("BlogId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Blog");
                });

            modelBuilder.Entity("BlogSiteModels.Models.Blog", b =>
                {
                    b.Navigation("BlogLikeAndDislike");

                    b.Navigation("Comments");
                });

            modelBuilder.Entity("BlogSiteModels.Models.Comment", b =>
                {
                    b.Navigation("CommentAnswers");
                });

            modelBuilder.Entity("BlogSiteModels.Models.User", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("UserBlogs");
                });
#pragma warning restore 612, 618
        }
    }
}
