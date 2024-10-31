using blogApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace blogApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CommentController(AppDbContext context)
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
        public async Task<ActionResult<ApiResponse<IEnumerable<Comment>>>> GetComments()
        {
           
            var comments = await _context.Comments
                .Include(c => c.Post)
                .Include(c => c.User)
                .ToListAsync();


            var response = new ApiResponse<IEnumerable<Comment>>
            {
                Success = true,
                Message = "Commentaires récupérées avec succès.",
                Data = comments
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Comment>>> GetComment(int id)
        {
           
            var comment = await _context.Comments
                .Include(c => c.Post)
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.CommentId == id);

            if (comment == null)
            {
                return NotFound(new ApiResponse<Comment>
                {
                    Success = false,
                    Message = "Commentaire non trouvée.",
                    Data = null
                });
            }

            var response = new ApiResponse<Comment>
            {
                Success = true,
                Message = "Commentaire récupérée avec succès.",
                Data = comment
            };

            return Ok(response);
        }
        [HttpPost]
        public async Task<ApiResponse<Comment>> Store(Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return new ApiResponse<Comment>
                {
                    Success = false,
                    Message = "Les données fournies ne sont pas valides.",
                    Data = null
                };
            }

            DateTime currentDate = DateTime.Now;
            comment.CreatedAt = currentDate;
            _context.Comments.Add(comment);

            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return new ApiResponse<Comment>
                {
                    Success = true,
                    Message = "Le commentaire est ajoutée avec succès.",
                    Data = comment
                };
            }


            return new ApiResponse<Comment>
            {
                Success = false,
                Message = "Une erreur est survenue lors de l'ajout du commentaire.",
                Data = null
            };
        }
    }

}
