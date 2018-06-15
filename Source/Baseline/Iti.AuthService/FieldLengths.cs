namespace Iti.AuthService
{
    public static class FieldLengths
    {
        public class AuthUser
        {
            public const int Id = 128;
        }

        public class PasswordResetKey
        {
            public const int AuthUserId = AuthUser.Id;
            public const int Key = 128;
        }
    }
}