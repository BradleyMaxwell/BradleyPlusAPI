using System;
using api.Utility;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public abstract class ApiController : ControllerBase
	{
		protected IActionResult ErrorResponse (ServiceResult result) // used to make it easy to return the correct error object by controllers
		{
			switch (result.Type)
			{
				case ServiceResultType.NotFoundError:
                    return NotFound();
				case ServiceResultType.InternalError:
					return Problem(title: result.Description);
				case ServiceResultType.ConflictError:
					return Conflict();
				case ServiceResultType.ValidationError:
					return ValidationProblem(title: result.Description);
				case ServiceResultType.UnauthorizedError:
					return Unauthorized();
				default:

					return Problem();
			}
		}
	}
}

