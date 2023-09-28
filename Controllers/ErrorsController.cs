using System;
using api.Services;
using api.Utility;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)] // make swagger ui ignore this controller
    public class ErrorsController : ApiController
	{
		[Route("/error")]
		public IActionResult Error() // global exception handler that will return this general server error with any unexpected error with a request
		{
			return Problem(); // returns a 500 internal error response
		}
	}
}

