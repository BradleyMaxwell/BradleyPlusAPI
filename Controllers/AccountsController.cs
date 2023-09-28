using api.Models;
using api.Schemas.Requests;
using Microsoft.AspNetCore.Mvc;
using api.Services.Interfaces;
using api.Utility;

namespace api.Controllers
{
    public class AccountsController : ApiController // controls what to do for all the account api endpoints
    {
        private readonly IAccountService _accountService; // used to call the database functionality that is abstracted away from the controller

        public AccountsController (IAccountService accountService)
        {
            _accountService = accountService; // dependancy injection
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccount (string id)
        {
            ServiceResult result = await _accountService.Get(id);
            return result.Type == ServiceResultType.Success ? Ok(result.Data) : ErrorResponse(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login (LoginRequest request) // used for authentication when getting the account
        {
            ServiceResult result = await _accountService.Login(request);
            return result.Type == ServiceResultType.Success ? Ok(result.Data) : ErrorResponse(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount (CreateAccountRequest request)
        {
            // turn external account request object into an account object
            Account account = new Account()
            {
                Id = Guid.NewGuid().ToString(),
                Email = request.Email,
                Password = request.Password
            };

            // tell the account service provider to create a new account object and return the appropriate http response based on service result
            ServiceResult result = await _accountService.Create(account);

            if (result.Type == ServiceResultType.Success)
            {
                return CreatedAtAction(
                actionName: nameof(GetAccount), // the name of the function that can retrieve the created resource
                routeValues: new { id = account.Id }, // the required parameter for calling the retrieve resource function
                value: account); // the content being sent back to the frontend when the resource is created
            }
            else
            {
                return ErrorResponse(result);
            }
        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> UpdateAccount (string id, UpdateAccountRequest request)
        {
            Account account = new Account()
            {
                Id = id,
                Email = request.Email,
                Password = request.Password,
                UserIds = request.UserIds
            };
            ServiceResult updateResult = await _accountService.Update(id, account);
            return updateResult.Type == ServiceResultType.Success ? NoContent() : ErrorResponse(updateResult);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAccount (string id)
        {
            ServiceResult result = await _accountService.Delete(id);
            return result.Type == ServiceResultType.Success ? NoContent() : ErrorResponse(result);
        }
    }
}

