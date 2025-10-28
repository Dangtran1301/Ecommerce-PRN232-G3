using SharedKernel.Application.Common.Enums;
using SharedKernel.Domain.Common.Results;

namespace CatalogService.API.Errors
{
    public static class StockErrors
    {
        public static Error NotFound(Guid id) =>
            Error.NotFound($"Stock record not found with ID: {id}");

        public static Error InsufficientQuantity(Guid productId, int requested, int available) =>
            Error.Failure(ErrorCodes.BadRequest,
                $"Not enough stock for product {productId}. Requested: {requested}, Available: {available}");

        public static Error InvalidData(string? message = null) =>
            Error.Failure(ErrorCodes.BadRequest, message ?? "Invalid stock data");
        public static Error DuplicateProduct(Guid productId) =>
            Error.Conflict($"A stock record for product {productId} already exists");
    }
}
