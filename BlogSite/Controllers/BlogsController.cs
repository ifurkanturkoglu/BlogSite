using BlogSiteModels.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlogSite.Controllers
{
    public class BlogsController : Controller
    {
        BlogSiteDbContext context;
        public BlogsController(BlogSiteDbContext _context) { 
            context = _context;
        }

        public IActionResult GetBlog(int id)
        {
            try
            {
                Blog blog = context.Blogs.FirstOrDefault(a => a.BlogId == id);
                return View(blog);
            }
            catch
            {
                return RedirectToAction(nameof(BlogsList));
            }
            
        }
        //DTO yapılacak
        public IActionResult BlogsList()
        {
            List<Blog> blogs = context.Blogs.ToList();
            return View(blogs);
        }
    }
}
