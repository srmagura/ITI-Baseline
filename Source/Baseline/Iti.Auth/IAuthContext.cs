namespace Iti.Auth
{
    public interface IAuthContext
    {
        bool IsAuthenticated { get; }
        string UserId { get; }
        string UserName { get; }

        //

        object AuthContextData { get; }
        void SetAuthContextData(object data);
    }
}
