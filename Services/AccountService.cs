using System.Security.Principal;
using Amazon.DynamoDBv2.DataModel;
using api.Models;
using api.Schemas.Requests;
using api.Services.Interfaces;
using api.Utility;

namespace api.Services
{
	public class AccountService : IAccountService
	{
		private readonly IDynamoDBContext _context; // the context in which the database is being used for which in this case is the Accounts table

		public AccountService(IDynamoDBContext context)
		{
			_context = context;
		}

		public async Task<ServiceResult> Get (string id) // get an account from Accounts table
		{
            Account account = await _context.LoadAsync<Account>(id);
            if (account != null)
            {
                return ServiceResult.WithData(ServiceResultType.Success, "an account was found", account);
            } else
            {
                return ServiceResult.WithoutData(ServiceResultType.NotFoundError, "no account was found with that Id");
            }
        }

        public async Task<ServiceResult> Login (LoginRequest request) // used for getting account from Accounts with login credentials
        {
            List<Account> accountsWithSameEmail = await _context.QueryAsync<Account>(
                request.Email,
                new DynamoDBOperationConfig { IndexName = "Email-index" }).GetRemainingAsync();

            // if there is not exactly 1 account with the same email then there is a problem
            if (accountsWithSameEmail.Count < 1)
            {
                return ServiceResult.WithoutData(ServiceResultType.NotFoundError, "no account with that email was found");
            } else if (accountsWithSameEmail.Count > 1)
            {
                return ServiceResult.WithoutData(ServiceResultType.ConflictError, "there was a mistake on our end where there is more than one account with that email. Please contact customer support");
            }

            // verify that the password given matches the password in the account found
            Account account = accountsWithSameEmail.Single();
            if (PasswordHasher.Verify(request.Password, account.Password) == false)
            {
                return ServiceResult.WithoutData(ServiceResultType.UnauthorizedError, "password was incorrect");
            }

            return ServiceResult.WithData(ServiceResultType.Success, "login successfully authenticated", account);
        }

		public async Task<ServiceResult> Create (Account account) // create new account record in Accounts table
		{
            // make sure the given credentials are valid
            if (account.Email.Length < 1 || account.Password.Length < 1)
            {
                return ServiceResult.WithoutData(ServiceResultType.ValidationError, "make sure both email and password are filled out");
            }

            // make sure that no other account has the same email
            List<Account> accountsWithSameEmail = await _context.QueryAsync<Account>(
                account.Email,
                new DynamoDBOperationConfig { IndexName = "Email-index" }).GetRemainingAsync();
            if (accountsWithSameEmail.Any())
            {
                return ServiceResult.WithoutData(ServiceResultType.ConflictError, "that email is already taken");
            }

            // encrypt the password and create new item in table
            account.Password = PasswordHasher.Encrypt(account.Password);    
            await _context.SaveAsync(account);
			return ServiceResult.WithoutData(ServiceResultType.Success, "account was created");
        }

		public async Task<ServiceResult> Update (string id, Account account) // overwrite the data in an existing Accounts record
        {
            // check that the account to be updated exists in the table
            Account existingAccount = await _context.LoadAsync<Account>(id);
            if (existingAccount == null)
            {
                return ServiceResult.WithoutData(ServiceResultType.NotFoundError, "no account with that Id was found");
            }

            // update the attributes of the existing account with the update request account fields and save changes
            existingAccount = account;
            await _context.SaveAsync(existingAccount);
            return ServiceResult.WithoutData(ServiceResultType.Success, "account was updated");
        }

		public async Task<ServiceResult> Delete (string id) // remove a record from the Accounts table
		{
            // check account exists before attempting to delete
            Account account = await _context.LoadAsync<Account>(id);
            if (account == null)
            {
                return ServiceResult.WithoutData(ServiceResultType.NotFoundError, "could not find an account with that Id to delete");
            }

            await _context.DeleteAsync(account);
            return ServiceResult.WithoutData(ServiceResultType.Success, "account was deleted");
        }
	}
}

