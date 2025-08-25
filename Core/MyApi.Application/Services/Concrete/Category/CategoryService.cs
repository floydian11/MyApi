using Microsoft.EntityFrameworkCore;
using MyApi.Application.Interfaces;
using MyApi.Application.Repositories;
using MyApi.Application.Services.Abstract;
using MyApi.Domain.Entities;

namespace MyApi.Application.Services.Concrete
{
    public class CategoryService : ServiceBase<Category>, ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryService(IUnitOfWork unitOfWork, ICategoryRepository categoryRepository)
            : base(categoryRepository)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<Category>> GetActiveCategoriesAsync()
        {
            // IRepository.GetWhere kullanımı
            return await _repository
                .GetWhere(c => c.IsActive)
                .ToListAsync();
        }

        public async Task<Category?> GetCategoryByNameAsync(string name)
        {
            // IRepository.FirstOrDefaultAsync kullanımı
            return await _repository.FirstOrDefaultAsync(c => c.Name == name);
        }

        // Tek kategori ekleme
        public async Task AddCategoryAsync(Category category)
        {
            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.CommitAsync();
        }

        // Tek kategori güncelleme
        public async Task UpdateCategoryAsync(Category category)
        {
            _unitOfWork.Categories.Update(category);
            await _unitOfWork.CommitAsync();
        }

        // Tek kategori silme
        public async Task DeleteCategoryAsync(Guid id)
        {
            var entity = await _unitOfWork.Categories.GetByIdAsync(id);
            if (entity != null)
            {
                await _unitOfWork.Categories.DeleteAsync(entity);
                await _unitOfWork.CommitAsync();
            }
        }
    }
}
