using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NewsWebApi.ConfigurationModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NewsWebApi.Services
{
    public class JwtService
    {
        private readonly JwtConfiguration jwtConfiguration;

        public JwtService(IOptions<JwtConfiguration> jwtConfiguration)
        {
            this.jwtConfiguration = jwtConfiguration.Value;
        }
        public string GenerateToken(string email, IEnumerable<string> roles)
        {
            var claims = new List<Claim>
           {
               new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString()),
               new Claim(JwtRegisteredClaimNames.Email, email),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
           };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Generate token
            var token = new JwtSecurityToken(
                issuer: jwtConfiguration.Issuer,
                audience: jwtConfiguration.Audience,
                claims: claims,
                expires: DateTime.Now.AddMonths(1),
                signingCredentials: credentials
            );
            return (new JwtSecurityTokenHandler().WriteToken(token).ToString());
        }
    }
}
