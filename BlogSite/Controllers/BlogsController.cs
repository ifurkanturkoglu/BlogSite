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
                    BlogLikeCount = b.LikeCount,
                    BlogDislikeCount = b.DislikeCount,
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

        //[HttpPost]
        //public IActionResult LikeBlog(int id)
        //{
        //    Blog blog = context.Blogs.Include(a=> a.BlogLikeAndDislike).Where(a=> a.BlogId == id).Single();


        //    if (blog.BlogLikeAndDislike.Any(a=> a.UserName == User.Identity.Name))
        //    {
        //        blog.LikeCount -= 1;
        //        LikeAndDislikeBlog likeAndDislikeBlog = context.LikeAndDislikeBlogs.Where(a => a.UserName == User.Identity.Name).Single();
        //        context.LikeAndDislikeBlogs.Remove(likeAndDislikeBlog);
        //    }
        //    else
        //    {
        //        LikeAndDislikeBlog likeAndDislikeBlog = new LikeAndDislikeBlog
        //        {
        //            BlogId = id,
        //            IsDisliked = false,
        //            IsLiked = false,
        //            UserName = User.Identity.Name
        //        };
        //        blog.LikeCount += 1;
        //        likeAndDislikeBlog.IsLiked = true;
        //        context.LikeAndDislikeBlogs.Add(likeAndDislikeBlog);
        //    }

        //    context.SaveChanges();

        //    return Json(new { likeCount = blog.LikeCount});
        //}


        //[HttpPost]

        //public IActionResult DislikeBlog(int id)
        //{
        //    Blog blog = context.Blogs.Include(a => a.BlogLikeAndDislike).Where(a => a.BlogId == id).Single();


        //    if (blog.BlogLikeAndDislike.Any(a => a.UserName == User.Identity.Name))
        //    {
        //        blog.DislikeCount -= 1;
        //        LikeAndDislikeBlog likeAndDislikeBlog = context.LikeAndDislikeBlogs.Where(a => a.UserName == User.Identity.Name).Single();
        //        context.LikeAndDislikeBlogs.Remove(likeAndDislikeBlog);
        //    }
        //    else
        //    {
        //        LikeAndDislikeBlog likeAndDislikeBlog = new LikeAndDislikeBlog
        //        {
        //            BlogId = id,
        //            IsDisliked = false,
        //            IsLiked = false,
        //            UserName = User.Identity.Name
        //        };
        //        blog.DislikeCount += 1;
        //        likeAndDislikeBlog.IsDisliked = true;
        //        context.LikeAndDislikeBlogs.Add(likeAndDislikeBlog);
        //    }

        //    context.SaveChanges();

        //    return Json(new { dislikeCount = blog.DislikeCount });
        //}
        [HttpPost]
        public IActionResult LikeOrDislikeBlog(int id, bool isLike)
        {


            //Model kısmındaki iki tane bool ifadeyi teke indir öyle çöz burayı sıfırdan yaz saçma oldu.

            Blog blog = context.Blogs.Include(a => a.BlogLikeAndDislike).SingleOrDefault(a => a.BlogId == id);

            if (blog == null)
            {
                return NotFound(); // Blog bulunamadı durumunda hata döndür
            }

            LikeAndDislikeBlog likeAndDislikeBlog = blog.BlogLikeAndDislike.FirstOrDefault(a => a.UserName == User.Identity.Name);

            if (likeAndDislikeBlog != null)
            {
                if(isLike == likeAndDislikeBlog.IsLiked)
                {
                    blog.LikeCount = likeAndDislikeBlog.IsLiked ? blog.LikeCount-1 : blog.LikeCount+1;
                    likeAndDislikeBlog.IsLiked = isLike == true ? false : true;
                    likeAndDislikeBlog.IsDisliked = isLike == false ? true : false;
                    blog.DislikeCount = isLike == true ? blog.DislikeCount :blog.DislikeCount - 1;
                }
                if(isLike == likeAndDislikeBlog.IsDisliked)
                {
                    blog.DislikeCount = likeAndDislikeBlog.IsDisliked ? blog.DislikeCount-1 : blog.DislikeCount + 1;
                    likeAndDislikeBlog.IsLiked =  isLike == true ? true : false;
                    likeAndDislikeBlog.IsDisliked = isLike == true ? false : true;
                    blog.LikeCount = isLike == false ? blog.LikeCount : blog.LikeCount - 1;
                }
                if(!likeAndDislikeBlog.IsLiked && !likeAndDislikeBlog.IsDisliked)
                {
                    context.LikeAndDislikeBlogs.Remove(likeAndDislikeBlog);
                }
            }
            else
            {
                likeAndDislikeBlog = new LikeAndDislikeBlog
                {
                    BlogId = id,
                    IsDisliked = !isLike,
                    IsLiked = isLike,
                    UserName = User.Identity.Name
                };

                if (isLike)
                {
                    blog.LikeCount += 1;
                }
                else
                {
                    blog.DislikeCount += 1;
                }

                context.LikeAndDislikeBlogs.Add(likeAndDislikeBlog);
            }

            context.SaveChanges();

            return Json(new { likeCount = blog.LikeCount, dislikeCount = blog.DislikeCount });
        }

    }
}
