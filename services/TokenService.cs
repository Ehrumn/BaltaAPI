using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Blog.services;

public class TokenService
{
    public string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);
        var tokeDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[] { 
                new (ClaimTypes.Name, "rafaelcaetanodresch"), //User.Identity.Name
                new (ClaimTypes.Role, "admin"),//User.IsInRole
                new (ClaimTypes.Role, "user"),//User.IsInRole
                new ("fruta", "banana")
            }),
            Expires = DateTime.UtcNow.AddHours(8),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokeDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
