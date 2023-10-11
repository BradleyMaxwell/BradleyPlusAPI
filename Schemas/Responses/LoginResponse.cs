using System;
namespace api.Schemas.Responses
{
	public record LoginResponse( // a login response differs to an account response because it requires an additional field for the access token
		string Id,
		string Email,
		string Password,
        List<string> UserIds,
		string accessToken,
		string refreshToken // only using refresh token like this because using it as a HttpOnly cookie is not working
        );
}

