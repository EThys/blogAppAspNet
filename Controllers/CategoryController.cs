using blogApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace blogApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        public class ApiResponse<T>
        {
            public bool Success { get; set; }
            public string? Message { get; set; }
            public T? Data { get; set; }
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<Category>>>> GetCategories()
        {
            //var categories = await _context.Categories.ToListAsync();
            var categories = await _context.Categories
                .Include(c => c.Posts) // Inclure les posts associés
                .ToListAsync();

            var response = new ApiResponse<IEnumerable<Category>>
            {
                Success = true,
                Message = "Categories récupérées avec succès.",
                Data = categories
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Category>>> GetCategory(int id)
        {
            //var category = await _context.Categories.FindAsync(id);
            var category = await _context.Categories
                .Include(c => c.Posts) // Inclure les posts associés
                .FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category == null)
            {
                return NotFound(new ApiResponse<Category>
                {
                    Success = false,
                    Message = "Categorie non trouvée.",
                    Data = null
                });
            }

            var response = new ApiResponse<Category>
            {
                Success = true,
                Message = "Categorie récupérée avec succès.",
                Data = category
            };

            return Ok(response);
        }
        [HttpPost]
        public async Task<ApiResponse<Category>> Store(Category category)
        {
            if (!ModelState.IsValid)
            {
                return new ApiResponse<Category>
                {
                    Success = false,
                    Message = "Les données fournies ne sont pas valides.",
                    Data = null
                };
            }

            if (await _context.Categories.AnyAsync(t => t.CategoryTitle == category.CategoryTitle))
            {
                return new ApiResponse<Category>
                {
                    Success = false,
                    Message = "Un élément avec ce titre existe déjà.",
                    Data = null
                };
            }

            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
            category.CreatedAt = currentDate;
            _context.Categories.Add(category);

            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return new ApiResponse<Category>
                {
                    Success = true,
                    Message = "La categorie est ajoutée avec succès.",
                    Data = category
                };
            }


            return new ApiResponse<Category>
            {
                Success = false,
                Message = "Une erreur est survenue lors de l'ajout de la Categorie.",
                Data = null
            };
        }
        [HttpPut("{id}")]
        public async Task<ApiResponse<Category>> PutTask(int id, Category category)
        {

            ModelState.Clear();
            var existingCategory = await _context.Categories.FindAsync(id);
            if (existingCategory == null)
            {
                return new ApiResponse<Category>
                {
                    Success = false,
                    Message = "Categorie non trouvée.",
                    Data = null
                };
            }


            existingCategory.CategoryTitle = category.CategoryTitle;
            existingCategory.CategoryDescription = category.CategoryDescription;
            

            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return new ApiResponse<Category>
                {
                    Success = true,
                    Message = "Categorie mise à jour avec succès.",
                    Data = existingCategory
                };
            }

            return new ApiResponse<Category>
            {
                Success = false,
                Message = "Une erreur est survenue lors de la mise à jour de la Categorie.",
                Data = null
            };
        }

    }
}
