using EmlakPortal.Data; // AppDbContext için bu gerekli
using EmlakPortal.Models; // Ev modeli için bu gerekli
using EmlakPortal.Repositories; // IRepository için bu gerekli
using Microsoft.AspNetCore.Mvc;

namespace EmlakPortal.Controllers
{
    public class EvController : Controller
    {
        // 1. Repository'mizi ve Context'imizi tutacak değişkenleri tanımlıyoruz.

        // Bu, veritabanı işlemlerimizi yapacak (Listele, Ekle, Sil, Güncelle)
        private readonly IRepository<Ev> _evRepository;

        // EF Core'un değişiklikleri veritabanına kaydetmesi (COMMIT) için
        private readonly AppDbContext _context;

        // 2. "Constructor" (Yapıcı Metot)
        public EvController(IRepository<Ev> evRepository, AppDbContext context)
        {
            _evRepository = evRepository;
            _context = context;
        }

        // 3. Index Aksiyonu (Tüm Evleri Listeleyecek Sayfa)
        // Tarayıcıdan "..../Ev/Index" adresi çağrıldığında bu metot çalışır.
        public IActionResult Index()
        {
            // Repository'mizi kullanarak tüm evleri veritabanından çekiyoruz.
            var evler = _evRepository.GetAll();

            // "evler" listesini View'a (görünüme) gönderiyoruz.
            return View(evler);
        }

        // 1. GET: Ev/Create
        public IActionResult Create()
        {
            return View();
        }

        // 2. POST: Ev/Create
        [HttpPost]
        [ValidateAntiForgeryToken] // Güvenlik önlemi (sahte istekleri engeller)
        public IActionResult Create(Ev ev)
        {
            // Eğer gelen veriler kurallara uygunsa (örn: Fiyat sayı mı?)
            if (ModelState.IsValid)
            {
                // 1. Repository'ye ekle (Hafızaya alır)
                _evRepository.Add(ev);

                // 2. Veritabanına işle (Kalıcı kaydeder)
                _context.SaveChanges();

                // 3. İş bitince listeye geri dön
                return RedirectToAction("Index");
            }

            // Eğer hata varsa (boş alan vs.), formu hata mesajlarıyla geri göster
            return View(ev);
        }

        // 1. GET: Ev/Edit/5
        // Bu metot, düzenlenmek istenen evi bulur ve formda gösterir.
        public IActionResult Edit(int id)
        {
            // Veritabanından o id'li evi bul
            var ev = _evRepository.GetById(id);

            // Eğer ev yoksa hata ver
            if (ev == null)
            {
                return NotFound();
            }

            // Evi bulduysan View'a gönder ki formda bilgileri dolsun
            return View(ev);
        }

        // 2. POST: Ev/Edit/5
        // Bu metot, formdaki değişiklikleri kaydeder.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Ev ev)
        {
            if (ModelState.IsValid)
            {
                // Repository'de güncelleme işlemini yap
                _evRepository.Update(ev);

                // Değişiklikleri veritabanına yansıt
                _context.SaveChanges();

                // Listeye geri dön
                return RedirectToAction("Index");
            }
            return View(ev);
        }
        // --- SİLME (DELETE) KODLARI ---

        // 1. GET: Ev/Delete/5
        // Bu metot, silinecek evi bulur ve "Emin misin?" sayfasını gösterir.
        public IActionResult Delete(int id)
        {
            var ev = _evRepository.GetById(id);
            if (ev == null)
            {
                return NotFound();
            }
            return View(ev);
        }

        // 2. POST: Ev/Delete/5
        // Bu metot, kullanıcı onay verince çalışır ve silme işlemini yapar.
        // ActionName("Delete") sayesinde, metot adı "DeleteConfirmed" olsa bile
        // tarayıcıdan "Delete" ismiyle çağrılabilir.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var ev = _evRepository.GetById(id);
            if (ev != null)
            {
                // Repository'den sil
                _evRepository.Delete(ev);

                // Veritabanına kaydet
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        // --- DETAYLAR (DETAILS) METODU ---

        // GET: Ev/Details/5
        public IActionResult Details(int id)
        {
            // Veritabanından evi bul
            var ev = _evRepository.GetById(id);

            // Bulamazsa hata ver
            if (ev == null)
            {
                return NotFound();
            }

            // Evi görünüme gönder
            return View(ev);
        }
    }
}