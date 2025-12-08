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
        // --- DÜZENLEME (EDIT) ---

        // 1. GET: Admin/Ev/Edit/5
        // Bu metot, düzenlenecek evi bulur ve formda gösterir.
        public IActionResult Edit(int id)
        {
            var ev = _evRepository.GetById(id);
            if (ev == null)
            {
                return NotFound();
            }
            return View(ev);
        }

        // 2. POST: Admin/Ev/Edit/5
        // Bu metot, formdaki değişiklikleri kaydeder.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Ev ev)
        {
            if (ModelState.IsValid)
            {
                // 1. Repository'de güncelleme işlemini yap (Hafızada günceller)
                _evRepository.Update(ev);

                // 2. Değişiklikleri veritabanına yansıt (Kalıcı kaydeder)
                _context.SaveChanges();

                // 3. Listeye geri dön
                return RedirectToAction("Index");
            }
            return View(ev);
        }
        // --- SİLME (DELETE) ---

        // 1. GET: Admin/Ev/Delete/5
        // Silinecek ilanı bulur ve "Emin misin?" sayfasını gösterir
        public IActionResult Delete(int id)
        {
            var ev = _evRepository.GetById(id);
            if (ev == null)
            {
                return NotFound();
            }
            return View(ev);
        }

        // 2. POST: Admin/Ev/Delete/5
        // Kullanıcı "Evet, Sil" butonuna basınca burası çalışır
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var ev = _evRepository.GetById(id);
            if (ev != null)
            {
                _evRepository.Delete(ev); // Repository'den sil
                _context.SaveChanges();   // Veritabanına işle
            }
            return RedirectToAction("Index");
        }
    }
}