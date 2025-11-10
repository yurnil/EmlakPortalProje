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
    }
}
