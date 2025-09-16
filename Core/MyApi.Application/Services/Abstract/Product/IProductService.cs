using MyApi.Application.DTOs.ARServices.Product;
using MyApi.Application.DTOs.Pagination;
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
        Task<IDataResult<List<ProductListDto?>>> GetAllProductsAsync();
        Task<IDataResult<PagedResultDto<ProductListDto>>> GetPagedProductsAsync(PaginationRequestDto request);
        Task<IDataResult<PagedResultDto<ProductListDto>>> GetProductsFilteredAsync(ProductFilterDto filter);
        Task<IDataResult<ProductListDto?>> GetProductByIdAsync(Guid id);
        Task<IDataResult<PagedResultDto<ProductListDto>>> SearchProductsAsync(ProductSearchDto filter);
        Task<IDataResult<ProductResponseDto?>> AddProductAsync(ProductCreateDto dto);
        Task<IDataResult<ProductResponseDto?>> UpdateProductAsync(ProductUpdateDto dto);
        Task<IDataResult<bool>> DeleteProductAsync(Guid id);
        Task<IDataResult<List<ProductListDto>>> GetProductsByCategoryIdAsync(Guid categoryId);
        Task<IDataResult<List<ProductListDto>>> GetExpensiveProductsAsync(decimal minPrice);
    }
}
