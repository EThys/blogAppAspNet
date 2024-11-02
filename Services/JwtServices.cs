using blogApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace blogApp.Services
{
    public class JwtServices
    {
        private readonly AppDbContext  _context;
        private readonly IConfiguration  _configuration;

        public JwtServices(AppDbContext? context, IConfiguration? configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<UserLoginModel?> Authentificate(UserLoginModel request)
        {

            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return null;

            var userAccount = await _context.Users.FirstOrDefaultAsync(x => x.UserName == request.Username);
            byte[] storedSalt = Convert.FromBase64String(userAccount.UserSalt);

            if (userAccount == null)
                return null;
           
            if (!PasswordHasher.VerifyPasswordHash(request.Password, Convert.FromBase64String(userAccount.UserPassword), storedSalt))
                 return null;

            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var key = _configuration["Jwt:Key"];
            var tokenValidiTyMins = _configuration.GetValue<int>("Jwt:TokenValidityMins");
            var tokenExpirityTimesTamp = DateTime.UtcNow.AddMinutes(tokenValidiTyMins);

            var tokenDescriptor = new SecurityTokenDescriptor

            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(JwtRegisteredClaimNames.Name,request.Username)
                }),
                Expires = tokenExpirityTimesTamp,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);

            return new UserLoginModel
            {
                AccessToken = accessToken,
                Username = request.Username,
                ExpiresIn = (int)tokenExpirityTimesTamp.Subtract(DateTime.UtcNow).TotalSeconds
            };
        }


              

            


    }


}
