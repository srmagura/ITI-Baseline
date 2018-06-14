namespace Iti.Core.Configuration
{
    public interface IConfigurationLoader
    {
        T GetSettings<T>();
    }
}