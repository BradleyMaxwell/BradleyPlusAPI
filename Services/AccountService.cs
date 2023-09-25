using System;
using System.Security.Principal;
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

		public async Task<ServiceResult> Get (string id)
		{
			try
			{
                Account account = await _context.LoadAsync<Account>(id);
				return ServiceResult.WithData(
					ServiceResultType.FoundSuccess,
					"an account was found",
					account);
            } catch (Exception exception)
			{
				return ServiceResult.WithoutData(
                    ServiceResultType.InternalError,
                    "something went wrong when trying to get the account");
            }
        }

		public async Task<ServiceResult> Create (Account account)
		{
			try
			{
                await _context.SaveAsync(account);
				return ServiceResult.WithoutData(
					ServiceResultType.CreatedSuccess,
					"account was created");
            } catch (Exception exception)
			{
                return ServiceResult.WithoutData(
                    ServiceResultType.CreatedSuccess,
                    "a server error prevented the account from being created");
            }
        }

		public async Task<ServiceResult> Update (Guid id, Account account)
		{
            try
            {
                return ServiceResult.WithoutData(
                    ServiceResultType.CreatedSuccess,
                    "account was created");
            }
            catch (Exception exception)
            {
                return ServiceResult.WithoutData(
                    ServiceResultType.CreatedSuccess,
                    "an error prevented the account from being created");
            }
        }

		public async Task<ServiceResult> Delete (Guid id)
		{
            try
            {
                return ServiceResult.WithoutData(
                    ServiceResultType.CreatedSuccess,
                    "account was created");
            }
            catch (Exception exception)
            {
                return ServiceResult.WithoutData(
                    ServiceResultType.CreatedSuccess,
                    "an error prevented the account from being created");
            }
        }
	}
}

