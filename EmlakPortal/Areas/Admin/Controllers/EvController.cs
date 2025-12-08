using EmlakPortal.Models;
using EmlakPortal.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // SelectList için gerekli
using Microsoft.EntityFrameworkCore; // Include metodu için gerekli

namespace EmlakPortal.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class EvController : Controller
    {
        private readonly IRepository<Ev> _evRepository;
        private readonly EmlakPortal.Data.AppDbContext _context;

        public EvController(IRepository<Ev> evRepository, EmlakPortal.Data.AppDbContext context)
        {
            _evRepository = evRepository;
            _context = context;
        }

        // GET: Admin/Ev
        public IActionResult Index()
        {
            // Eski hali: var evler = _evRepository.GetAll();

            // YENİ HALİ: İlişkili verileri de (Include) getiriyoruz
            // Not: Repository deseninde "GetAll" metodu genellikle Include desteklemez.
            // Bu yüzden burada geçici olarak direkt _context üzerinden çekebiliriz
            // VEYA Repository'yi güncelleyebiliriz.

            // En pratik çözüm (şimdilik): Context üzerinden çekmek
            var evler = _context.Evler
                .Include(e => e.IlanTur)   // Türü getir (Daire, Villa vs.)
                .Include(e => e.IlanDurum) // Durumu getir (Satılık vs.)
                .ToList();

            return View(evler);
        }

        // GET: Admin/Ev/Create
        public IActionResult Create()
        {
            // --- OTOMATİK VERİ DOLDURMA ---
            // Eğer veritabanında hiç tür yoksa, hemen ekle!
            if (!_context.IlanTurleri.Any())
            {
                _context.IlanTurleri.AddRange(
                    new IlanTur { Ad = "Daire" },
                    new IlanTur { Ad = "Villa" },
                    new IlanTur { Ad = "Müstakil Ev" },
                    new IlanTur { Ad = "Arsa" },
                    new IlanTur { Ad = "İşyeri" }
                );
                _context.SaveChanges();
            }

            // Eğer veritabanında hiç durum yoksa, hemen ekle!
            if (!_context.IlanDurumlari.Any())
            {
                _context.IlanDurumlari.AddRange(
                    new IlanDurum { Ad = "Satılık" },
                    new IlanDurum { Ad = "Kiralık" },
                    new IlanDurum { Ad = "Günlük Kiralık" }
                );
                _context.SaveChanges();
            }
            // ---------------------------------------------

            // Listeleri doldurup sayfaya gönder
            ViewBag.Turler = new SelectList(_context.IlanTurleri.ToList(), "Id", "Ad");
            ViewBag.Durumlar = new SelectList(_context.IlanDurumlari.ToList(), "Id", "Ad");
            return View();
        }

        // POST: Admin/Ev/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Ev ev)
        {
            if (ModelState.IsValid)
            {
                _evRepository.Add(ev);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            // Hata olursa listeleri tekrar doldur
            ViewBag.Turler = new SelectList(_context.IlanTurleri.ToList(), "Id", "Ad");
            ViewBag.Durumlar = new SelectList(_context.IlanDurumlari.ToList(), "Id", "Ad");
            return View(ev);
        }

        // GET: Admin/Ev/Edit/5
        public IActionResult Edit(int id)
        {
            var ev = _evRepository.GetById(id);
            if (ev == null)
            {
                return NotFound();
            }

            ViewBag.Turler = new SelectList(_context.IlanTurleri.ToList(), "Id", "Ad", ev.IlanTurId);
            ViewBag.Durumlar = new SelectList(_context.IlanDurumlari.ToList(), "Id", "Ad", ev.IlanDurumId);
            return View(ev);
        }

        // POST: Admin/Ev/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Ev ev)
        {
            if (ModelState.IsValid)
            {
                _evRepository.Update(ev);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Turler = new SelectList(_context.IlanTurleri.ToList(), "Id", "Ad", ev.IlanTurId);
            ViewBag.Durumlar = new SelectList(_context.IlanDurumlari.ToList(), "Id", "Ad", ev.IlanDurumId);
            return View(ev);
        }

        // GET: Admin/Ev/Delete/5
        public IActionResult Delete(int id)
        {
            var ev = _evRepository.GetById(id);
            if (ev == null)
            {
                return NotFound();
            }
            return View(ev);
        }

        // POST: Admin/Ev/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var ev = _evRepository.GetById(id);
            if (ev != null)
            {
                _evRepository.Delete(ev);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        // Admin/Ev/SilAjax
        [HttpPost]
        public IActionResult SilAjax(int id)
        {
            var ev = _evRepository.GetById(id);
            if (ev == null)
            {
                return Json(new { success = false, message = "Kayıt bulunamadı." });
            }

            _evRepository.Delete(ev);
            _context.SaveChanges();

            // İşlem başarılı mesajı gönderiyoruz
            return Json(new { success = true });
        }
    }
}