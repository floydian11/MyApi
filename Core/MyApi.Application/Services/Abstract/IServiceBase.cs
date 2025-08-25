using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Services.Abstract
{
    public interface IServiceBase<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(Guid id);
       //Ek metotlar
        Task<bool> ExistsAsync(Guid id);
        Task<int> CountAsync(Expression<Func<T, bool>>? filter = null);
        Task<List<T>> FindAsync(Expression<Func<T, bool>> filter);

        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> filter);
    }
}
