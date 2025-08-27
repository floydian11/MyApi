using MyApi.Application.Results;
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
        Task<IDataResult<List<T>>> GetAllAsync();
        Task<IDataResult<T?>> GetByIdAsync(Guid id);
        Task<IResult> AddAsync(T entity);
        Task<IResult> UpdateAsync(T entity);
        Task<IResult> DeleteAsync(T entity);
        //Ek metotlar
        Task<IDataResult<bool>> ExistsAsync(Guid id);
        Task<IDataResult<int>> CountAsync(Expression<Func<T, bool>>? filter = null);
        Task<IDataResult<List<T>>> FindAsync(Expression<Func<T, bool>> filter);
        Task<IDataResult<T?>> FirstOrDefaultAsync(Expression<Func<T, bool>> filter);
        
    }
}
