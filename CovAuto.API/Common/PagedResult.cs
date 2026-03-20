namespace CovAuto.API.Common;

/// <summary>
/// Wrapper voor gepagineerde resultaten. Bevat de items en paginatie-informatie.
/// </summary>
public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}