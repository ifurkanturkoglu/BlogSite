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
            //blog.BlogWriter = user.UserName;
            Blog newBlog = new Blog()
            {
                BlogText = blog.BlogText,
                BlogDescription = blog.BlogDescription,
                BlogTitle = blog.BlogTitle,
                ImageUrl = path.Substring(7),
                UserID = user.UserID,
                BlogAddDate = DateTime.Now
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
        public IActionResult AddComment(int id,[FromBody]Comment model)
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

            return Json(new {newComment = newComment.CommentText});
        }

        [HttpPost]
        [ActionName("GetBlog")]
        public IActionResult AddCommentAnswer(int id,[FromBody]Comment model)
        {
            Comment answeredComment = context.Comments.Where(a => a.CommentId == id).FirstOrDefault();

            if (answeredComment != null)
            {
                Comment newComment = new Comment()
                {
                    BlogId = context.Comments.Include(a=> a.Blog).Where(a => a.CommentId == id).Select(a => a.BlogId).First(),
                    CommentText = model.CommentText,
                    UserId = context.Users.Where(a => a.UserName == User.Identity.Name).Select(a => a.UserID).FirstOrDefault(),
                    CommentAddTime = model.CommentAddTime,
                    ParentCommentId = id
                };

                context.Comments.Add(newComment);
                context.SaveChanges();
            }
            return Json(new { success = answeredComment == null });
        }


    }
}
