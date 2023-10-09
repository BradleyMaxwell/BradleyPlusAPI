using System;
using static api.Utility.JwtTokenManager;

namespace api.Utility.Interfaces
{
	public interface IJwtTokenManager
	{
		public string GenerateToken(string id, TokenType tokenType);
		public bool VerifyToken(string token);
	}
}

