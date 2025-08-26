using Microsoft.EntityFrameworkCore;
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
        public async Task<List<Category>> GetActiveCategoriesAsync()
        {
            // Active olan kategorileri getir
            return await _repository.GetWhere(c => c.IsActive).ToListAsync();
        }

        public async Task<Category?> GetCategoryByNameAsync(string name)
        {
            // İsme göre kategori getir
            return await _repository.FirstOrDefaultAsync(c => c.Name == name);
        }
    }
}
