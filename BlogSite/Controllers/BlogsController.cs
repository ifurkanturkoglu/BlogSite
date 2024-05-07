using BlogSite.Models;
using BlogSiteModels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogSite.Controllers
{
    public class BlogsController : Controller
    {
        BlogSiteDbContext context;
        public BlogsController(BlogSiteDbContext _context)
        {
            context = _context;
        }

        public IActionResult GetBlog(int id)
        {
            //route sadece id değil ayrıca kullanıcı adını da aalabilir. Ona bi bak
            try
            {
                BlogAndCommentViewModel blog = context.Blogs.Where(a => a.BlogId == id).Select(b => new BlogAndCommentViewModel
                {
                    BlogTitle = b.BlogTitle,
                    BlogDescription = b.BlogDescription,
                    BlogText = b.BlogText,
                    BlogWriter = b.User.UserName,
                    ImageUrl = b.ImageUrl,
                    Comments = b.Comments.Select(a => new CommentViewModel{
                        CommentId = a.CommentId,
                        CommentText = a.CommentText,
                        CommentWriter = context.Users.Where(b => b.UserID == a.UserId).FirstOrDefault().UserName.ToString(),
                        CommentAddTime = a.CommentAddTime.ToString("dd/MM/yyyy HH:MM:ss"),
                        CommentAnswers = a.CommentAnswers
                    }).ToList()

                }).FirstOrDefault();

                return View(blog);
            }
            catch
            {
                return RedirectToAction(nameof(BlogsList));
            }

        }
        //DTO yapılacak
        public IActionResult BlogsList(int page)
        {
            if (page == 0)
                page = 1;
            IQueryable<BlogViewViewModel> blogs = context.Blogs.OrderByDescending(a => a.BlogAddDate).Skip((page - 1) * 4).Select(a => new BlogViewViewModel
            {
                BlogId = a.BlogId,
                BlogTitle = a.BlogTitle,
                BlogDescription = a.BlogDescription,
                BlogText = a.BlogText,
                ImageUrl = a.ImageUrl,
                BlogWriter = a.User.UserName
            }).Take(4);


            ViewBag.PageCount = Math.Ceiling((double)context.Blogs.Count() / 4);
            ViewBag.CurrentPage = page;
            return View(blogs);
        }

        public IActionResult CommentAnswer(int id)
        {
            CommentViewModel answeredComment = context.Comments.Where(a => a.CommentId == id).Select(new CommentViewModel
            {
                CommentId = id,
                CommentText = 
            }).FirstOrDefault();
            return PartialView("_Comment",answeredComment);
        }
    }
}
