using System;
using System.Collections.Generic;

namespace ITI.Baseline.Util
{
    public class FilteredList<T>
    {
        [Obsolete("Serialization Only", true)]
        public FilteredList()
        {
        }

        public FilteredList(int totalFilteredCount, List<T> items)
        {
            TotalFilteredCount = totalFilteredCount;
            Items = items;
        }

        //

        public int TotalFilteredCount { get; set; }
        public List<T> Items { get; set; } = new List<T>();

        public bool HasItems()
        {
            return Items != null && Items.Count > 0;
        }
    }
}
