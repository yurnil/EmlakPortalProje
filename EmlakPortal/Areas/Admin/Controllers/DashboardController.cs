using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmlakPortal.Areas.Admin.Controllers
{
    // ÖNEMLİ: Bu Controller'ın 'Admin' alanına ait olduğunu belirtiyoruz.
    // Bunu yazmazsak sistem bu sayfayı bulamaz!
    [Area("Admin")]
    [Authorize]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}