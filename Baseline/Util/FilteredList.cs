namespace ITI.Baseline.Util;

public class FilteredList<T>
{
    public FilteredList(List<T> items, int totalFilteredCount)
    {
        Items = items;
        TotalFilteredCount = totalFilteredCount;
    }

    public List<T> Items { get; }
    public int TotalFilteredCount { get; }
}
