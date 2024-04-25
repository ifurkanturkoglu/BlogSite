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

                bool IsCookieHave = CheckCookies();
               

                if(user.Type == UserType.Admin)
                {
                    List<Claim> claims = new List<Claim>();

                    claims.Add(new Claim(ClaimTypes.Name, model.UserName));
                    claims.Add(new Claim(ClaimTypes.Role, UserType.Admin.ToString()));


                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
                    AuthenticationProperties authProperties = new AuthenticationProperties()
                    {
                        ExpiresUtc = DateTimeOffset.UtcNow.AddSeconds(10),
                        IsPersistent = true,
                        RedirectUri = "/User/Login"
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties
                        );

                    var cookieValue = Request.Cookies["Asp.NetCore.Cookies"];
                    //Burdan devam cookilere iyice bak otomatik giriş yap. Beni hatırla olayı.
                    return RedirectToAction("Home", "Admin",new {area = "Admin"});
                }
            }

            //Bu kısıma kullanıcı arayüzü açılacak.Blog ekleme kısmı.
            return RedirectToAction();
        }

        public bool CheckCookies()
        {
            return Response.Cookies is null;
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
                Password = model.Password,
                Type = UserType.User                
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
