using Demo.Model;
using Demo.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Demo.Database;

namespace Demo.Repository
{
    public class UserService : IUserDetails
    {
        private readonly DatabaseContext context;
        private readonly IConfiguration configuration;

        public UserService(DatabaseContext _context, IConfiguration _configuration)
        {
            context = _context;
            configuration = _configuration;
        }

        public async Task<string> AddUser(UserDetailsDto userDto)
        {

            if (await context.UserDetails.AnyAsync(u => u.EmailId == userDto.EmailId))
            {
                throw new InvalidOperationException("User already exists with the given email.");
            }

            if (await context.UserDetails.AnyAsync(u => u.Contact == userDto.Contact))
            {
                throw new InvalidOperationException("User already exists with the given mobile number.");
            }

            var user = new UserDetails
            {
                UserName = userDto.UserName,
                EmailId = userDto.EmailId,
                Contact = userDto.Contact,
                Password = userDto.Password,
                UserType = userDto.UserType ?? "Doctor" 
            };

            context.UserDetails.Add(user);

            await context.SaveChangesAsync();

            return "User registered successfully";
        }

        
        public async Task<string> LoginUserAsync(UserLoginDto userLoginDto)
        {
            if (userLoginDto == null)
            {
                throw new ArgumentNullException("User login data is missing.");
            }
            
            var user = await context.UserDetails
                .FirstOrDefaultAsync(u => u.EmailId == userLoginDto.UserName || u.Contact == userLoginDto.UserName);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid Credentials");
            }

            if (user.Password != userLoginDto.Password)
            {
                throw new UnauthorizedAccessException("Invalid Credentials");
            }
                
            var token = GenerateJwtToken(user);
            return token;
        }
        private string GenerateJwtToken(UserDetails user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, configuration["Jwt:Subject"] ?? "default-subject"),
                new Claim(JwtRegisteredClaimNames.Email, user.EmailId),
                new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                new Claim("UserId",user.UserId.ToString()),
                new Claim(ClaimTypes.Role, user.UserType),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = Encoding.ASCII.GetBytes(configuration["Jwt:SecretKey"]);
            var symmetricKey = new SymmetricSecurityKey(key);

            var signingCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddMinutes(60);

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
