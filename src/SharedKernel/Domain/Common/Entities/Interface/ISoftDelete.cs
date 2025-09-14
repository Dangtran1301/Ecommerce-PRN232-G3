namespace SharedKernel.Domain.Common.Entities.Interface;

public interface ISoftDelete
{
    bool IsDeleted { get; }
    DateTime? DeletedAt { get; }
    string? DeletedBy { get; }

    void MarkAsDeleted(string? user = null);
    void Restore();
}