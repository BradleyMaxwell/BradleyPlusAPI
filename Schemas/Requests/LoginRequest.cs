using System;
namespace api.Schemas.Requests
{
	public record LoginRequest(
		string Email,
		string Password);
}

