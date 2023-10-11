using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api.Utility.Interfaces;
using Microsoft.IdentityModel.Tokens;
using static api.Utility.JwtTokenManager;

namespace api.Utility
{
    public enum TokenType // used by a controller to tell the token manager what type of token they want
    {
        Access,
        Refresh
    }

    public class JwtTokenManager : IJwtTokenManager
    {
        public string GenerateToken(string userId, TokenType tokenType)
        {
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GetSecretKey(tokenType)));
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
                expires: DateTime.Now.AddMinutes(GetDuration(tokenType)),
                signingCredentials: signingCredentials
            );

            // convert the generated token to a string and send back
            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenString;
        }

        public bool VerifyToken(string token, TokenType tokenType) // determine whether a given string is a valid token or not
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            TokenValidationParameters validationParameters = GetTokenValidationParameters(tokenType);
            try
            {
                tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return true;
            } catch (SecurityTokenException) {
                return false;
            }
        }

        public TokenValidationParameters GetTokenValidationParameters (TokenType tokenType) 
        {
            TokenValidationParameters validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(GetSecretKey(tokenType))),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            return validationParameters;
        }
        private int GetDuration (TokenType tokenType) // used to abstract away hard coding the duration of tokens anywhere else
        {
            switch (tokenType)
            {
                case TokenType.Access:
                    return 15; // access tokens need to be short life span so any compromised tokens cannot be used for long
                case TokenType.Refresh:
                    return 24 * 60; // refresh tokens need to be longer life span for user experience so the user does not have to perform constant logins
                default:
                    return 0; // should never get to this point as the parameter is an enum
            }
        }

        private string GetSecretKey (TokenType tokenType) // get the secret key used to sign and verify JWTs
        {
            switch (tokenType)
            {
                case TokenType.Access:
                    return Environment.GetEnvironmentVariable("JWT_ACCESS_SECRET");
                case TokenType.Refresh:
                    return Environment.GetEnvironmentVariable("JWT_REFRESH_SECRET");
                default:
                    return "";
            }
        }
    }
}

