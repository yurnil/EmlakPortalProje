// EvController.cs

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

        // Bu, EF Core'un değişiklikleri veritabanına kaydetmesi (COMMIT) için
        // geçici olarak gereklidir.
        private readonly AppDbContext _context;

        // 2. "Constructor" (Yapıcı Metot)
        // Program.cs'te yaptığımız ayar sayesinde, bu controller her çalıştığında
        // .NET bize otomatik olarak bir IRepository<Ev> ve bir AppDbContext sağlayacak.
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
    }
}