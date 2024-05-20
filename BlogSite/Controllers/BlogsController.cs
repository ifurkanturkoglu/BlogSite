using BlogSite.Models;
using BlogSiteModels.Models;
using Microsoft.AspNetCore.Authorization;
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
                    BlogLikeCount = b.LikeCount,
                    BlogDislikeCount = b.DislikeCount,
                    IsLiked = context.LikeAndDislikeBlogs.Where(a=> a.BlogId == id).Where(a=> a.UserName == User.Identity.Name).Select(a=> a.IsLiked).Single(),
                    Comments = b.Comments.Select(a => new CommentViewModel
                    {
                        CommentId = a.CommentId,
                        CommentText = a.CommentText,
                        CommentWriter = context.Users.Where(c => c.UserID == a.UserId).FirstOrDefault().UserName.ToString(),
                        CommentAddTime = a.CommentAddTime.ToString("dd/MM/yyyy HH:MM:ss"),                        
                        ParentCommentId = a.ParentCommentId,
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
            CommentViewModel answeredComment = context.Comments
                .Where(a => a.CommentId == id)
                .Select(a => new CommentViewModel
                {
                    CommentId = id,
                    CommentText = "",
                    CommentWriter = User.Identity.Name,
                    CommentAddTime = DateTime.Now.ToString("dd:mm:yyyy ss:mm:hh"),
                    ParentCommentId = a.ParentCommentId
                }).FirstOrDefault();
            return PartialView("_Comment", answeredComment);
        }

        
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult LikeOrDislikeBlog(int id, bool isLike)
        {
            //Model kısmındaki iki tane bool ifadeyi teke indir öyle çöz burayı sıfırdan yaz saçma oldu.

            Blog blog = context.Blogs.Include(a => a.BlogLikeAndDislike).SingleOrDefault(a => a.BlogId == id);

            LikeAndDislikeBlog likeAndDislikeBlog = context.LikeAndDislikeBlogs.SingleOrDefault(a => a.UserName == User.Identity.Name);

            if(likeAndDislikeBlog != null)
            {
                if(likeAndDislikeBlog.IsLiked == isLike)
                {
                    //context.LikeAndDislikeBlogs.Remove(likeAndDislikeBlog);
                    blog.BlogLikeAndDislike.Remove(likeAndDislikeBlog);
                }
                else
                {
                    likeAndDislikeBlog.IsLiked = isLike;
                }
            }

            else
            {
                LikeAndDislikeBlog likeAndDislike = new LikeAndDislikeBlog
                {
                    BlogId = id,
                    IsLiked = isLike,
                    UserName = User.Identity.Name.ToString()
                };
                context.LikeAndDislikeBlogs.Add(likeAndDislike);
            }

            
            blog.LikeCount = blog.BlogLikeAndDislike.Count(a => a.IsLiked);
            blog.DislikeCount = blog.BlogLikeAndDislike.Count(a => !a.IsLiked);

            context.SaveChanges();

            return Json(new { likeCount = blog.LikeCount, dislikeCount = blog.DislikeCount });
        }

    }
}
