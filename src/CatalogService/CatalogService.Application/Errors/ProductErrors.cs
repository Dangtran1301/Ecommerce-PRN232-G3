using SharedKernel.Application.Common.Enums;
using SharedKernel.Domain.Common.Results;

namespace CatalogService.API.Errors
{
    public static class ProductErrors
    {
        public static Error NotFound(Guid id) =>
            Error.NotFound($"Product not found with ID: {id}");

        public static Error NameTaken(string name) =>
            Error.Conflict($"Product name already exists: {name}");

        public static Error InvalidData(string? message = null) =>
            Error.Failure(ErrorCodes.BadRequest, message ?? "Invalid product data");

        public static Error OutOfStock(Guid id) =>
            Error.Failure(ErrorCodes.BadRequest, $"Product with ID: {id} is out of stock");

        public static Error MissingRelations(string relation) =>
            Error.Failure(ErrorCodes.BadRequest, $"Missing required relation: {relation}");

        public static Error CategoryNotFound(string relation) =>
            Error.NotFound("Please select a category");
    }
}