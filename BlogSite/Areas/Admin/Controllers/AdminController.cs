using BlogSiteModels.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace BlogSite.Areas.Admin.Controllers
{

    /// <summary>
    /// /Loglama işlemlerini yap unutma!!!
    /// </summary>


    [Area("Admin")]
    public class AdminController : Controller
    {
        BlogSiteDbContext context;
        public AdminController(BlogSiteDbContext _context)
        {
            context = _context;
        }

        public IActionResult Home()
        {
            return View();
        }

        
        public IActionResult AddBlog()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddBlog(Blog blog) {

            if(!ModelState.IsValid)
            {
                return View();
            }


            Blog newBlog = new Blog()
            {
                BlogText = blog.BlogText,
                BlogDescription = blog.BlogDescription,
                BlogTitle = blog.BlogTitle,
                ImageUrl = blog.ImageUrl                
            };
            context.Add(newBlog);
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
            List<Blog> blogList = context.Blogs.ToList();
            return View(blogList);
        }


        public IActionResult EditBlog(int id)
        {
            Blog blog = context.Blogs.Where(a=> a.BlogId==id).FirstOrDefault();
            return View(blog);
        }

        [HttpPost]
        public IActionResult EditBLog(int id,Blog blog)
        {
            Blog editedBlog = context.Blogs.Where(a => a.BlogId == id).FirstOrDefault();

            if(editedBlog == null)
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
            Blog blog = context.Blogs.Where(a=> a.BlogId == id).FirstOrDefault();

            context.Remove(blog);
            int result =  context.SaveChanges();
            
            if(result == 0)
            {
                ViewBag.Mesaj = "Silme işlemi başarısız.";
                return View("BlogsList.cshtml");
            }

            return RedirectToAction(nameof(BlogsList));
        }
    }
}
