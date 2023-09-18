namespace api.Schemas.Requests
{
	public record UpdateAccountRequest(
        string Email,
		string Password,
        string? CardNumber,
		DateOnly? CardExpiryDate,
		Subscription? Subscription
	);
}

