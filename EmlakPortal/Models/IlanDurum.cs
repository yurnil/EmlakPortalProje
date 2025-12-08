namespace EmlakPortal.Models
{
    public class IlanDurum
    {
        public int Id { get; set; }
        public string Ad { get; set; } // Örn: "Satılık", "Kiralık"

        // Bir durumda birden fazla ev olabilir
        public ICollection<Ev> Evler { get; set; }
    }
}