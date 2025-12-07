using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EmlakPortal.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthController : Controller
    {
        // 1. GET: Giriş Sayfasını Göster
        public IActionResult Login()
        {
            return View();
        }

        // 2. POST: Giriş Yapılınca Çalışır
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            // Kullanıcı adı ve şifreyi elle yazıyoruz.
            // Finalde burayı veritabanından kontrol edeceğiz.
            if (email == "admin@emlak.com" && password == "123456")
            {
                // Kimlik bilgilerini oluştur (Nüfus cüzdanı gibi)
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, email),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties();

                // Çerezi (Cookie) oluştur ve kullanıcıyı sisteme al
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                // Başarılıysa Panele git
                return RedirectToAction("Index", "Dashboard");
            }

            // Hatalıysa mesaj göster
            ViewBag.Error = "Hatalı e-posta veya şifre!";
            return View();
        }

        // 3. Çıkış Yap (Logout)
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(); // Çerezi siler
            return RedirectToAction("Login");
        }
    }
}