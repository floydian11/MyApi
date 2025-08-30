using MyApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<int> CountByCategoryIdAsync(Guid categoryId);
        Task<List<string>> GetProductNamesByCategoryIdAsync(Guid categoryId);
    }
}
