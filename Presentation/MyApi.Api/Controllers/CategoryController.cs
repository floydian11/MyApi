using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null) return NotFound();
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Category category)
        {
            await _categoryService.AddCategoryAsync(category); // Commit burada
            return Ok(category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Category category)
        {
            var existing = await _categoryService.GetByIdAsync(id);
            if (existing == null) return NotFound();

            existing.Name = category.Name;
            existing.IsActive = category.IsActive;

            await _categoryService.UpdateAsync(existing);
            return Ok(existing);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existing = await _categoryService.GetByIdAsync(id);
            if (existing == null) return NotFound();

            await _categoryService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveCategories()
        {
            var categories = await _categoryService.GetActiveCategoriesAsync();
            return Ok(categories);
        }
    }
}
