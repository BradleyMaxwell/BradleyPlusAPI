using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api.Utility.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace api.Utility
{
    public class JwtTokenManager : IJwtTokenManager
    {
        public string GenerateToken(string userId, int durationMinutes)
        {
            string secretKey = Environment.GetEnvironmentVariable("JWT_SECRET");
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            Claim[] claims = new[] // list of labels that securely go inside the JWT that describes information about it
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId), // the subject of the JWT 
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // a unique ID for the JWT itself
            };

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: "https://localhost:7186", // this would be replaced with the production server url
                audience: "http://localhost:3000", // the url where this token will be sent to
                claims: claims,
                expires: DateTime.Now.AddMinutes(durationMinutes),
                signingCredentials: signingCredentials
            );

            // convert the generated token to a string and send back
            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenString;
        }

        public bool VerifyToken()
        {
            return true;
        }
    }
}

