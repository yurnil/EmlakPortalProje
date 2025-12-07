using EmlakPortal.Models;
using EmlakPortal.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmlakPortal.Areas.Admin.Controllers
{
    [Area("Admin")] // Burası Admin alanı
    [Authorize]     // Sadece giriş yapanlar görebilir
    public class EvController : Controller
    {
        private readonly IRepository<Ev> _evRepository;
        private readonly EmlakPortal.Data.AppDbContext _context; // Kaydetmek için bu şart!

        // Yapıcı Metot: Hem Repository hem Context'i içeri alıyoruz
        public EvController(IRepository<Ev> evRepository, EmlakPortal.Data.AppDbContext context)
        {
            _evRepository = evRepository;
            _context = context;
        }

        // GET: Admin/Ev (Listeleme)
        public IActionResult Index()
        {
            var evler = _evRepository.GetAll();
            return View(evler);
        }

        // GET: Admin/Ev/Create (Formu Göster)
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Ev/Create (Kaydet)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Ev ev)
        {
            if (ModelState.IsValid)
            {
                // 1. Repository'ye ekle
                _evRepository.Add(ev);

                // 2. Veritabanına işle (Bu satır olmadan kaydetmez)
                _context.SaveChanges();

                // 3. Listeye geri dön
                return RedirectToAction("Index");
            }

            // Hata varsa formu tekrar göster
            return View(ev);
        }
    }
}