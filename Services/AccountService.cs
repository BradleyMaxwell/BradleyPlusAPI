using System;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using api.Models;
using api.Services.Interfaces;

namespace api.Services
{
	public class AccountService : IAccountService
	{
		private readonly IDynamoDBContext _context; // the context in which the database is being used for which in this case is the Accounts table

		public AccountService(IDynamoDBContext context)
		{
			_context = context;
		}

		public async Task<Account> Get (string id)
		{
			return await _context.LoadAsync<Account>(id);
        }

		public async Task Create (Account account)
		{
			await _context.SaveAsync(account);
        }

		public void Update (Guid id, Account account)
		{

		}

		public void Delete (Guid id)
		{

		}
	}
}

