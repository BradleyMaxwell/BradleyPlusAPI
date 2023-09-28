using Amazon.DynamoDBv2.DataModel;

namespace api.Models
{
	[DynamoDBTable("Accounts")]
	public class Account
	{
		[DynamoDBHashKey]
		public string Id { get; set; }

		[DynamoDBProperty]
		public string Email { get; set; }

        [DynamoDBProperty]
        public string Password { get; set; }

		[DynamoDBProperty]
		public List<string> UserIds { get; set; } // the user ids that are linked to a given account
	}
}

