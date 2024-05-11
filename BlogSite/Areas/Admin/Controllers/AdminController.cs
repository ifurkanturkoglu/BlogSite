using BlogSite.Models;
using BlogSiteModels.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

namespace BlogSite.Areas.Admin.Controllers
{

    /// <summary>
    /// /Loglama işlemlerini yap unutma!!!
    /// </summary>

    [Area("Admin")]
    [Authorize(Roles = "Admin")]
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
        public async Task<IActionResult> AddBlog(BlogAddViewModel blog)
        {

            User user = context.Users.Include(a => a.UserBlogs).Where(a => a.UserName == User.Identity.Name).FirstOrDefault();

            string path = await ImageUpload(blog.Image);
            if (!ModelState.IsValid || user == null)
            {
                return View();
            }
            Blog newBlog = new Blog()
            {
                BlogText = blog.BlogText,
                BlogDescription = blog.BlogDescription,
                BlogTitle = blog.BlogTitle,
                ImageUrl = path.Substring(7),
                UserID = user.UserID,
                BlogAddDate = DateTime.Now,
            };

            user.UserBlogs.Add(newBlog);

            int result = context.SaveChanges();

            if (result == 0)
            {
                ViewBag.Mesaj = "Ekleme Başarısız";
                return View();
            }

            TempData["Mesaj"] = "Ekleme Başarılı";
            return RedirectToAction(nameof(AddBlog));
        }

        public async Task<string> ImageUpload(IFormFile file)
        {
            if (file == null)
            {
                ViewBag.Mesaj = "Resim yüklenemedi.";
                return "";
            }

            var path = Path.Combine("wwwroot/img/", file.FileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return path;
        }

        public IActionResult BlogsList()
        {
            List<Blog> blogList = context.Blogs.Include(a => a.User).ThenInclude(a => a.UserBlogs).Where(a => a.User.UserName == User.Identity.Name).ToList();
            return View(blogList);
        }

        public IActionResult EditBlog(int id)
        {
            Blog blog = GetUserBlog(id);
            return View(blog);
        }

        [HttpPost]
        public IActionResult EditBLog(int id, Blog blog)
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

            if (!string.IsNullOrEmpty(blog.ImageUrl))
            {
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", blog.ImageUrl.TrimStart('/'));

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
            context.Remove(blog);
            int result = context.SaveChanges();

            if (result == 0)
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

        [HttpPost]
        [ActionName("GetBlog")]
        [Route("/Blogs/GetBlog/{id}")]
        public IActionResult AddComment(int id, [FromBody] Comment model)
        {

            Comment newComment = new Comment()
            {
                BlogId = id,
                CommentText = model.CommentText,
                UserId = context.Users.Where(a => a.UserName == User.Identity.Name).Select(a => a.UserID).FirstOrDefault(),
                CommentAddTime = model.CommentAddTime
            };

            context.Comments.Add(newComment);
            context.SaveChanges();
            //Comment blog = context.Blogs.Where(a=> a.BlogId == id).Include(a => a.Comments)   alt yorum eklerken bak

            return Json(new { newComment = newComment.CommentText });
        }

        [HttpPost]
        public IActionResult AddCommentAnswer(int id, Comment model)
        {
            Comment answeredComment = context.Comments.Where(a => a.CommentId == id).FirstOrDefault();

            if (answeredComment != null)
            {
                Comment newComment = new Comment()
                {
                    BlogId = context.Comments.Include(a => a.Blog).Where(a => a.CommentId == id).Select(a => a.BlogId).First(),
                    CommentText = model.CommentText,
                    UserId = context.Users.Where(a => a.UserName == User.Identity.Name).Select(a => a.UserID).FirstOrDefault(),
                    CommentAddTime = model.CommentAddTime,
                    ParentCommentId = id
                };

                answeredComment.CommentAnswers?.Add(newComment);
                context.Comments.Add(newComment);
                context.SaveChanges();
            }
            return Json(new { success = answeredComment == null });
        }
        public IActionResult UserCommentsList()
        {
            List<CommentViewModel> comments = context.Comments
                .Include(a => a.User)
                .ThenInclude(b => b.Comments)
                .Where(c => c.User.UserName == User.Identity.Name).Select(d => new CommentViewModel
                {
                    CommentId = d.CommentId,
                    CommentText = d.CommentText,
                    CommentWriter = d.User.UserName,
                    CommentAddTime = d.CommentAddTime.ToString("dd.mm.yyyy ss:mm:hh"),
                    CommentAnswers = d.CommentAnswers,
                    ParentCommentId = d.ParentCommentId
                })
                .ToList();

            return View(comments);
        }


        public IActionResult UserDeleteComment(int id)
        {
            Comment comment = context.Comments.Where(a => a.CommentId == id).FirstOrDefault();

            List<Comment> commentAnswers = context.Comments.Include(a => a.CommentAnswers).ThenInclude(b => b.CommentAnswers).Where(b => b.CommentId == id).ToList();

            if (comment == null)
            {
                return Json("yorum bulunamadı");
            }

            if (comment != null)
            {
                DeleteSubComment(comment);
            }

            context.Comments.Remove(comment);

            int result = context.SaveChanges();

            if (result == 0)
            {
                Json("yorum silinemedi");
            }

            return RedirectToAction("UserCommentsList", "Admin");

        }

        public void DeleteSubComment(Comment comment)
        {
            if (comment.CommentAnswers == null)
            {
                context.Comments.Remove(comment);
                return;
            }

            foreach (Comment item in comment.CommentAnswers)
            {
                DeleteSubComment(item);
            }
            context.Comments.Remove(comment);
        }

    }
}
