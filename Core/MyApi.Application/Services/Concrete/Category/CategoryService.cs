using MyApi.Application.Repositories;
using MyApi.Application.Services.Abstract;
using MyApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MyApi.Application.Services.Concrete
{
    public class CategoryService : ServiceBase<Category>, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository) : base(categoryRepository)
        {
            _categoryRepository = categoryRepository;
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
    }
}
