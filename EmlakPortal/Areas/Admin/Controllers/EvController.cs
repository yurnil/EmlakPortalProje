using EmlakPortal.Models;
using EmlakPortal.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http; // Session için gerekli

namespace EmlakPortal.Areas.Admin.Controllers
{
    [Area("Admin")]
    // [Authorize]  <-- Bunu iptal ettik çünkü kendi Session sistemimizi kullanıyoruz.
    public class EvController : Controller
    {
        private readonly IRepository<Ev> _evRepository;
        private readonly EmlakPortal.Data.AppDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public EvController(IRepository<Ev> evRepository, EmlakPortal.Data.AppDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _evRepository = evRepository;
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Admin/Ev
        public IActionResult Index()
        {
            // --- GÜVENLİK KONTROLÜ (KAPI BEKÇİSİ) ---
            // Eğer "AdminGiris" session'ı yoksa, kullanıcıyı Login sayfasına gönder.
            if (HttpContext.Session.GetString("AdminGiris") == null)
            {
                // area="" diyerek Admin klasöründen çıkıp ana dizindeki AccountController'a gitmesini sağlıyoruz.
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            // ----------------------------------------

            var evler = _context.Evler
                .Include(e => e.IlanTur)
                .Include(e => e.IlanDurum)
                .ToList();

            return View(evler);
        }

        // GET: Admin/Ev/Create
        public IActionResult Create()
        {
            // İstersen buraya da aynı güvenlik kontrolünü ekleyebilirsin ama Index'te olması şimdilik yeterli.

            // --- OTOMATİK VERİ DOLDURMA (SEED DATA) ---
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

            ViewBag.Turler = new SelectList(_context.IlanTurleri.ToList(), "Id", "Ad");
            ViewBag.Durumlar = new SelectList(_context.IlanDurumlari.ToList(), "Id", "Ad");
            return View();
        }

        // POST: Admin/Ev/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ev ev, IFormFile? resimDosyasi)
        {
            if (ModelState.IsValid)
            {
                // --- RESİM YÜKLEME İŞLEMİ ---
                if (resimDosyasi != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(resimDosyasi.FileName);
                    string uploadPath = Path.Combine(wwwRootPath, @"img");

                    if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

                    using (var fileStream = new FileStream(Path.Combine(uploadPath, fileName), FileMode.Create))
                    {
                        await resimDosyasi.CopyToAsync(fileStream);
                    }

                    ev.ResimUrl = @"/img/" + fileName;
                }
                else
                {
                    ev.ResimUrl = "";
                }
                // -----------------------------

                _evRepository.Add(ev);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

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
        public async Task<IActionResult> Edit(Ev ev, IFormFile? resimDosyasi)
        {
            if (ModelState.IsValid)
            {
                // --- RESİM GÜNCELLEME İŞLEMİ ---
                if (resimDosyasi != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(resimDosyasi.FileName);
                    string uploadPath = Path.Combine(wwwRootPath, @"img");

                    if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

                    // ESKİ RESMİ SİL
                    if (!string.IsNullOrEmpty(ev.ResimUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, ev.ResimUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // YENİ RESMİ KAYDET
                    using (var fileStream = new FileStream(Path.Combine(uploadPath, fileName), FileMode.Create))
                    {
                        await resimDosyasi.CopyToAsync(fileStream);
                    }

                    ev.ResimUrl = @"/img/" + fileName;
                }
                // -------------------------------

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

            return Json(new { success = true });
        }
    }
}