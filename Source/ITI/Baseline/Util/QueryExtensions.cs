using System.Linq.Expressions;

namespace ITI.Baseline.Util
{
    public static class QueryExtensions
    {
        public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(
            this IQueryable<TSource> source, 
            Expression<Func<TSource, TKey>> keySelector, 
            bool descending
        )
        {
            return descending 
                ? source.OrderByDescending(keySelector)
                : source.OrderBy(keySelector);
        }

        public static IOrderedQueryable<TSource> ThenBy<TSource, TKey>(
            this IOrderedQueryable<TSource> source,
            Expression<Func<TSource, TKey>> keySelector,
            bool descending
        )
        {
            return descending 
                ? source.ThenByDescending(keySelector)
                : source.ThenBy(keySelector);
        }
    }
}
