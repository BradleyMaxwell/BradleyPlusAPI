using System;
using Amazon.DynamoDBv2.DataModel;
using api.Schemas;

namespace api.Models
{
	[DynamoDBTable("Accounts")]
    public class Account
	{
		Guid Id { get; }
		string Email { get; }
		string Password { get; }
        string? CardNumber { get; }
		DateOnly? CardExpiryDate { get; }
		//Subscription? Subscription { get; }

		public Account(Guid id, string email, string password, string? cardNumber, DateOnly? cardExpiryDate)
		{
			Id = id;
			Email = email;
			Password = password;
            CardNumber = cardNumber;
			CardExpiryDate = cardExpiryDate;
			//Subscription = subscription;
		}
	}
}

