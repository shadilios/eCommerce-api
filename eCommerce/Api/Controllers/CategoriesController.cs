using AutoMapper;
using eCommerce.Api.Dtos;
using eCommerce.Core.Entities;
using eCommerce.Core.Interfaces;
using eCommerce.Core.Specifications;
using eCommerce.Data;
using eCommerce.Data.Roles;
using eCommerce.Services.CategoriesService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {


        private readonly IGenericRepository<Category> categoriesRepo;
        private readonly IMapper mapper;

        public CategoriesController(IGenericRepository<Category> categoriesRepo, IMapper mapper)
        {

            this.categoriesRepo = categoriesRepo;
            this.mapper = mapper;
        }

        // GET: api/Categories
        [HttpGet]
        //[Route("GetCategoriesBasic")]
        public async Task<ActionResult<IReadOnlyList<Category>>> GetCategories()
        {
            var categories = await categoriesRepo.ListAllAsync();

            if (categories == null)
            {
                return NotFound();
            }
            return Ok(categories);
        }

        // GET: api/Categories
        [HttpGet]
        [Route("GetCategoriesBasicDto")]
        public async Task<ActionResult<IReadOnlyList<Category>>> GetCategoriesDto()
        {
            var categories = await categoriesRepo.ListAllAsync();

            if (categories == null)
            {
                return NotFound();
            }
            return Ok(mapper
                .Map<IReadOnlyList<Category>, IReadOnlyList<CategoryToReturnDto>>(categories));
        }



        // GET: api/Categories/GetCategoriesWithProducts
        [HttpGet]
        [Route("GetCategoriesWithProducts")]
        public async Task<ActionResult<IReadOnlyList<Category>>> GetCategoriesWithProducts()
        {
            var spec = new CategoriesWithProductsSpecification();

            var categories = await categoriesRepo.ListEntityWithSpecAsync(spec);

            if (categories == null)
            {
                return NotFound();
            }
            return Ok(categories);
        }

        // GET: api/Categories/5
        [HttpGet]
        [Route("GetCategory/{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = categoriesRepo.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        //GET: api/Categories/5
        [HttpGet]
        [Route("GetCategoryByIdWithProducts/{id}")]
        public async Task<ActionResult<Category>> GetCategoryWithProducts(int id)
        {
            var spec = new CategoriesWithProductsSpecification(id);

            var category = categoriesRepo.GetEntityWithSpecAsync(spec);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        // GET: api/Categories/5
        [HttpGet]
        [Route("GetCategoryByIdWithProductsDto/{id}")]
        public async Task<ActionResult<CategoryToReturnDto>> GetCategoryWithProductsDto(int id)
        {
            var spec = new CategoriesWithProductsSpecification(id);

            var category = await categoriesRepo.GetEntityWithSpecAsync(spec);

            if (category == null)
            {
                return NotFound();
            }

            var dto = mapper.Map<Category, CategoryToReturnDto>(category);

            return Ok(dto);
        }

        // PUT: api/Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Admin + "," + Roles.Editor)]
        public async Task<IActionResult> PutCategory(int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }
           await categoriesRepo.Put(id, category);

            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        //// POST: api/Categories
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //[Authorize(Roles = Roles.Admin)]

        //public async Task<ActionResult<Category>> PostCategory(Category category)
        //{
        //    if (_context.Categories == null)
        //    {
        //        return Problem("Entity set 'AppDbContext.Categories'  is null.");
        //    }
        //    _context.Categories.Add(category);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetCategory", new { id = category.Id }, category);
        //}

        //// DELETE: api/Categories/5
        //[HttpDelete("{id}")]
        //[Authorize(Roles = Roles.Admin)]
        //public async Task<IActionResult> DeleteCategory(int id)
        //{
        //    if (_context.Categories == null)
        //    {
        //        return NotFound();
        //    }
        //    var category = await _context.Categories.FindAsync(id);
        //    if (category == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Categories.Remove(category);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool CategoryExists(int id)
        //{
        //    return (_context.Categories?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}
