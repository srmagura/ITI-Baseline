namespace Iti.Core.ValueObjects
{
    public static class ValueObjectExtensions
    {
        public static TVo NullIfNoValue<TVo>(this TVo vobj)
            where TVo : ValueObject<TVo>
        {
            if (vobj == null)
                return null;
            return vobj.HasValue() ? vobj : null;
        }
    }
}