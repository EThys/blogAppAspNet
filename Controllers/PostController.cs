using blogApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static blogApp.Controllers.CategoryController;

namespace blogApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {

        private readonly AppDbContext _context;
        public PostController(AppDbContext context)
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
        public async Task<ActionResult<ApiResponse<IEnumerable<Post>>>> GetPosts()
        {

            var posts = await _context.Posts
                .Include(c => c.Category)
                .Include(c => c.User)
                .Include(c => c.Comments)
                .ToListAsync();

            var response = new ApiResponse<IEnumerable<Post>>
            {
                Success = true,
                Message = "Posts récupérées avec succès.",
                Data = posts
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Post>>> GetPost(int id)
        {

            var post = await _context.Posts
                .Include(c => c.Category)
                .Include(c => c.User)
                .Include(c => c.Comments)
                .FirstOrDefaultAsync(c => c.PostId == id);

            if (post == null)
            {
                return NotFound(new ApiResponse<Post>
                {
                    Success = false,
                    Message = "Publication non trouvée.",
                    Data = null
                });
            }

            var response = new ApiResponse<Post>
            {
                Success = true,
                Message = "Publication récupérée avec succès.",
                Data = post
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<ApiResponse<Post>> Store(Post post)
        {
            if (!ModelState.IsValid)
            {
                return new ApiResponse<Post>
                {
                    Success = false,
                    Message = "Les données fournies ne sont pas valides.",
                    Data = null
                };
            }

            if (await _context.Posts.AnyAsync(t => t.PostTitle == post.PostTitle))
            {
                return new ApiResponse<Post>
                {
                    Success = false,
                    Message = "Un élément avec ce titre existe déjà.",
                    Data = null
                };
            }

            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
            post.CreatedAt = currentDate;
            _context.Posts.Add(post);

            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return new ApiResponse<Post>
                {
                    Success = true,
                    Message = "Publication ajoutée avec succès.",
                    Data = post
                };
            }

            return new ApiResponse<Post>
            {
                Success = false,
                Message = "Une erreur est survenue lors de l'ajout de la publication.",
                Data = null
            };
        }
    }
}
