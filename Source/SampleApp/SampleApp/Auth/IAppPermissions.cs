namespace SampleApp.Auth
{
    public interface IAppPermissions
    {
        bool CanViewFooSummary { get; }
        bool CanManageFoos { get; }
    }
}