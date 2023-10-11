using api.Models;
using api.Schemas.Requests;
using Microsoft.AspNetCore.Mvc;
using api.Services.Interfaces;
using api.Utility;
using api.Utility.Interfaces;
using api.Schemas.Responses;
using static api.Utility.JwtTokenManager;
using Microsoft.AspNetCore.Authorization;
using System.Net;

namespace api.Controllers
{
    public class AccountsController : ApiController // controls what to do for all the account api endpoints
    {
        private readonly IAccountService _accountService; // used to call the database functionality that is abstracted away from the controller
        private readonly IJwtTokenManager _jwtTokenManager; // used to generate a token if the user successfully logs in
        private readonly string refreshTokenKey = "refreshToken"; // as this value is repeated it was sensible to reduce hard coding it

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

        [AllowAnonymous] // needs to not require authorization because it is the entry point for users into getting authenticated
        [HttpPost("login")]
        public async Task<IActionResult> Login (LoginRequest request) // used for authentication when getting the account
        {
            ServiceResult result = await _accountService.Login(request);
            if (result.Type == ServiceResultType.Success)
            {
                // create a JWT access token for the logged in account and send the account info and token back together 
                Account account = (Account)result.Data; // result.Data is a generic object so it requires type casting, but since it successfully got an account it will work
                string accessToken = _jwtTokenManager.GenerateToken(account.Id, TokenType.Access);
                string refreshToken = _jwtTokenManager.GenerateToken(account.Id, TokenType.Refresh);
                LoginResponse response = new LoginResponse(account.Id, account.Email, account.Password, account.UserIds, accessToken, refreshToken);

                // WOULD DO THIS BUT COOKIES NOT WORKING - send the refresh token which is used to get new access tokens as a http only cookie so it cannot be retrieved by javascript and send
                Response.Cookies.Append(refreshTokenKey, refreshToken);
                return Ok(response);
            } else
            {
                return ErrorResponse(result);
            }
        }

        [Authorize(AuthenticationSchemes = "Refresh")] // refreshing the access token requires a refresh token
        [HttpGet("refresh/{id:guid}")]
        public IActionResult RefreshAccessToken (string id) // endpoint used by the client without the user knowing to get new short term access tokens regularly for security
        {
            // generate a new access token and send it back
            string newAccessToken = _jwtTokenManager.GenerateToken(id, TokenType.Access);
            return Ok(new { accessToken = newAccessToken });
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

