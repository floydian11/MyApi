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
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories); // ServiceBase'deki GetAllAsync zaten entity listesi döner
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
            var category = await _categoryService.GetCategoryByNameAsync(name);
            if (category == null) return NotFound();
            return Ok(category);
        }

        // --- Ekleme ---
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryCreateDto dto)
        {
            var created = await _categoryService.AddCategoryAsync(dto);
            return Ok(created);
        }

       // --- Güncelleme ---
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CategoryUpdateDto dto)
        {
            var updated = await _categoryService.UpdateCategoryAsync(id, dto);
            return Ok(updated);
        }
        
        // --- Silme ---
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            return NoContent();
        }

        // --- Aktif/Pasif ---
        [HttpPut("{id}/deactivate")]
        public async Task<IActionResult> Deactivate(Guid id)
        {
            var category = await _categoryService.DeactivateCategoryAsync(id);
            return Ok(category);
        }

        [HttpPut("{id}/activate")]
        public async Task<IActionResult> Activate(Guid id)
        {
            var category = await _categoryService.ActivateCategoryAsync(id);
            return Ok(category);
        }
    }
}
