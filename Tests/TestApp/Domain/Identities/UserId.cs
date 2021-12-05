using ITI.DDD.Domain;

namespace TestApp.Domain.Identities
{
    public record UserId : Identity
    {
        public UserId() { }
        public UserId(Guid guid) : base(guid) { }
        public UserId(Guid? guid) : base(guid ?? Guid.Empty) { }
    }
}
