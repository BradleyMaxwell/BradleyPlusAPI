namespace api.Schemas.Requests
{
	public record CreateAccountRequest(
		string Email,
		string Password
	);
}

