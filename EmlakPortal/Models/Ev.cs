using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
namespace EmlakPortal.Models
{
    public class Ev
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Baslik { get; set; }

        [Required]
        public string Adres { get; set; }

        [Required]
        public decimal Fiyat { get; set; }

        public string? ResimUrl { get; set; }

        public string? Aciklama { get; set; }
        // 1. İlan Türü Bağlantısı (Örn: Daire)
        public int IlanTurId { get; set; }
        public IlanTur? IlanTur { get; set; } // Soru işareti (?) boş geçilebilir olmasını sağlar (hata almamak için şimdilik)

        // 2. İlan Durumu Bağlantısı (Örn: Satılık)
        public int IlanDurumId { get; set; }
        public IlanDurum? IlanDurum { get; set; }
    }
}
