namespace EmlakPortal.Models
{
    public class IlanTur
    {
        public int Id { get; set; }
        public string Ad { get; set; } // Örn: "Daire", "Villa"

        // Bir türde birden fazla ev olabilir (İlişki için hazırlık)
        public ICollection<Ev> Evler { get; set; }
    }
}