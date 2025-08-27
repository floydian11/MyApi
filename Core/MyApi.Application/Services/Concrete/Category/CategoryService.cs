using Microsoft.EntityFrameworkCore;
using MyApi.Application.DTOs.Category;
using MyApi.Application.Interfaces;
using MyApi.Application.Repositories;
using MyApi.Application.Services.Abstract;
using MyApi.Domain.Entities;

namespace MyApi.Application.Services.Concrete
{
    public class CategoryService : ServiceBase<Category>, ICategoryService
    {
        public CategoryService(IUnitOfWork unitOfWork)
            : base(unitOfWork.Categories, unitOfWork)
        {
        }
        public async Task<List<CategoryReadDto>> GetActiveCategoriesAsync()
        {
            // Active olan kategorileri getir DTO öncesi
            //return await _repository.GetWhere(c => c.IsActive).ToListAsync();
            //DTO sonrası
            // IQueryable<Category> al
            var query = _repository.GetWhere(c => c.IsActive);

            // DTO'ya map et ve veritabanında sorguyu çalıştır
            var dtoList = await query
                .Select(c => new CategoryReadDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    IsActive = c.IsActive,
                    ProductCount = c.Products.Count
                })
                .ToListAsync();

            return dtoList;

        }

        public async Task<CategoryReadDto?> GetCategoryByNameAsync(string name)
        {
            // İsme göre kategori getir
            var category = await _repository.FirstOrDefaultAsync(c => c.Name == name);
            if (category == null) return null;

            return new CategoryReadDto
            {
                Id = category.Id,
                Name = category.Name,
                IsActive = category.IsActive,
                ProductCount = category.Products.Count
            };
        }

        // 3️⃣ Yeni kategori ekleme
        public async Task<CategoryReadDto> AddCategoryAsync(CategoryCreateDto dto)
        {
            // İş Kural 2: Aynı isimde kategori eklenemez
            if (await _repository.AnyAsync(c => c.Name == dto.Name))
                throw new InvalidOperationException("Bu isimde bir kategori zaten mevcut.");

            // İş Kural 4: İsim boş olamaz ve max 50 karakter
            if (string.IsNullOrWhiteSpace(dto.Name) || dto.Name.Length > 50)
                throw new InvalidOperationException("Kategori ismi boş olamaz ve 50 karakterden uzun olamaz.");

            var category = new Category
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                IsActive = dto.IsActive
            };

            await _repository.AddAsync(category);
            await _unitOfWork.CommitAsync(); // ServiceBase'den geliyor

            return new CategoryReadDto
            {
                Id = category.Id,
                Name = category.Name,
                IsActive = category.IsActive,
                ProductCount = 0
            };
        }

        // 4️⃣ Kategori güncelleme
        public async Task<CategoryReadDto> UpdateCategoryAsync(Guid id, CategoryUpdateDto dto)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null) throw new KeyNotFoundException("Kategori bulunamadı.");

            // İş Kural 3: Aktif olmayan kategoriler sadece admin tarafından güncellenebilir
            if (!category.IsActive && !dto.IsAdminAction)
                throw new InvalidOperationException("Pasif kategoriler sadece admin tarafından güncellenebilir.");

            // İş Kural 4: İsim boş olamaz ve max 50 karakter
            if (string.IsNullOrWhiteSpace(dto.Name) || dto.Name.Length > 50)
                throw new InvalidOperationException("Kategori ismi boş olamaz ve 50 karakterden uzun olamaz.");

            category.Name = dto.Name;
            category.IsActive = dto.IsActive;

            await UpdateAsync(category); // ServiceBase.UpdateAsync kullanılıyor

            return new CategoryReadDto
            {
                Id = category.Id,
                Name = category.Name,
                IsActive = category.IsActive,
                ProductCount = category.Products.Count
            };
        }

        // 5️⃣ Kategori silme
        public async Task DeleteCategoryAsync(Guid id)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null) throw new KeyNotFoundException("Kategori bulunamadı.");

            // İş Kural 1: Eğer ürünlerde kullanılıyorsa silinemez
            if (category.Products.Any())
                throw new InvalidOperationException("Bu kategoriye bağlı ürünler olduğu için silinemez.");

            // İş Kural 5: En az bir kategori her zaman aktif olmalı
            var activeCount = await _repository.CountAsync(c => c.IsActive);
            if (category.IsActive && activeCount <= 1)
                throw new InvalidOperationException("En az bir kategori her zaman aktif olmalı.");

            await DeleteAsync(id); // ServiceBase.DeleteAsync kullanılıyor
        }

        public async Task<CategoryReadDto> DeactivateCategoryAsync(Guid id)
        {
            // 1️⃣ Kategori var mı kontrol et
            var category = await _repository.GetByIdAsync(id);
            if (category == null)
                throw new Exception("Kategori bulunamadı.");

            // 2️⃣ İş kuralı: En az bir kategori aktif kalmalı
            var activeCount = await _repository.CountAsync(c => c.IsActive);
            if (category.IsActive && activeCount <= 1)
                throw new Exception("En az bir kategori her zaman aktif olmalıdır.");

            // 3️⃣ İş kuralı: Sadece admin pasifleştirebilir
            if (!category.IsAdminAction) // örnek kural
                throw new Exception("Pasifleştirme işlemi sadece admin tarafından yapılabilir.");

            // 4️⃣ Kategoriyi pasif yap
            category.IsActive = false;

            // 5️⃣ DB’ye yansıt
            _repository.Update(category);
            await _unitOfWork.CommitAsync();

            // 6️⃣ DTO’ya map et
            return new CategoryReadDto
            {
                Id = category.Id,
                Name = category.Name,
                IsActive = category.IsActive,
                ProductCount = category.Products.Count
            };
        }

        public async Task<CategoryReadDto> ActivateCategoryAsync(Guid id)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null)
                throw new Exception("Kategori bulunamadı.");

            if (category.IsActive)
                throw new Exception("Kategori zaten aktif.");

            category.IsActive = true;

            _repository.Update(category);
            await _unitOfWork.CommitAsync();

            return new CategoryReadDto
            {
                Id = category.Id,
                Name = category.Name,
                IsActive = category.IsActive,
                ProductCount = category.Products.Count
            };
        }
    }
}
