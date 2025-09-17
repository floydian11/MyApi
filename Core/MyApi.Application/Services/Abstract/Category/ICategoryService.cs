using MyApi.Application.DTOs.ARServices.Category;
using MyApi.Application.Results.Eski;
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
        Task<IDataResult<List<CategoryReadDto>>> GetActiveCategoriesAsync();
        //Task<List<CategoryReadDto>> GetActiveCategoriesAsync(); Result öncesi
        Task<IDataResult<CategoryReadDto?>> GetCategoryByNameAsync(string name);
        Task<IDataResult<CategoryReadDto?>> AddCategoryAsync(CategoryCreateDto dto);
        Task<IDataResult<CategoryReadDto?>> UpdateCategoryAsync(Guid id, CategoryUpdateDto dto);
        Task<IResult> DeleteCategoryAsync(Guid id);
        Task<IDataResult<CategoryReadDto?>> DeactivateCategoryAsync(Guid id, bool isAdminAction);
        Task<IDataResult<CategoryReadDto?>> ActivateCategoryAsync(Guid id);
        Task<IDataResult<CategoryWithProductsDto?>> GetCategoryByNameWithProductsAsync(string name);
    }
}
