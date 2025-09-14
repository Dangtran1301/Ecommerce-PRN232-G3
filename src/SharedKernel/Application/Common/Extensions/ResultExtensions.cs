using Microsoft.AspNetCore.Mvc;
using SharedKernel.Domain.Common.Results;

namespace SharedKernel.Application.Common.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        var response = ApiResponse<T>.FromResult(result);

        return result.IsSuccess ? new OkObjectResult(response) : MapErrorToActionResult(response, result.Error!);
    }

    public static IActionResult ToActionResult(this Result result)
    {
        if (result.IsSuccess)
        {
            var successResponse = ApiResponse<object>.FromResult(Result<object>.Success(null!));
            return new OkObjectResult(successResponse);
        }

        var errorResult = Result<object>.Failure(result.Error!);
        var response = ApiResponse<object>.FromResult(errorResult);
        return MapErrorToActionResult(response, result.Error!);
    }

    private static IActionResult MapErrorToActionResult<T>(ApiResponse<T> response, Error error)
    {
        return error.Code switch
        {
            "NOT_FOUND" => new NotFoundObjectResult(response),
            "UNAUTHORIZED" => new UnauthorizedObjectResult(response),
            "FORBIDDEN" => new ForbidResult(),
            "CONFLICT" => new ConflictObjectResult(response),
            _ => new BadRequestObjectResult(response)
        };
    }
}