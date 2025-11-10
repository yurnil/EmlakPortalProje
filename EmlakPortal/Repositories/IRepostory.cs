namespace EmlakPortal.Repositories
{
    // '<T> where T : class' bu arayüzün herhangi bir modelle (Ev, Musteri, vb.)
    // çalışabilmesini sağlayan "Generic" yapıdır.
    public interface IRepository<T> where T : class
    {
        // Tümünü Listele
        IEnumerable<T> GetAll();

        // ID'ye göre bir tane getir
        // 'object' kullanıyoruz ki ID (Primary Key) int, string veya Guid olabilir.
        T GetById(object id);

        // Yeni bir tane ekle
        void Add(T entity);

        // Bir taneyi güncelle
        void Update(T entity);

        // Bir taneyi sil
        void Delete(T entity);
    }
}
