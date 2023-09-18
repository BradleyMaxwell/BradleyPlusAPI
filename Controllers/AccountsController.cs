using api.Models;
using api.Schemas.Requests;
using Microsoft.AspNetCore.Mvc;
using Amazon.DynamoDBv2;
using api.Services.Interfaces;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountsController : ControllerBase // controls what to do for all the account api endpoints
    {
        private readonly IAccountService _accountService; // used to call the database functionality that is abstracted away from the controller

        public AccountsController (IAccountService accountService)
        {
            _accountService = accountService; // dependancy injection
        }

        [HttpGet("{id:guid}")]
        public IActionResult GetAccount (Guid id)
        {
            return Ok(id);
        }

        [HttpPost]
        public IActionResult CreateAccount (CreateAccountRequest request)
        {
            // turn external account request object into an account object
            Account account = new Account(
                Guid.NewGuid(),
                request.Email,
                request.Password,
                null,
                null
            );

            // tell the account service provider to create a new account object
            _accountService.Create(account);

            return Ok(account);
        }

        [HttpPatch("{id:guid}")]
        public IActionResult UpdateAccount (Guid id, UpdateAccountRequest request)
        {
            return Ok(id);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteAccount (Guid id)
        {
            return Ok(id);
        }
    }
}

