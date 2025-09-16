using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApi.Application.DTOs.ARServices.Product;
using MyApi.Application.DTOs.Pagination;
using MyApi.Application.Services.Abstract;

namespace MyApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var result = await _productService.GetAllProductsAsync();
            return Ok(result);
        }

        // POST: api/Product/search
        [HttpPost("search")]
        public async Task<IActionResult> SearchProducts([FromBody] ProductSearchDto filter)
        {
            var result = await _productService.SearchProductsAsync(filter);
            return Ok(result);
        }

        // GET: api/Product/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var result = await _productService.GetProductByIdAsync(id);
            return Ok(result);
        }

        // GET: api/Product/paged
        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedProducts([FromQuery] PaginationRequestDto request)
        {
            var result = await _productService.GetPagedProductsAsync(request);
            return Ok(result);
        }

        // POST: api/Product/filter
        [HttpPost("filter")]
        public async Task<IActionResult> GetProductsFiltered([FromBody] ProductFilterDto filter)
        {
            var result = await _productService.GetProductsFilteredAsync(filter);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromForm] ProductCreateDto dto)
        {
            var result = await _productService.AddProductAsync(dto);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromForm] ProductUpdateDto dto)
        {
            var result = await _productService.UpdateProductAsync(dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var result = await _productService.DeleteProductAsync(id);
            return Ok(result);
        }

        // GET: api/Product/category/{categoryId}
        [HttpGet("category/{categoryId:guid}")]
        public async Task<IActionResult> GetProductsByCategoryId(Guid categoryId)
        {
            var result = await _productService.GetProductsByCategoryIdAsync(categoryId);
            return Ok(result);
        }

        // GET: api/Product/expensive
        [HttpGet("expensive")]
        public async Task<IActionResult> GetExpensiveProducts([FromQuery] decimal minPrice)
        {
            var result = await _productService.GetExpensiveProductsAsync(minPrice);
            return Ok(result);
        }
    }
}
