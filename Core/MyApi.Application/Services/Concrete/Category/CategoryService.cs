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
        private readonly IProductRepository _productRepository;
        public CategoryService(IRepository<Category> repository, IUnitOfWork unitOfWork, IMapper mapper, ILogger<CategoryService> logger, IProductRepository productRepository)
            : base(repository, unitOfWork, mapper)
        {
            _logger = logger;
            _productRepository = productRepository;
           
        }

        // ===== Base Service Versiyonu =====
        // Basit CRUD işlemleri için BaseService kullanıyoruz.

        // Tüm kategorileri listele
        public async Task<IDataResult<List<CategoryReadDto>>> GetAllCategoriesAsync()
        {
            return await GetAllAsync<CategoryReadDto>();
        }

        // Id ile kategori getir
        public async Task<IDataResult<CategoryReadDto?>> GetCategoryByIdAsync(Guid id)
        {
            var result = await GetByIdAsync<CategoryReadDto>(id);
            if (!result.Success)
                return new ErrorDataResult<CategoryReadDto>(default!, "Kategori bulunamadı.");

            return result;
        }
        // ===== Custom Repository Versiyonu =====
        // Hesaplanmış alan, navigation property veya özel business rule varsa
        // BaseService yerine repository + manual mapping kullanıyoruz.

        // Sadece aktif kategorileri getir
        // ===== Custom Repository Versiyonu =====
        public async Task<IDataResult<List<CategoryReadDto>>> GetActiveCategoriesAsync()
        {
            var query = _repository.GetAll(tracking: false)
                .Where(c => c.IsActive);

            var dtos = new List<CategoryReadDto>();
            foreach (var category in await query.ToListAsync())
            {
                var productCount = await _productRepository.CountByCategoryIdAsync(category.Id);
                dtos.Add(new CategoryReadDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    IsActive = category.IsActive,
                    ProductCount = productCount
                });
            }

            return new SuccessDataResult<List<CategoryReadDto>>(dtos, "Aktif kategoriler listelendi");
        }

        // Ad ile kategori getir, manuel mapping çünkü ProductCount var
        public async Task<IDataResult<CategoryReadDto?>> GetCategoryByNameAsync(string name)
        {
            var category = await _repository.FirstOrDefaultAsync(c => c.Name == name);
            if (category == null)
                return new ErrorDataResult<CategoryReadDto?>(null, "Kategori bulunamadı.");

            var productCount = await _productRepository.CountByCategoryIdAsync(category.Id);

            var dto = new CategoryReadDto
            {
                Id = category.Id,
                Name = category.Name,
                IsActive = category.IsActive,
                ProductCount = productCount
            };

            return new SuccessDataResult<CategoryReadDto>(dto);
        }

        // Kategori ekle (business rules var, hesaplanmış alan yok)
        public async Task<IDataResult<CategoryReadDto?>> AddCategoryAsync(CategoryCreateDto dto)
        {
            // Business Rule 1: Name validation
            if (string.IsNullOrWhiteSpace(dto.Name) || dto.Name.Length > 50)
                return new ErrorDataResult<CategoryReadDto?>(null, "Kategori ismi boş olamaz ve 50 karakterden uzun olamaz.");

            // Business Rule 2: Duplicate name check
            if (await _repository.AnyAsync(c => c.Name == dto.Name))
                return new ErrorDataResult<CategoryReadDto?>(null, "Bu isimde kategori zaten mevcut.");

            // BaseService AddAsync kullanabiliriz çünkü hesaplanmış alan yok
            var result = await AddAsync<CategoryCreateDto, CategoryReadDto>(dto);

            // Mesajı override edelim
            return new SuccessDataResult<CategoryReadDto>(result.Data, "Kategori başarıyla eklendi.");
        }

        // Kategori güncelle
        public async Task<IDataResult<CategoryReadDto?>> UpdateCategoryAsync(Guid id, CategoryUpdateDto dto)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null)
                return new ErrorDataResult<CategoryReadDto?>(null, "Kategori bulunamadı.");

            if (!category.IsActive && !dto.IsAdminAction)
                return new ErrorDataResult<CategoryReadDto?>(null, "Pasif kategoriler sadece admin tarafından güncellenebilir.");

            // Business Rule: Duplicate name check (exclude current)
            if (await _repository.AnyAsync(c => c.Name == dto.Name && c.Id != id))
                return new ErrorDataResult<CategoryReadDto?>(null, "Bu isimde başka kategori zaten mevcut.");

            category.Name = dto.Name;
            category.IsActive = dto.IsActive;
            _repository.Update(category);
            await _unitOfWork.CommitAsync();

            // ProductCount artık ProductRepository üzerinden alınıyor
            var productCount = await _productRepository.CountByCategoryIdAsync(category.Id);

            var resultDto = new CategoryReadDto
            {
                Id = category.Id,
                Name = category.Name,
                IsActive = category.IsActive,
                ProductCount = productCount
            };

            return new SuccessDataResult<CategoryReadDto>(resultDto, "Kategori güncellendi.");
        }

        // Kategori sil
        public async Task<IResult> DeleteCategoryAsync(Guid id)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null)
                return new ErrorResult("Kategori bulunamadı.");

            // Business rules: navigation property kontrolü
            if (category.Products.Count > 0)
                return new ErrorResult("Bu kategoriye bağlı ürünler olduğu için silinemez.");

            var activeCount = await _repository.CountAsync(c => c.IsActive);
            if (category.IsActive && activeCount <= 1)
                return new ErrorResult("En az bir kategori her zaman aktif olmalı.");

            // BaseService DeleteAsync kullanılabilir ama navigation property olduğu için repository ile manuel
            await _repository.DeleteAsync(category);
            await _unitOfWork.CommitAsync();

            return new SuccessResult("Kategori silindi.");
        }

        // Pasif yap
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

            var productCount = await _productRepository.CountByCategoryIdAsync(category.Id);

            var dto = new CategoryReadDto
            {
                Id = category.Id,
                Name = category.Name,
                IsActive = category.IsActive,
                ProductCount = productCount
            };

            return new SuccessDataResult<CategoryReadDto>(dto, "Kategori pasif hale getirildi.");
        }

        // Aktif yap
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

            var productCount = await _productRepository.CountByCategoryIdAsync(category.Id);

            var dto = new CategoryReadDto
            {
                Id = category.Id,
                Name = category.Name,
                IsActive = category.IsActive,
                ProductCount = productCount
            };

            return new SuccessDataResult<CategoryReadDto>(dto, "Kategori aktif hale getirildi.");
        }

        // Kategori ve ürünlerini getir
        // Kategori ve ürünlerini getir
        public async Task<IDataResult<CategoryWithProductsDto?>> GetCategoryByNameWithProductsAsync(string name)
        {
            var category = await _repository.FirstOrDefaultAsync(c => c.Name == name);

            if (category == null)
                return new ErrorDataResult<CategoryWithProductsDto?>(null, "Kategori bulunamadı.");

            // Ürün isimleri artık ProductRepository üzerinden alınıyor
            var productNames = await _productRepository.GetProductNamesByCategoryIdAsync(category.Id);

            var dto = new CategoryWithProductsDto
            {
                Id = category.Id,
                Name = category.Name,
                IsActive = category.IsActive,
                ProductNames = productNames
            };

            return new SuccessDataResult<CategoryWithProductsDto>(dto, "Kategori ve ürünleri getirildi.");
        }
    }
}
