namespace api.Schemas.Requests
{
	public record UpdateAccountRequest(
        string Email,
		string Password,
		List<string> UserIds
	);
}

