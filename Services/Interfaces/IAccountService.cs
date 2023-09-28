using api.Models;
using api.Schemas.Requests;
using api.Utility;

namespace api.Services.Interfaces
{
	public interface IAccountService
	{
        Task<ServiceResult> Get(string id);
		Task<ServiceResult> Login(LoginRequest request);
		Task<ServiceResult> Create(Account account);
        Task<ServiceResult> Update(string id, Account account);
        Task<ServiceResult> Delete(string id);
	}
}

