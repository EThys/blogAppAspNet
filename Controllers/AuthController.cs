using blogApp.Models;
using blogApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace blogApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;
        private readonly JwtServices _jwtServices;
        private readonly ILogger<AuthController> _logger;

        public class ApiResponse<T>
        {
            public bool Success { get; set; }
            public string? Message { get; set; }
            public T? Data { get; set; }
            public string? DebugInfo { get; set; } // Nouvelle propriété pour le débogage
        }

        public AuthController(IConfiguration configuration, AppDbContext context, JwtServices jwtServices, ILogger<AuthController> logger)
        {
            _configuration = configuration;
            _context = context;
            _logger = logger;
            _jwtServices = jwtServices;
        }

        [HttpPost("register")]
        public async Task<ApiResponse<User>> Register([FromBody] User user)
        {
           
            if (string.IsNullOrWhiteSpace(user.UserName) || string.IsNullOrWhiteSpace(user.UserPassword))
                return  new ApiResponse<User>
                {
                    Success = false,
                    Message = "Username and password are required.",
                    Data = null,

                };
 
            if (await _context.Users.AnyAsync(u => u.UserName == user.UserName))
                return new ApiResponse<User>
                {
                    Success = false,
                    Message = "Username already exists.",
                    Data = null
                };
           
            PasswordHasher.CreatePasswordHash(user.UserPassword, out byte[] passwordHash, out byte[] passwordSalt);

            // Création d'un nouvel utilisateur
            var newUser = new User
            {
                UserName = user.UserName,
                UserPassword = Convert.ToBase64String(passwordHash),
                UserEmail=user.UserEmail,
                UserCountry=user.UserCountry,
                UserState=user.UserState,
                UserSalt = Convert.ToBase64String(passwordSalt)
            };

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return new ApiResponse<User>
            {
                Success = true,
                Message = "User registered successfully.",
                Data = newUser
            };
        }

        [HttpPost("login")]
        public async Task<ApiResponse<UserLoginModel>> Login([FromBody] UserLoginModel userLogin)
        {
            if (string.IsNullOrWhiteSpace(userLogin.Username) || string.IsNullOrWhiteSpace(userLogin.Password))
                return new ApiResponse<UserLoginModel>
                {
                    Success = false,
                    Message = "Username and password are required.",
                    Data = null,

                };

            var userAccount = await _context.Users.FirstOrDefaultAsync(x => x.UserName == userLogin.Username);
            _logger.LogInformation($"User account fetched: {userAccount}");
 
            if (userAccount == null)
            {
                return new ApiResponse<UserLoginModel>
                {
                    Success = false,
                    Message = "Invalid username",
                    Data = null,
                    DebugInfo = $"Username attempted: {userLogin.Username}" // Ajoutez cette ligne
                };
            }

            byte[] storedSalt = Convert.FromBase64String(userAccount.UserSalt);
            _logger.LogInformation($"UBINGOOOOOOOOOOOOOOOO: {storedSalt.Length}");

            if (!PasswordHasher.VerifyPasswordHash(userLogin.Password, Convert.FromBase64String(userAccount.UserPassword), storedSalt))
            {
                return new ApiResponse<UserLoginModel>
                {
                    Success = false,
                    Message = "Invalid password.",
                    Data = null
                };
            }

           
            var tokenResponse = await _jwtServices.Authentificate(userLogin);
            _logger.LogInformation($"Obama: {tokenResponse}");
           

            if (tokenResponse == null)
            {
                return new ApiResponse<UserLoginModel>
                {
                    Success = false,
                    Message = "Authentication failed.",
                    Data = null
                };
            }
            userAccount.AccessToken = tokenResponse?.AccessToken;
            

            _context.Users.Update(userAccount); 
            await _context.SaveChangesAsync();

            return new ApiResponse<UserLoginModel>
            {
                Success = true,
                Message = "Authentication successful.",
                Data = tokenResponse
            };
        }
        [Authorize]
        [HttpPost("changePassword")]
        public async Task<ApiResponse<User>> ChangePassword([FromBody] ChangePasswordModel changePasswordModel)
        {
            if (string.IsNullOrWhiteSpace(changePasswordModel.Username) ||
                string.IsNullOrWhiteSpace(changePasswordModel.NewPassword))
            {
                return new ApiResponse<User>
                {
                    Success = false,
                    Message = "Username and new password are required.",
                    Data = null
                };
            }

            var userAccount = await _context.Users.FirstOrDefaultAsync(x => x.UserName == changePasswordModel.Username);

            if (userAccount == null)
            {
                return new ApiResponse<User>
                {
                    Success = false,
                    Message = "Invalid username.",
                    Data = null
                };
            }

           
            PasswordHasher.CreatePasswordHash(changePasswordModel.NewPassword, out byte[] newPasswordHash, out byte[] newPasswordSalt);

            userAccount.UserPassword = Convert.ToBase64String(newPasswordHash);
            userAccount.UserSalt = Convert.ToBase64String(newPasswordSalt);

            _context.Users.Update(userAccount);
            await _context.SaveChangesAsync();

            return new ApiResponse<User>
            {
                Success = true,
                Message = "Password changed successfully.",
                Data = userAccount
            };
        }
    }
}