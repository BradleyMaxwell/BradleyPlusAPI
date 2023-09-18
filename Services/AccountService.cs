using System;
using Amazon.DynamoDBv2.DataModel;
using api.Models;
using api.Services.Interfaces;

namespace api.Services
{
	public class AccountService : IAccountService
	{
		//private readonly IDynamoDBContext _dbContext; // used to connect to DynamoDB

		//public AccountService (IDynamoDBContext dbContext)
		//{
		//	_dbContext = dbContext; // dependancy injection
		//}
		public void Get (Guid id)
		{
		}

		public void Create (Account account)
		{

		}

		public void Update (Guid id, Account account)
		{

		}

		public void Delete (Guid id)
		{

		}
	}
}

