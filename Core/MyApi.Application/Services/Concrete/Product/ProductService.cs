using Microsoft.EntityFrameworkCore;
using MyApi.Application.Repositories;
using MyApi.Application.Services.Abstract;
using MyApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Services.Concrete
{    
    public class ProductService : ServiceBase<Product>, IProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository) : base(productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<Product>> GetProductsByCategoryIdAsync(Guid categoryId)
        {
            // IRepository.GetWhere kullanımı
            return await _repository
         .GetWhere(p => p.Categories.Any(c => c.Id == categoryId))
         .ToListAsync();
        }

        public async Task<List<Product>> GetExpensiveProductsAsync(decimal minPrice)
        {
            // IRepository.GetWhere + ordering
            return await _repository
                .GetWhere(p => p.Price >= minPrice)
                .OrderByDescending(p => p.Price)
                .ToListAsync();
        }
    }
}
