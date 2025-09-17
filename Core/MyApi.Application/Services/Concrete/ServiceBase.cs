using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MyApi.Application.DTOs;
using MyApi.Application.Interfaces;
using MyApi.Application.Repositories;
using MyApi.Application.Results;
using MyApi.Application.Results.Eski;
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


        // Generic GetAll
        public virtual async Task<IDataResult<List<TDto>>> GetAllAsync<TDto>()
     where TDto : class, IDto
        {
            var query = _repository.GetAll(tracking: false);
            var dtos = await query
                .ProjectTo<TDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            // İş kuralı yok → genel bir başarı mesajı
            return new SuccessDataResult<List<TDto>>(dtos, "Kayıtlar listelendi");
        }

        // Generic GetById
        public virtual async Task<IDataResult<TDto?>> GetByIdAsync<TDto>(Guid id)
            where TDto : class, IDto
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return new ErrorDataResult<TDto?>(null, "Kayıt bulunamadı.");

            var dto = _mapper.Map<TDto>(entity);
            return new SuccessDataResult<TDto>(dto);
        }

        // Generic Add
        public virtual async Task<IDataResult<TDto>> AddAsync<TCreateDto, TDto>(TCreateDto dto)
            where TDto : class, IDto
        {
            // 1️⃣ DTO'dan Entity'ye mapping
            var entity = _mapper.Map<T>(dto);

            // 2️⃣ Repository üzerinden ekleme
            await _repository.AddAsync(entity);

            // 3️⃣ UnitOfWork ile transaction commit
            await _unitOfWork.CommitAsync();

            // 4️⃣ Entity'i tekrar DTO'ya map et
            var resultDto = _mapper.Map<TDto>(entity);

            // 5️⃣ Başarı mesajı ile IDataResult dön
            return new SuccessDataResult<TDto>(resultDto, "Kayıt başarıyla eklendi.");
        }

        // Generic Update
        public virtual async Task<IDataResult<TDto?>> UpdateAsync<TUpdateDto, TDto>(Guid id, TUpdateDto dto)
            where TDto : class, IDto
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return new ErrorDataResult<TDto?>(null, "Kayıt bulunamadı.");

            _mapper.Map(dto, entity);
            _repository.Update(entity);
            await _unitOfWork.CommitAsync();

            var resultDto = _mapper.Map<TDto>(entity);
            return new SuccessDataResult<TDto?>(resultDto, "Kayıt güncellendi.");
        }
        
        // Generic Delete
        public virtual async Task<IResult> DeleteAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return new ErrorResult("Kayıt bulunamadı.");

            await _repository.DeleteAsync(entity);
            await _unitOfWork.CommitAsync();

            return new SuccessResult("Kayıt silindi.");
        }

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
