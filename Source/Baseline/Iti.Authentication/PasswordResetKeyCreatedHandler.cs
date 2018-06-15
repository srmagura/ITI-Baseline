using System;
using Iti.Core.DomainEvents;
using Iti.Core.UnitOfWork;
using Iti.Email;
using Iti.Logging;

namespace Iti.Authentication
{
    public class PasswordResetKeyCreatedHandler : IDomainEventHandler<PasswordResetKeyCreatedEvent>
    {
        private readonly IEmailSender _emailer;
        private readonly IAuthenticationRepository _repo;
        private readonly IAuthenticationUrlResolver _urlBuilder;

        public PasswordResetKeyCreatedHandler(IEmailSender emailer, IAuthenticationRepository repo, IAuthenticationUrlResolver urlBuilder)
        {
            _emailer = emailer;
            _repo = repo;
            _urlBuilder = urlBuilder;
        }

        public void Handle(PasswordResetKeyCreatedEvent ev)
        {
            using (var uow = UnitOfWork.Begin())
            {
                var subject = "Password Reset Request";

                var url = _urlBuilder.PasswordResetUrl(ev.Key);
                var body = $"Click here to reset your password: {url}";

                var pwrk = _repo.GetPasswordResetKey(ev.Key);
                if (pwrk == null)
                {
                    Log.Warning($"Could not send password reset key email: invalid key [{ev.Key}] for pwrk:{ev.PasswordResetKeyId}");
                    return;
                }

                if (!pwrk.Email.HasValue())
                {
                    Log.Warning($"");
                    return;
                }

                _emailer.Send(pwrk.Email?.Value, subject, body);

                uow.Commit();
            }
        }
    }
}