namespace api.Models
{
	public enum Genre
	{

	}

	public class Film
	{
		Guid Id { get; }
		string Title { get; }
		Genre[] Genres { get; }
		TimeSpan Duration { get; }
		string Description { get; }
		int AgeRating { get; }
		DateOnly ReleaseDate { get; }
        DateTime DateAdded { get; }
		// need to add thumbnail image attribute

		public Film (Guid id, string title, Genre[] genres, TimeSpan duration, string description, int ageRating, DateOnly releaseDate, DateTime dateAdded)
		{
			Id = id;
			Title = title;
			Genres = genres;
			Duration = duration;
			Description = description;
			AgeRating = ageRating;
			ReleaseDate = releaseDate;
			DateAdded = dateAdded;
		}
	}
}

