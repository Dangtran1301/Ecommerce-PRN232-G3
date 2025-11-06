using SharedKernel.Application.Common.Enums;
using SharedKernel.Domain.Common.Results;

namespace CatalogService.API.Errors
{
    public static class ProductVariantErrors
    {
        public static Error NotFound(Guid id) =>
            Error.NotFound($"Product variant not found with ID: {id}");

        public static Error NameTaken(string name) =>
            Error.Conflict($"Product variant name already exists: {name}");

        public static Error InvalidData(string? message = null) =>
            Error.Failure(ErrorCodes.BadRequest, message ?? "Invalid product variant data");

        public static Error InvalidPrice(decimal price) =>
            Error.Validation($"Invalid variant price: {price}");
    }
}