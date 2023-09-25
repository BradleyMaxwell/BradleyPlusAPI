using api.Models;

namespace api.Services.Interfaces
{
	public interface IAccountService
	{
        Task<ServiceResult> Get(string id);
		Task<ServiceResult> Create(Account account);
        Task<ServiceResult> Update(Guid id, Account account);
        Task<ServiceResult> Delete(Guid id);
	}
}

