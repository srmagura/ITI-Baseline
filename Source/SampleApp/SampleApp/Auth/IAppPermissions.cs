namespace FooSampleApp.Auth
{
    public interface IAppPermissions
    {
        bool CanViewFooSummary { get; }
        bool CanManageFoos { get; }
    }
}