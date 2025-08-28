using AutoMapper;
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
        protected readonly IMapper _mapper;
        public ServiceBase(IRepository<T> repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public virtual async Task<IResult> AddAsync<TCreateDto>(TCreateDto dto)
        {
            var entity = _mapper.Map<T>(dto);
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return new SuccessResult(); // mesaj üst katmanda
        }

        public virtual async Task<IResult> DeleteAsync(T entity)
        {
            if (entity == null)
                return new ErrorResult();

            await _repository.DeleteAsync(entity);
            await _unitOfWork.CommitAsync();
            return new SuccessResult();
        }

        public virtual async Task<IDataResult<List<TDto>>> GetAllAsync<TDto>()
        {
            var entities = await _repository.GetAll().ToListAsync();
            var dtos = _mapper.Map<List<TDto>>(entities);
            return new SuccessDataResult<List<TDto>>(dtos);
        }
        public virtual async Task<IDataResult<TDto>> GetByIdAsync<TDto>(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);

            if (entity == null)
                return new ErrorDataResult<TDto>(default!); // mesaj üst katmanda

            var dto = _mapper.Map<TDto>(entity);
            return new SuccessDataResult<TDto>(dto);
        }

        public virtual async Task<IResult> UpdateAsync<TUpdateDto>(TUpdateDto dto)
        {
            var entity = _mapper.Map<T>(dto);
            if (entity == null)
                return new ErrorResult(); // mesaj üst katmanda

            _repository.Update(entity);
            await _unitOfWork.CommitAsync();
            return new SuccessResult();
        }
        //ek metotlar

        public virtual async Task<IDataResult<bool>> ExistsAsync(Guid id)
        {
            var exists = await _repository.AnyAsync(x => x.Id == id);
            return new SuccessDataResult<bool>(exists);
        }


        public virtual async Task<IDataResult<int>> CountAsync(Expression<Func<T, bool>>? filter = null)
        {
            var count = await _repository.CountAsync(filter);
            return new SuccessDataResult<int>(count);
        }


        public virtual async Task<IDataResult<List<T>>> FindAsync(Expression<Func<T, bool>> filter)
        {
            var list = await _repository.GetWhere(filter).ToListAsync();
            return new SuccessDataResult<List<T>>(list);
        }

        public virtual async Task<IDataResult<T?>> FirstOrDefaultAsync(Expression<Func<T, bool>> filter)
        {
            var entity = await _repository.FirstOrDefaultAsync(filter);

            return entity == null
                ? new ErrorDataResult<T?>(null)  // mesaj üst katmanda
                : new SuccessDataResult<T?>(entity);
        }
    }
}
