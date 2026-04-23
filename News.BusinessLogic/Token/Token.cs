using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace News.BusinessLogic.Token
{
    public class Token(IConfiguration configuration)
    {
        private readonly string? _secureKey = configuration["JwtSettings:SecretKey"];

        public string GenerateUserToken(string email, Guid userId, string? userName = null) {
            var claims = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier, userId.ToString()),  
                new (ClaimTypes.Email, email),                     
                new (ClaimTypes.Name, userName ?? email),           
                new (ClaimTypes.Role, "User"),
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) 
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secureKey!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            
            var token = new JwtSecurityToken(
                issuer: "ETN-News",      
                audience: "ETN-Users", 
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: credentials
            );
            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        
        public ClaimsPrincipal Verify(string jwt)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secureKey);
            
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,      
                ValidIssuer = "ETN-News",
                ValidateAudience = true,    
                ValidAudience = "ETN-Users",
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            
            try
            {
                var principal = tokenHandler.ValidateToken(jwt, validationParameters, out _);
                return principal;
            }
            catch (Exception ex)
            {
                throw new SecurityTokenException("Invalid token", ex);
            }
        }
    }
}