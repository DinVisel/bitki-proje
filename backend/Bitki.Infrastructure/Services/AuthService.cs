using Bitki.Core.Interfaces.Services;
using Bitki.Core.Interfaces.Repositories.Auth;
using Bitki.Core.Entities;

namespace Bitki.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthUserRepository _userRepository;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;

        public AuthService(IAuthUserRepository userRepository, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<(string Token, string Role, string Username)?> LoginAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null)
            {
                // Hardcoded fallback ONLY for development transition (remove in production)
                // This ensures we can still login if DB is empty before Seeding runs or fails
                if (username == "admin" && password == "admin") return await GenerateJwt("admin", "Admin");
                if (username == "user" && password == "user") return await GenerateJwt("user", "User");

                return null;
            }

            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return null;
            }

            return await GenerateJwt(user.Username, user.Role);
        }

        public async Task<bool> RegisterAsync(string username, string password)
        {
            var existingUser = await _userRepository.GetByUsernameAsync(username);
            if (existingUser != null) return false;

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            var user = new User
            {
                Username = username,
                PasswordHash = passwordHash,
                Role = "User" // Default role
            };

            await _userRepository.CreateAsync(user);
            return true;
        }

        private Task<(string Token, string Role, string Username)?> GenerateJwt(string username, string role)
        {
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var key = System.Text.Encoding.ASCII.GetBytes(_configuration["JwtSettings:SecretKey"]!);
            var tokenDescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, username),
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"],
                SigningCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key), Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Task.FromResult<(string Token, string Role, string Username)?>((tokenString, role, username));
        }
    }
}
