using System;
using System.Collections.Generic;

namespace Authentication.Infrastructure
{
    public interface IAuthenticationEventRepository
    {
        void Store(AuthenticationEvent authenticationEvent);
        IList<AuthenticationEvent> All(Guid aggregateId);

    }
}
