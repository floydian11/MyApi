using MyApi.Application.DTOs.Product;
using MyApi.Application.Repositories;
using MyApi.Application.Results;
using MyApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Services.Abstract
{
    public interface IProductService : IServiceBase<Product>
    {
        Task<IDataResult<List<ProductListDto>>> GetAllProductsAsync();
        Task<IDataResult<ProductListDto?>> GetProductByIdAsync(Guid id);
        Task<IDataResult<ProductResponseDto>> AddProductAsync(ProductCreateDto dto);
        Task<IDataResult<ProductResponseDto?>> UpdateProductAsync(Guid id, ProductUpdateDto dto);
        Task<IDataResult<List<ProductListDto>>> GetProductsByCategoryIdAsync(Guid categoryId);
        Task<IDataResult<List<ProductListDto>>> GetExpensiveProductsAsync(decimal minPrice);
    }
}
