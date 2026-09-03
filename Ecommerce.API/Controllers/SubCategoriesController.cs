using ECommerce.DataAccess.Data;
using ECommerce.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubCategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SubCategoriesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<SubCategory>> AddSubCategory(SubCategory subCategory)
        {
            await _context.SubCategories.AddAsync(subCategory);
            await _context.SaveChangesAsync();

            return Ok(subCategory);
        }

        [HttpGet]
        public async Task<ActionResult<List<SubCategory>>> GetAllSubCategories()
        {
            var subCategories = await _context.SubCategories
                .Include(s => s.Category)
                .ToListAsync();

            return Ok(subCategories);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<SubCategory>> GetSubCategoryById(int id)
        {
            var subCategory = await _context.SubCategories
                .Include(s => s.Category)
                .FirstOrDefaultAsync(s => s.SubCategoryId == id);

            if (subCategory == null)
            {
                return NotFound();
            }

            return Ok(subCategory);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<SubCategory>> UpdateSubCategory(
    int id, SubCategory subCategory)
        {
            if (id != subCategory.SubCategoryId)
            {
                return BadRequest();
            }

            _context.SubCategories.Update(subCategory);
            await _context.SaveChangesAsync();

            return Ok(subCategory);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubCategory(int id)
        {
            var subCategory = await _context.SubCategories.FindAsync(id);

            if (subCategory == null)
            {
                return NotFound();
            }

            _context.SubCategories.Remove(subCategory);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
