using BlogSite.Models;
using BlogSiteModels.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlogSite.Controllers
{
    public class UserController : Controller
    {
        BlogSiteDbContext context;
        public UserController(BlogSiteDbContext _context)
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
            User user = context.Users.Where(a => a.UserName == model.UserName).FirstOrDefault();

            if (user == null)
            {
                ViewBag.Mesaj = "Kayıtlı kullanıcı bulunamadı.";
                return View();
            }
            else if (user.Password.Equals(model.Password))
            {
                ViewBag.Mesaj = "Giriş Başarılı..";
                //Bu kısımda session yapılacak.

            }

            //Bu kısıma kullanıcı arayüzü açılacak.Blog ekleme kısmı.
            return RedirectToAction();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(LoginViewModel model)
        {

            if (!ModelState.IsValid)
            {
                ViewBag.Mesaj = "İlgili alanları belirtilen kurallara göre doldurun.";
                return View();
            }
            User user = new User()
            {
                UserName = model.UserName,
                Password = model.Password
            };


            try
            {
                context.Users.Add(user);
                int result = context.SaveChanges();
            }
            catch(Exception ex)
            {
                ViewBag.Mesaj = "Kayıt oluşturulamadı.";
                return View();
            }

            return RedirectToAction(nameof(Login));

        }
    }
}
