namespace SharedKernel.Domain.Common.Entities.Interface;

public interface IAuditable
{
    DateTime CreatedAt { get; }
    string? CreatedBy { get; }

    DateTime? UpdatedAt { get; }
    string? UpdatedBy { get; }

    void SetCreated(string? user = null);
    void SetUpdated(string? user = null);
}