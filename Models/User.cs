namespace api.Models
{
	public class User
	{
		Guid Id { get; }
		Guid Account { get; }
		string Name { get; }
		DateOnly DateOfBirth { get; }
		Film[] Watchlist { get; }
		Film[] WatchHistory { get; }

		public User(Guid id, Guid account, string name, DateOnly dateOfBirth, Film[] watchlist, Film[] watchHistory)
		{
			Id = id;
			Account = account;
			Name = name;
			DateOfBirth = dateOfBirth;
			Watchlist = watchlist;
			WatchHistory = watchHistory;
		}
	}
}

