using Amazon.DynamoDBv2.DataModel;

namespace api.Models
{
    [DynamoDBTable("Users")]
	public class User
	{
        [DynamoDBHashKey]
        public string Id { get; set; }

        [DynamoDBProperty]
        public string AccountId { get; set; } // the account that the user belongs to

        [DynamoDBProperty]
        public DateTime DateOfBirth { get; set; }

        [DynamoDBProperty]
        public List<string> WatchHistoryIds { get; set; } // a list of the ids of the films that the user has watched

        [DynamoDBProperty]
        public List<string> WatchlistIds { get; set; } // a list of the film ids that the user has marked as films they want to watch
    }
}

