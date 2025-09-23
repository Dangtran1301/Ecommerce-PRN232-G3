namespace SharedKernel.Application.Common;

public enum FilterOperator
{
    Equals,
    NotEquals,
    Contains,
    StartsWith,
    EndsWith,
    GreaterThan,
    LessThan,
    GreaterOrEqual,
    LessOrEqual,
    In
}

public class FilterDescriptor
{
    public string Field { get; set; } = string.Empty;
    public FilterOperator Operator { get; set; } = FilterOperator.Equals;
    public object? Value { get; set; }
}

public class DynamicQuery
{
    public List<FilterDescriptor> Filters { get; set; } = new();
    public string? SortBy { get; set; }
    public bool Descending { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public int Skip => (PageNumber - 1) * PageSize;
    public int Take => PageSize;
}