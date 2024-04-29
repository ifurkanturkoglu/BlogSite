﻿using BlogSite.Models;
using BlogSiteModels.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace BlogSite.Areas.Admin.Controllers
{

    /// <summary>
    /// /Loglama işlemlerini yap unutma!!!
    /// </summary>


    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        BlogSiteDbContext context;
        public AdminController(BlogSiteDbContext _context)
        {
            context = _context;
        }

        public IActionResult Home(LoginViewModel model)
        {
            return View(model);
        }

        
        public IActionResult AddBlog()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddBlog(BlogViewModel blog) {

            User user = context.Users.Include(a=> a.UserBlogs).Where(a=> a.UserName == User.Identity.Name).FirstOrDefault();
            if(user != null)
            {
                blog.BlogWriter = user.UserName;
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            Blog newBlog = new Blog()
            {
                BlogText = blog.BlogText,
                BlogDescription = blog.BlogDescription,
                BlogTitle = blog.BlogTitle,
                ImageUrl = blog.ImageUrl,
                UserID = user.UserID
            };

            user.UserBlogs.Add(newBlog);

            int result = context.SaveChanges();

            if(result == 0)
            {
                ViewBag.Mesaj = "Ekleme Başarısız";
                return View();
            }

            TempData["Mesaj"] = "Ekleme Başarılı";
            return RedirectToAction(nameof(AddBlog));
        }

        public IActionResult BlogsList()
        {
            List<Blog> blogList = context.Blogs.Include(a=> a.User).ThenInclude(a=> a.UserBlogs).Where(a=> a.User.UserName == User.Identity.Name).ToList();
            return View(blogList);
        }


        public IActionResult EditBlog(int id)
        {
            Blog blog = GetUserBlog(id);
            return View(blog);
        }

        [HttpPost]
        public IActionResult EditBLog(int id,Blog blog)
        {
            Blog editedBlog = GetUserBlog(id);

            if (editedBlog == null)
            {
                return RedirectToAction(nameof(BlogsList));
            }

            editedBlog.BlogTitle = blog.BlogTitle;
            editedBlog.BlogDescription = blog.BlogDescription;
            editedBlog.BlogText = blog.BlogText;

            int result = context.SaveChanges();

            return View(editedBlog);
        }

        public IActionResult DeleteBlog(int id)
        {
            Blog blog = GetUserBlog(id);

            context.Remove(blog);
            int result =  context.SaveChanges();
            
            if(result == 0)
            {
                ViewBag.Mesaj = "Silme işlemi başarısız.";
                return View("BlogsList.cshtml");
            }

            return RedirectToAction(nameof(BlogsList));
        }

        Blog GetUserBlog(int id)
        {
            return context.Blogs.Include(a => a.User).ThenInclude(a => a.UserBlogs).Where(a => a.BlogId == id).FirstOrDefault();
        }
    }
}
