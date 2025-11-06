using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SharedKernel.Application.Extensions;

public class UnauthorizedObjectResult : ObjectResult
{
    public UnauthorizedObjectResult(object? value) : base(value)
    {
        StatusCode = StatusCodes.Status401Unauthorized;
    }
}