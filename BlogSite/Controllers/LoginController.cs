using BlogSite.Models;
using BlogSiteModels.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlogSite.Controllers
{
    public class LoginController : Controller
    {
        BlogSiteDbContext context;
        public LoginController(BlogSiteDbContext _context)
        {
            context = _context;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            




            ViewBag.Mesaj = "Giriş Başarılı..";
            return View();
        }
    }
}
