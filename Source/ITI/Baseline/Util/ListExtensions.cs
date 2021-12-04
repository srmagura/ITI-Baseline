namespace ITI.Baseline.Util
{
    public static class ListExtensions
    {
        public static bool HasItems<T>(this List<T>? list)
        {
            return list != null && list.Count > 0;
        }
    }
}
