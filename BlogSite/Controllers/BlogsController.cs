using BlogSite.Models;
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
            //route sadece id değil ayrıca kullanıcı adını da aalabilir. Ona bi bak
            try
            {
                BlogViewModel blog = context.Blogs.Where(a => a.UserID == id).Select(b => new BlogViewModel
                {
                    BlogTitle = b.BlogTitle,
                    BlogDescription = b.BlogDescription,
                    BlogText = b.BlogText,
                    BlogWriter = b.User.UserName,
                    ImageUrl = b.ImageUrl,
                }).FirstOrDefault();
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
            List<BlogViewModel> blogs = context.Blogs.Select(a => new BlogViewModel
            {
                BlogId = a.BlogId,
                BlogTitle = a.BlogTitle,
                BlogDescription = a.BlogDescription,
                BlogText = a.BlogText,
                ImageUrl = a.ImageUrl,
                BlogWriter = a.User.UserName
            }).ToList();
            

            return View(blogs);
        }
    }
}
