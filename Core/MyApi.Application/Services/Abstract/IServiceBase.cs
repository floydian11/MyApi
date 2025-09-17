using MyApi.Application.DTOs;
using MyApi.Application.Results.Eski;
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
     
        Task<IDataResult<List<TDto>>> GetAllAsync<TDto>() where TDto : class, IDto;
        Task<IDataResult<TDto?>> GetByIdAsync<TDto>(Guid id) where TDto : class, IDto;
        Task<IDataResult<TDto>> AddAsync<TCreateDto, TDto>(TCreateDto dto) where TDto : class, IDto;
        Task<IDataResult<TDto?>> UpdateAsync<TUpdateDto, TDto>(Guid id, TUpdateDto dto) where TDto : class, IDto;
        Task<IResult> DeleteAsync(Guid id);
        //Ek metotlar
        Task<IDataResult<bool>> ExistsAsync(Guid id);
        Task<IDataResult<int>> CountAsync(Expression<Func<T, bool>>? filter = null);
        Task<IDataResult<List<T>>> FindAsync(Expression<Func<T, bool>> filter);
        Task<IDataResult<T?>> FirstOrDefaultAsync(Expression<Func<T, bool>> filter);
        
    }
}
