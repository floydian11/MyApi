using Microsoft.EntityFrameworkCore;
using MyApi.Application.Interfaces;
using MyApi.Application.Repositories;
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
        public async Task AddAsync(T entity)
        {
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync(); // DB’ye yansır
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity != null)
                await _repository.DeleteAsync(entity);
            await _unitOfWork.CommitAsync(); // DB’ye yansır
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _repository.GetAll().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            _repository.Update(entity);
            await _unitOfWork.CommitAsync(); // DB’ye yansır
        }

        //ek metotlar

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _repository.AnyAsync(x => x.Id == id);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>>? filter = null)
        {
            return await _repository.CountAsync(filter);
        }

        public async Task<List<T>> FindAsync(Expression<Func<T, bool>> filter)
        {
            return await _repository.GetWhere(filter).ToListAsync();
        }

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> filter)
        {
            return await _repository.FirstOrDefaultAsync(filter);
        }
    }
}
