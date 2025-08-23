using MyApi.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        // --- Sorgulama ---
        IQueryable<T> GetAll(bool tracking = true);
        // Tüm verileri getirir. (tracking ile EF değişiklikleri izlesin mi seçilir)

        IQueryable<T> GetWhere(Expression<Func<T, bool>> predicate, bool tracking = true);
        // Filtreli sorgu yapar. (tracking opsiyonel)

        Task<T?> GetByIdAsync(Guid id, bool tracking = true);
        // Primary Key (Guid) ile tek kayıt getirir.

        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, bool tracking = true);
        // Filtreye uyan ilk kaydı getirir.

        // --- Ekleme ---
        Task AddAsync(T entity);
        // Yeni entity ekler.

        Task AddRangeAsync(IEnumerable<T> entities);
        // Birden fazla entity ekler.

        // --- Güncelleme ---
        void Update(T entity);
        // Entity günceller.

        void UpdateRange(IEnumerable<T> entities);
        // Birden fazla entity günceller.

        // --- Silme ---
        void Delete(T entity);
        // Entity siler.

        void DeleteRange(IEnumerable<T> entities);
        // Birden fazla entity siler.
        Task<bool> DeleteAsync(Guid id);
        //

        // --- Kontrol & Sayma ---
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        // Belirli şarta uyan kayıt var mı kontrol eder.

        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);
        // Kayıt sayısını döner. Filtre varsa ona göre sayar.

        // --- Sayfalama ---
        Task<List<T>> GetPagedAsync(int pageNumber, int pageSize, bool tracking = true);
        // Sayfalama yaparak kayıtları getirir.

        // --- Kaydet ---
        Task<int> SaveChangesAsync();
        // Veritabanına yapılan değişiklikleri yazar.
    }
}
