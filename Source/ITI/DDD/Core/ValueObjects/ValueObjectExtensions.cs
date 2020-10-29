namespace ITI.DDD.Domain.ValueObjects
{
    public static class ValueObjectExtensions
    {
        public static TVo NullIfNoValue<TVo>(this TVo vobj)
            where TVo : IValueObject
        {
            if (vobj == null)
                return default(TVo);
            return vobj.HasValue() ? vobj : default(TVo);
        }


    }
}