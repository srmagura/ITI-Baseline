namespace ITI.Baseline.Util;

public class PagedList<T>
{
    public PagedList(List<T> items, int totalFilteredCount)
    {
        Items = items;
        TotalFilteredCount = totalFilteredCount;
    }

    public int TotalFilteredCount { get; protected set; }
    public List<T> Items { get; } = new List<T>();
}
