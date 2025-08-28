using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyApi.Application.DTOs.Category;
using MyApi.Application.Interfaces;
using MyApi.Application.Repositories;
using MyApi.Application.Results;
using MyApi.Application.Services.Abstract;
using MyApi.Domain.Entities;

namespace MyApi.Application.Services.Concrete
{
    public class CategoryService : ServiceBase<Category>, ICategoryService
    {
        ILogger<CategoryService> _logger;
        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CategoryService> logger)
            : base(unitOfWork.Categories, unitOfWork, mapper)
        {
            _logger = logger;   
        }
        
        public async Task<IDataResult<List<CategoryReadDto>>> GetActiveCategoriesAsync()
        {
            var query = _repository.GetWhere(c => c.IsActive);

            var dtoList = await query
                .Select(c => new CategoryReadDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    IsActive = c.IsActive,
                    ProductCount = c.Products.Count
                })
                .ToListAsync();

            return new SuccessDataResult<List<CategoryReadDto>>(dtoList);
        }

        
        public async Task<IDataResult<CategoryReadDto?>> GetCategoryByNameAsync(string name)
        {
            var category = await _repository.FirstOrDefaultAsync(c => c.Name == name);
            if (category == null)
                return new ErrorDataResult<CategoryReadDto?>(null, "Kategori bulunamadı.");

            var dto = new CategoryReadDto
            {
                Id = category.Id,
                Name = category.Name,
                IsActive = category.IsActive,
                ProductCount = category.Products.Count
            };

            return new SuccessDataResult<CategoryReadDto>(dto);
        }

        // 3️⃣ Yeni kategori ekleme

        public async Task<IDataResult<CategoryReadDto?>> AddCategoryAsync(CategoryCreateDto dto)
        {
            // İş Kural 2: Aynı isimde kategori eklenemez
            if (await _repository.AnyAsync(c => c.Name == dto.Name))
                return new ErrorDataResult<CategoryReadDto?>(null, "Kategori bulunamadı.");

            // İş Kural 4: İsim boş olamaz ve max 50 karakter
            if (string.IsNullOrWhiteSpace(dto.Name) || dto.Name.Length > 50)
                return new ErrorDataResult<CategoryReadDto?>(null, "Kategori ismi boş olamaz ve 50 karakterden uzun olamaz.");

            var category = new Category
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                IsActive = dto.IsActive
            };

            await _repository.AddAsync(category);
            await _unitOfWork.CommitAsync();

            var resultDto = new CategoryReadDto
            {
                Id = category.Id,
                Name = category.Name,
                IsActive = category.IsActive,
                ProductCount = 0
            };

            return new SuccessDataResult<CategoryReadDto>(resultDto, "Kategori başarıyla eklendi.");
        }
        // 4️⃣ Kategori güncelleme
        
        public async Task<IDataResult<CategoryReadDto?>> UpdateCategoryAsync(Guid id, CategoryUpdateDto dto)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null)
                return new ErrorDataResult<CategoryReadDto?>(null, "Kategori bulunamadı.");

            if (!category.IsActive && !dto.IsAdminAction)
                return new ErrorDataResult<CategoryReadDto?>(null, "Pasif kategoriler sadece admin tarafından güncellenebilir.");

            category.Name = dto.Name;
            category.IsActive = dto.IsActive;

            await UpdateAsync(category);

            var resultDto = new CategoryReadDto
            {
                Id = category.Id,
                Name = category.Name,
                IsActive = category.IsActive,
                ProductCount = category.Products.Count
            };

            return new SuccessDataResult<CategoryReadDto>(resultDto, "Kategori güncellendi.");
        }
        // 5️⃣ Kategori silme
       
        public async Task<IResult> DeleteCategoryAsync(Guid id)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null)
                return new ErrorResult("Kategori bulunamadı.");

            if (category.Products.Count > 0)
                return new ErrorResult("Bu kategoriye bağlı ürünler olduğu için silinemez.");

            var activeCount = await _repository.CountAsync(c => c.IsActive);
            if (category.IsActive && activeCount <= 1)
                return new ErrorResult("En az bir kategori her zaman aktif olmalı.");

            await DeleteAsync(category);
            return new SuccessResult("Kategori silindi.");
        }
        public async Task<IDataResult<CategoryReadDto?>> DeactivateCategoryAsync(Guid id, bool isAdminAction)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null)
                return new ErrorDataResult<CategoryReadDto?>(null, "Kategori bulunamadı.");

            var activeCount = await _repository.CountAsync(c => c.IsActive);
            if (category.IsActive && activeCount <= 1)
                return new ErrorDataResult<CategoryReadDto?>(null, "En az bir kategori her zaman aktif olmalıdır.");

            if (!isAdminAction)
                return new ErrorDataResult<CategoryReadDto?>(null, "Pasifleştirme işlemi sadece admin tarafından yapılabilir.");

            category.IsActive = false;
            _repository.Update(category);
            await _unitOfWork.CommitAsync();

            var dto = new CategoryReadDto
            {
                Id = category.Id,
                Name = category.Name,
                IsActive = category.IsActive,
                ProductCount = category.Products.Count
            };

            return new SuccessDataResult<CategoryReadDto>(dto, "Kategori pasif hale getirildi.");
        }


        public async Task<IDataResult<CategoryReadDto?>> ActivateCategoryAsync(Guid id)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null)
                return new ErrorDataResult<CategoryReadDto?>(null, "Kategori bulunamadı.");

            if (category.IsActive)
                return new ErrorDataResult<CategoryReadDto?>(null, "Kategori zaten aktif.");

            category.IsActive = true;
            _repository.Update(category);
            await _unitOfWork.CommitAsync();

            var dto = new CategoryReadDto
            {
                Id = category.Id,
                Name = category.Name,
                IsActive = category.IsActive,
                ProductCount = category.Products.Count
            };

            return new SuccessDataResult<CategoryReadDto>(dto, "Kategori aktif hale getirildi.");
        }

        public async Task<IDataResult<CategoryWithProductsDto?>> GetCategoryByNameWithProductsAsync(string name)
        {
            var category = await _repository.FirstOrDefaultAsync(c => c.Name == name);

            if (category == null)
                return new ErrorDataResult<CategoryWithProductsDto?>(null, "Kategori bulunamadı.");

            var dto = new CategoryWithProductsDto
            {
                Id = category.Id,
                Name = category.Name,
                IsActive = category.IsActive,
                ProductNames = category.Products.Select(p => p.Name).ToList()
            };

            return new SuccessDataResult<CategoryWithProductsDto>(dto, "Kategori ve ürünleri getirildi.");
        }
    }
}
