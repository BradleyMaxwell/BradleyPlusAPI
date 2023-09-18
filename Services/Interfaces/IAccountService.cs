using api.Models;

namespace api.Services.Interfaces
{
	public interface IAccountService : IService
	{
		void Create(Account account);
		void Update(Guid id, Account account);
		void Delete(Guid id);
	}
}

