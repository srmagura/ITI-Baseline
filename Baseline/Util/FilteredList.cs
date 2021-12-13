namespace ITI.Baseline.Util;

public class FilteredList<T>
{
    public FilteredList(T[] items, int totalFilteredCount)
    {
        Items = items;
        TotalFilteredCount = totalFilteredCount;
    }

    public T[] Items { get; }
    public int TotalFilteredCount { get; }
}
