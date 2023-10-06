using System;
namespace api.Utility.Interfaces
{
	public interface IJwtTokenManager
	{
		public string GenerateToken(string id, int duration);
		public bool VerifyToken();
	}
}

