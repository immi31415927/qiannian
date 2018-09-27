namespace EC.Libraries.Core.Pager
{
    public interface IPagedList
    {
        int CurrentPageIndex { get; set; }
        int PageSize { get; set; }
        int TotalCount { get; set; }
    }
}
