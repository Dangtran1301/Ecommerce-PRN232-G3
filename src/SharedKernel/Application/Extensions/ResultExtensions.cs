using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Application.Common;
using SharedKernel.Application.Common.Enums;
using SharedKernel.Domain.Common.Results;

namespace SharedKernel.Application.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        ApiResponse<T> response = result;

        return result.IsSuccess
            ? new OkObjectResult(response)
            : MapErrorToActionResult(response, result.Error!);
    }

    public static IActionResult ToActionResult(this Result result)
    {
        ApiResponse response = result;

        return result.IsSuccess
            ? new OkObjectResult(response)
            : MapErrorToActionResult(response, result.Error!);
    }

    private static IActionResult MapErrorToActionResult(object response, Error error)
    {
        return error.Code switch
        {
            ErrorCodes.NotFound => new NotFoundObjectResult(response),
            ErrorCodes.Unauthorized => new UnauthorizedObjectResult(response),
            ErrorCodes.Forbidden => new ForbidResult(),
            ErrorCodes.Conflict => new ConflictObjectResult(response),
            ErrorCodes.BadRequest => new BadRequestObjectResult(response),

            _ => new ObjectResult(response)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            }
        };
    }
}