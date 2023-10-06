using api.Models;
using api.Schemas.Requests;
using Microsoft.AspNetCore.Mvc;
using api.Services.Interfaces;
using api.Utility;
using api.Utility.Interfaces;
using api.Schemas.Responses;

namespace api.Controllers
{
    public class AccountsController : ApiController // controls what to do for all the account api endpoints
    {
        private readonly IAccountService _accountService; // used to call the database functionality that is abstracted away from the controller
        private readonly IJwtTokenManager _jwtTokenManager; // used to generate a token if the user successfully logs in

        public AccountsController (IAccountService accountService, IJwtTokenManager jwtTokenManager)
        {
            // dependancy injection
            _accountService = accountService;
            _jwtTokenManager = jwtTokenManager;
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
            if (result.Type == ServiceResultType.Success)
            {
                // create a JWT access token for the logged in account and send the account info and token back together 
                Account account = (Account)result.Data; // result.Data is a generic object so it requires type casting, but since it successfully got an account it will work
                string accessToken = _jwtTokenManager.GenerateToken(account.Id, 30);
                LoginResponse response = new LoginResponse(account.Id, account.Email, account.Password, account.UserIds, accessToken);
                return Ok(response);
            } else
            {
                return ErrorResponse(result);
            }
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

