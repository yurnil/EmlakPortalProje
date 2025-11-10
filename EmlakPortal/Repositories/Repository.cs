using EmlakPortal.Data; // AppDbContext için bunu eklemeliyiz
using Microsoft.EntityFrameworkCore; // DbSet için bunu eklemeliyiz
using System.Collections.Generic;
using System.Linq;

namespace EmlakPortal.Repositories
{
    // Bu sınıf, IRepository'yi uygular
    public class Repository<T> : IRepository<T> where T : class
    {
        // Veritabanı bağlantımız (AppDbContext)
        private readonly AppDbContext _context;
        // Hangi modelle (Ev, Musteri vb.) çalışıyorsak onu tutan DbSet
        private readonly DbSet<T> _dbSet;

        // "Dependency Injection" ile AppDbContext'i alıyoruz
        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>(); // T'nin 'Ev' olduğunu varsayarsak, bu _context.Evler'e eşdeğerdir
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public T GetById(object id)
        {
            // Find metodu, primary key'e göre arama yapar
            return _dbSet.Find(id);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }
    }
}