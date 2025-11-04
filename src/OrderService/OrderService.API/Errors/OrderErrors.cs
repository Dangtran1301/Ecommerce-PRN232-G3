using SharedKernel.Application.Common.Enums;
using SharedKernel.Domain.Common.Results;

namespace OrderService.API.Errors
{
    public static class OrderErrors
    {
        public static Error NotFound(Guid id) =>
            Error.NotFound($"Order not found with ID: {id}");

        public static Error InvalidData(string? message = null) =>
            Error.Failure(ErrorCodes.BadRequest, message ?? "Invalid order data");

        public static Error CustomerNotFound(Guid customerId) =>
            Error.Failure(ErrorCodes.BadRequest, $"Customer not found with ID: {customerId}");

        public static Error EmptyOrderItems() =>
            Error.Failure(ErrorCodes.BadRequest, "Order must contain at least one order item");

        public static Error InvalidStatusTransition(string fromStatus, string toStatus) =>
            Error.Failure(ErrorCodes.BadRequest, $"Cannot change order status from {fromStatus} to {toStatus}");

        public static Error PaymentFailed(Guid orderId) =>
            Error.Failure(ErrorCodes.InternalServerError, $"Payment failed for order ID: {orderId}");

        public static Error ProductUnavailable(Guid productId) =>
            Error.Failure(ErrorCodes.BadRequest, $"Product with ID: {productId} is unavailable for ordering");
    }
}