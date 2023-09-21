using api.Models;

namespace api.Services.Interfaces
{
	public interface IAccountService
	{
        Task<Account> Get(string id);
		Task Create(Account account);
		void Update(Guid id, Account account);
		void Delete(Guid id);
	}
}

