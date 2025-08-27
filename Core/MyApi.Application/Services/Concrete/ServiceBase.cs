using Microsoft.EntityFrameworkCore;
using MyApi.Application.Interfaces;
using MyApi.Application.Repositories;
using MyApi.Application.Results;
using MyApi.Application.Services.Abstract;
using MyApi.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Services.Concrete
{
    public class ServiceBase<T> : IServiceBase<T> where T : BaseEntity
    {
        protected readonly IRepository<T> _repository;
        protected readonly IUnitOfWork _unitOfWork;
        public ServiceBase(IRepository<T> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IResult> AddAsync(T entity)
        {
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return new SuccessResult(); // mesaj opsiyonel, üst katmanda verilecek
        }


        public async Task<IResult> DeleteAsync(T entity)
        {
            if (entity == null)
                return new ErrorResult(); // mesaj üst katmanda verilecek

            await _repository.DeleteAsync(entity);
            await _unitOfWork.CommitAsync();
            return new SuccessResult(); // mesaj opsiyonel
        }

       
        public async Task<IDataResult<List<T>>> GetAllAsync()
        {
            var list = await _repository.GetAll().ToListAsync();
            return new SuccessDataResult<List<T>>(list);
        }
        public async Task<IDataResult<T?>> GetByIdAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity != null
                ? new SuccessDataResult<T?>(entity)
                : new ErrorDataResult<T?>(null);
        }
        public async Task<IResult> UpdateAsync(T entity)
        {
            if (entity == null)
                return new ErrorResult(); // mesaj üst katmanda verilecek

            _repository.Update(entity);
            await _unitOfWork.CommitAsync();
            return new SuccessResult(); // mesaj opsiyonel
        }
        //ek metotlar

        public async Task<IDataResult<bool>> ExistsAsync(Guid id)
        {
            var exists = await _repository.AnyAsync(x => x.Id == id);
            return new SuccessDataResult<bool>(exists);
        }

        public async Task<IDataResult<int>> CountAsync(Expression<Func<T, bool>>? filter = null)
        {
            var count = await _repository.CountAsync(filter);
            return new SuccessDataResult<int>(count);
        }


        public async Task<IDataResult<List<T>>> FindAsync(Expression<Func<T, bool>> filter)
        {
            var list = await _repository.GetWhere(filter).ToListAsync();
            return new SuccessDataResult<List<T>>(list);
        }

        public async Task<IDataResult<T?>> FirstOrDefaultAsync(Expression<Func<T, bool>> filter)
        {
            var entity = await _repository.FirstOrDefaultAsync(filter);
           
            return entity == null
                  ? new ErrorDataResult<T?>(null, "Entity bulunamadı.")  // ← null veriyoruz
                   : new SuccessDataResult<T?>(entity);
        }
    }
}
