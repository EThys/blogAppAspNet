using blogApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace blogApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        
        private readonly AppDbContext _context;
        public UserController(AppDbContext context)
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
        public async Task<ActionResult<ApiResponse<IEnumerable<User>>>> GetUsers()
        {

            var users = await _context.Users
                .Include(c => c.Comments)
                .Include(c => c.Posts)
                .ToListAsync();

            var response = new ApiResponse<IEnumerable<User>>
            {
                Success = true,
                Message = "Users récupérées avec succès.",
                Data = users
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<User>>> GetUser(int id)
        {

            var user = await _context.Users
                .Include(c => c.Comments)
                .Include(c => c.Posts)
                .FirstOrDefaultAsync(c => c.UserId == id);

            if (user == null)
            {
                return NotFound(new ApiResponse<Post>
                {
                    Success = false,
                    Message = "User non trouvée.",
                    Data = null
                });
            }

            var response = new ApiResponse<User>
            {
                Success = true,
                Message = "User récupéré avec succès.",
                Data = user
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<ApiResponse<User>> Store(User user)
        {
            if (!ModelState.IsValid)
            {
                return new ApiResponse<User>
                {
                    Success = false,
                    Message = "Les données fournies ne sont pas valides.",
                    Data = null
                };
            }

            if (await _context.Users.AnyAsync(t => t.UserEmail == user.UserEmail))
            {
                return new ApiResponse<User>
                {
                    Success = false,
                    Message = "ce User existe déjà.",
                    Data = null
                };
            }
            _context.Users.Add(user);

            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return new ApiResponse<User>
                {
                    Success = true,
                    Message = "User ajoutée avec succès.",
                    Data = user
                };
            }


            return new ApiResponse<User>
            {
                Success = false,
                Message = "Une erreur est survenue lors de l'ajout d'un User.",
                Data = null
            };
        }
    }
}

