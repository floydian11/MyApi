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
        // Tüm verileri getirir. tracking=true ise EF değişiklikleri izler, false ise izleme yapılmaz.

        IQueryable<T> GetAllAsNoTracking();
        // Tüm verileri getirir. Her zaman tracking kapalıdır.

        IQueryable<T> GetWhere(Expression<Func<T, bool>> predicate, bool tracking = true);
        // Filtreli sorgu yapar. tracking parametresi ile izleme açılıp kapatılabilir.

        Task<T?> GetByIdAsync(Guid id, bool tracking = true);
        // Primary Key (Guid) ile tek kayıt getirir. tracking opsiyonel.

        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, bool tracking = true);
        // Belirli bir filtreye uyan ilk kaydı getirir. tracking opsiyonel.

        // --- Ekleme ---
        Task AddAsync(T entity);
        // Yeni bir entity ekler.

        Task AddRangeAsync(IEnumerable<T> entities);
        // Birden fazla entity ekler.

        // --- Güncelleme ---
        void Update(T entity);
        // Var olan entity üzerinde değişiklikleri uygular.

        void UpdateRange(IEnumerable<T> entities);
        // Birden fazla entity üzerinde değişiklik uygular.

        // --- Silme ---
        void Delete(T entity);
        // Entity’yi siler.

        void DeleteRange(IEnumerable<T> entities);
        // Birden fazla entity’yi siler.

        Task<bool> DeleteByIdAsync(Guid id);
        // Primary Key (Guid) ile entity’yi bulur ve siler. Kayıt yoksa false döner.

        // --- Kontrol & Sayma ---
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        // Belirli bir şartı sağlayan kayıt var mı kontrol eder.

        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);
        // Filtre varsa ona göre, yoksa tüm kayıt sayısını döner.

        // --- Sayfalama ---
        Task<List<T>> GetPagedAsync(int pageNumber, int pageSize, bool tracking = true);
        // Sayfalama ile kayıtları getirir. tracking parametresi ile izleme açılıp kapatılabilir.
        // pageNumber ve pageSize invalid olsa da şimdilik exception fırlatılmaz, eski davranış korunur.
    }
}
