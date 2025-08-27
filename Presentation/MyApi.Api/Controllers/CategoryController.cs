using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApi.Application.DTOs.Category;
using MyApi.Application.Services.Abstract;
using MyApi.Domain.Entities;

namespace MyApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

       
        // --- Listeleme ---
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _categoryService.GetAllAsync();

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }


        [HttpGet("active")]
        public async Task<IActionResult> GetActiveCategories()
        {
            var categories = await _categoryService.GetActiveCategoriesAsync();
            return Ok(categories); // DTO tabanlı, iş kuralları uygulanmış
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null) return NotFound();
            return Ok(category);
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var result = await _categoryService.GetCategoryByNameWithProductsAsync(name);
            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result);
        }

        // --- Ekleme ---
        
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CategoryCreateDto dto)
        {
            var result = await _categoryService.AddCategoryAsync(dto);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }

        // --- Güncelleme --
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CategoryUpdateDto dto)
        {
            var result = await _categoryService.UpdateCategoryAsync(id, dto);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }

        // --- Silme ---
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _categoryService.DeleteCategoryAsync(id);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }

        // --- Aktif/Pasif ---
        [HttpPatch("deactivate/{id}")]
        public async Task<IActionResult> Deactivate(Guid id, bool isAdminAction)
        {
            var result = await _categoryService.DeactivateCategoryAsync(id, isAdminAction);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPatch("activate/{id}")]
        public async Task<IActionResult> Activate(Guid id)
        {
            var result = await _categoryService.ActivateCategoryAsync(id);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }
    }
}
