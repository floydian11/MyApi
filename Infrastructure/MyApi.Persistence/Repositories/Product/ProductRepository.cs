using Microsoft.EntityFrameworkCore;
using MyApi.Application.Repositories;
using MyApi.Domain.Entities;
using MyApi.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Persistence.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<int> CountByCategoryIdAsync(Guid categoryId)
        {
            return await _context.Products
                .Where(p => p.Categories.Any(c => c.Id == categoryId))
                .CountAsync();
        }

        public async Task<List<string>> GetProductNamesByCategoryIdAsync(Guid categoryId)
        {
            return await _context.Products
                .Where(p => p.Categories.Any(c => c.Id == categoryId))
                .Select(p => p.Name)
                .ToListAsync();
        }
    }
}

//IProductRepositorty → interface, DI container’ın hangi türü inject edeceğini biliyor.

//Yani ProductRepository hem base class’taki generic metotları kullanıyor hem de IProductRepository olarak DI’ye kaydediliyor.