using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProductTrackingAPI.Utils
{
    public class TokenWriter
    {
        private IConfiguration _configuration;

        
        public TokenWriter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        
        public string GenerateToken(List<Claim> claims)
        {
            

            var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    expires: DateTime.Now.AddMinutes(double.Parse(_configuration["Jwt:ExpiryMinutes"]!)),
                    claims: claims,
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)),
                    SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ClaimsPrincipal? ValidateToken(string token)
        {
            
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = false,
                //ValidAudience = audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)),
                ValidateLifetime = true, // Ensures token hasn't expired
                ClockSkew = TimeSpan.Zero, // Adjust if needed for token clock tolerance

            }; ;

            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                // Validate token and return claims principal if successful
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
                return principal;
            }
            catch (Exception)
            {
                // Token is invalid
                return null;
            }
        }

        
    }
}
