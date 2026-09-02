namespace Infrastructure.Responding
{
    public record PagedResponse<T>(
        IEnumerable<T> Items,
        int TotalCount,
        int Page = 1,
        int PageSize = 10);
}