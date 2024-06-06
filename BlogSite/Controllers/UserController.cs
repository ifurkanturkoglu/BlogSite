using BlogSite.Models;
using BlogSiteModels.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace BlogSite.Controllers
{
    public class UserController : Controller
    {

        BlogSiteDbContext context;

        

        public UserController(BlogSiteDbContext _context)
        {

            context = _context;

        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(LoginViewModel model)
        {
            //Burada userType a göre farklı işlemler yapılabilir.

            if (!ModelState.IsValid)
            {
                ViewBag.Mesaj = "İlgili alanları belirtilen kurallara göre doldurun.";
                return View();
            }
            User user = new User()
            {
                UserName = model.UserName,
                Password = model.Password,
                Type = UserType.Admin
            };


            try
            {
                context.Users.Add(user);
                int result = context.SaveChanges();
            }
            catch (Exception ex)
            {
                ViewBag.Mesaj = "Kayıt oluşturulamadı.";
                return View();
            }

            return RedirectToAction(nameof(Login));

        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
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

List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Role, (user.Type == UserType.Admin ? UserType.Admin : UserType.User).ToString()));
                claims.Add(new Claim(ClaimTypes.Name, model.UserName));

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties authProperties = new AuthenticationProperties()
                {
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(12),
                    IsPersistent = false,
                    RedirectUri = "/User/Login"
                };
                if (model.SaveLogin)
                {
                    authProperties.ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30);
                    authProperties.IsPersistent = true;
                }
                await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties
                        );
                return RedirectToAction("Home", "Admin", new { area = "Admin"});
                //Burdan devam cookilere iyice bak otomatik giriş yap. Beni hatırla olayı.
            }
            //Bu kısıma kullanıcı arayüzü açılacak.Blog ekleme kısmı.
            return View();
        }

        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login","User",new { area = ""});
        }
    }
}
