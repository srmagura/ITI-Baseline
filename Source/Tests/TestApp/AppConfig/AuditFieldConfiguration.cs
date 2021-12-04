using ITI.Baseline.Audit;
using TestApp.Domain;

namespace TestApp.AppConfig
{
    public class AuditFieldConfiguration : IAuditFieldConfiguration
    {
        public Dictionary<string, List<string>> IgnoredFields =>
            new Dictionary<string, List<string>>
            {
                [nameof(Customer)] = new List<string> { nameof(Customer.SomeNumber) }
            };

        public Dictionary<string, List<string>> MaskedFields =>
            new Dictionary<string, List<string>>
            {
                [nameof(Customer)] = new List<string> { nameof(Customer.SomeMoney) }
            };
    }
}
