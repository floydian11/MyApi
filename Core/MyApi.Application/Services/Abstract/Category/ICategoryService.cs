using MyApi.Application.DTOs.Category;
using MyApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Services.Abstract
{
    public interface ICategoryService : IServiceBase<Category>
    {
        Task<List<CategoryReadDto>> GetActiveCategoriesAsync();
        Task<CategoryReadDto?> GetCategoryByNameAsync(string name);
        Task<CategoryReadDto> AddCategoryAsync(CategoryCreateDto dto);
        Task<CategoryReadDto> UpdateCategoryAsync(Guid id, CategoryUpdateDto dto);
        Task DeleteCategoryAsync(Guid id);
        Task<CategoryReadDto> DeactivateCategoryAsync(Guid id);
        Task<CategoryReadDto> ActivateCategoryAsync(Guid id);
    }
}
