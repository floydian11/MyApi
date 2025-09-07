using Microsoft.EntityFrameworkCore;
using MyApi.Application.Repositories;
using MyApi.Domain.Entities.Common;
using MyApi.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Persistence.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly AppDbContext _context;
        
        private readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            // _context.Set<T>() → EF Core'a diyor ki: “Veritabanında T türünde bir tablo var, onu kullan.”
            // T tipi BaseEntity’den türediği için bu DbSet<T> ile CRUD işlemleri yapabiliriz.
            _dbSet = _context.Set<T>();
        }

        // --- Sorgulama ---
        public IQueryable<T> GetAll(bool tracking = true)
        {
            // Tüm verileri getirir. tracking=true ise EF değişiklikleri izler, false ise izleme yapılmaz.
            return tracking ? _dbSet.AsQueryable() : _dbSet.AsNoTracking();
        }

        public IQueryable<T> GetAllAsNoTracking()
        {
            // Tüm verileri getirir. Her zaman tracking kapalıdır.
            return _dbSet.AsNoTracking();
        }

        public IQueryable<T> GetWhere(Expression<Func<T, bool>> predicate, bool tracking = true)
        {
            // Filtreli sorgu yapar. tracking parametresi ile izleme açılıp kapatılabilir.
            var query = _dbSet.Where(predicate);
            return tracking ? query : query.AsNoTracking();
        }

        public async Task<T?> GetByIdAsync(Guid id, bool tracking = true)
        {
            // Primary Key (Guid) ile tek kayıt getirir.
            // tracking=false ise EF entity'i detach ederek izlemeyi kapatır.
            var entity = await _dbSet.FindAsync(id);
            if (!tracking && entity is not null)
                _context.Entry(entity).State = EntityState.Detached;

            return entity;
        }

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, bool tracking = true)
        {
            // Belirli bir filtreye uyan ilk kaydı getirir. tracking opsiyonel.
            return tracking
                ? await _dbSet.FirstOrDefaultAsync(predicate)
                : await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        // --- Ekleme ---
        public async Task AddAsync(T entity)
        {
            // Yeni bir entity ekler. Henüz DB'ye yazılmaz.
            await _dbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            // Birden fazla entity ekler. Henüz DB'ye yazılmaz.
            await _dbSet.AddRangeAsync(entities);
        }

        // --- Güncelleme ---
        public void Update(T entity)
        {
            // Var olan entity üzerinde değişiklikleri uygular.
            // Henüz DB'ye yansımaz.
            _dbSet.Update(entity);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            // Birden fazla entity üzerinde değişiklik uygular. Henüz DB'ye yansımaz.
            _dbSet.UpdateRange(entities);
        }

        // --- Silme ---
        public Task DeleteAsync(T entity)
        {
            // Entity'yi siler. Henüz DB'ye yansımaz.
            _dbSet.Remove(entity);
            return Task.CompletedTask;
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            // Birden fazla entity'yi siler. Henüz DB'ye yansımaz.
            _dbSet.RemoveRange(entities);
        }

        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            // Primary Key ile entity'yi bulur ve siler.
            var entity = await _dbSet.FindAsync(id);
            if (entity is null) return false;

            _dbSet.Remove(entity);
            return true;
        }

        // --- Kontrol & Sayma ---
        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            // Belirli bir şartı sağlayan kayıt var mı kontrol eder.
            return await _dbSet.AnyAsync(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
        {
            // Filtre varsa ona göre, yoksa tüm kayıt sayısını döner.
            return predicate is null
                ? await _dbSet.CountAsync()
                : await _dbSet.CountAsync(predicate);
        }

        // --- Sayfalama ---
        public async Task<List<T>> GetPagedAsync(int pageNumber, int pageSize, bool tracking = true)
        {
            // Sayfalama ile kayıtları getirir.
            // tracking parametresi ile EF değişiklik izleme açılıp kapatılabilir.
            // pageNumber < 1 veya pageSize < 1 durumunda şimdilik exception fırlatılmaz.
            var query = tracking ? _dbSet.AsQueryable() : _dbSet.AsNoTracking();

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
