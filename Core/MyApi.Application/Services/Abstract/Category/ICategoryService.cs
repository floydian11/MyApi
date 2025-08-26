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
        Task<List<Category>> GetActiveCategoriesAsync();
        Task<Category?> GetCategoryByNameAsync(string name);

        //Task AddCategoryAsync(Category category);
        //Task UpdateCategoryAsync(Category category);
        //Task DeleteCategoryAsync(Guid id);

    }
}
