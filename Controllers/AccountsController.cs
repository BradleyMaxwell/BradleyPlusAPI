using api.Models;
using api.Schemas.Requests;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccount (string id)
        {
            Account account = await _accountService.Get(id);
            return account != null ? Ok(account) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount(CreateAccountRequest request)
        {
            // turn external account request object into an account object
            Account account = new Account()
            {
                Id = Guid.NewGuid().ToString(),
                Email = request.Email,
                Password = request.Password
            };

            // tell the account service provider to create a new account object
            await _accountService.Create(account);

            return CreatedAtAction(
                actionName: nameof(GetAccount), // the name of the function that can retrieve the created resource
                routeValues: new { id = account.Id }, // the required parameter for calling the retrieve resource function
                value: account); // the content being sent back to the frontend when the resource is created
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

