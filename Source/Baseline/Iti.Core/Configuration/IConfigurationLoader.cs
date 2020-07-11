namespace Iti.Baseline.Core.Configuration
{
    public interface IConfigurationLoader
    {
        T GetSettings<T>();
    }
}