using System;
using Iti.Core.DomainEvents;
using Iti.Core.UnitOfWork;
using Iti.Email;
using Iti.Logging;

namespace Iti.Authentication
{
    public class PasswordResetKeyCreatedHandler : IDomainEventHandler<PasswordResetKeyCreated>
    {
        private readonly IEmailSender _emailer;
        private readonly IAuthenticationRepository _repo;

        // TODO:JT:XXX: test that this will actually work!!!
        public PasswordResetKeyCreatedHandler(IEmailSender emailer, IAuthenticationRepository repo)
        {
            _emailer = emailer;
            _repo = repo;
        }

        public void Handle(PasswordResetKeyCreated ev)
        {
            using (var uow = UnitOfWork.Begin())
            {
                var subject = "Password Reset Request";

                var body = "TODO:JT:XXX: URL LINK";

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