namespace Iti.Core.Sequences
{
    public interface ISequenceResolver
    {
        long GetNextValue(string name);
    }
}