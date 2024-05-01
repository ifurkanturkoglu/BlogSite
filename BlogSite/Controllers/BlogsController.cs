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
                BlogViewViewModel blog = context.Blogs.Where(a => a.BlogId == id).Select(b => new BlogViewViewModel
                {
                    BlogTitle = b.BlogTitle,
                    BlogDescription = b.BlogDescription,
                    BlogText = b.BlogText,
                    BlogWriter = b.User.UserName,
                    ImageUrl = b.ImageUrl                    
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
            List<BlogViewViewModel> blogs = context.Blogs.Select(a => new BlogViewViewModel
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
