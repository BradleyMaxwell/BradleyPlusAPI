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
                    return NotFound(result.Description);
				case ServiceResultType.InternalError:
					return Problem(result.Description);
				case ServiceResultType.ConflictError:
					return Conflict(result.Description);
				case ServiceResultType.ValidationError:
					return ValidationProblem(result.Description);
				case ServiceResultType.UnauthorizedError:
					return Unauthorized(result.Description);
				default:

					return Problem();
			}
		}
	}
}

