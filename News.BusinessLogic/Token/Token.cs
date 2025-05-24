using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace News.BusinessLogic.Token;

public class Token
{
    private readonly string _secureKey = "a very very very important secure key";

    public string GenerateUserToken(string name)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, name),
            new(ClaimTypes.Role, "User")
        };


        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secureKey));
        var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Today.AddDays(1),
            signingCredentials: credentials
        );
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }

    public string GenerateAdminToken(string name)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, name),
            new(ClaimTypes.Role, "Admin")
        };


        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secureKey));
        var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Today.AddDays(1),
            signingCredentials: credentials
        );
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }

    public JwtSecurityToken Verify(string jwt)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_secureKey);
        tokenHandler.ValidateToken(jwt, new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuerSigningKey = true,
            ValidateIssuer = false,
            ValidateAudience = false
        }, out var validatedToken);

        return (JwtSecurityToken)validatedToken;
    }
}