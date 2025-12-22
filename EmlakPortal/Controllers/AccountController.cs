using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; // Session için gerekli

namespace EmlakPortal.Controllers
{
    public class AccountController : Controller
    {
        // 1. Giriş Sayfasını Göster (GET)
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // 2. Giriş Yap Butonuna Basılınca (POST)
        [HttpPost]
        public IActionResult Login(string kadi, string sifre)
        {
            // Basit kontrol: Kullanıcı adı "admin", şifre "123" mü?
            if (kadi == "admin" && sifre == "123")
            {
                // Doğruysa: Sisteme not al (Session oluştur)
                HttpContext.Session.SetString("AdminGiris", "ok");

                // Admin panelindeki Ev listesine yönlendir
                // Area adı: "Admin", Controller: "Ev", Action: "Index"
                return RedirectToAction("Index", "Ev", new { area = "Admin" });
            }

            // Yanlışsa: Hata mesajı göster
            ViewBag.Hata = "Hatalı kullanıcı adı veya şifre!";
            return View();
        }

        // 3. Çıkış Yap
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Oturumu sil
            return RedirectToAction("Index", "Home"); // Ana sayfaya at
        }
    }
}