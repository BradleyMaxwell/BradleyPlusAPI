using Amazon.DynamoDBv2.DataModel;

namespace api.Models
{
	public class Film
	{
		[DynamoDBHashKey]
		public string Id { get; set; }

		[DynamoDBProperty]
		public string Title { get; set; }

		[DynamoDBProperty]
		public int Duration { get; set; } // duration in minutes

        [DynamoDBProperty]
        public string Description { get; set; }

        [DynamoDBProperty]
        public int AgeRating { get; set; }

        [DynamoDBProperty]
        public DateTime ReleaseDate { get; set; }

        [DynamoDBProperty]
        public DateTime DateAdded { get; set; }

        [DynamoDBProperty]
        public string ThumbnailId { get; set; } // the unique id which is the filename of the thumbnail in cloud storage
    }
}

