using System;
using System.Collections.Generic;
using Authentication.EventStore.Models;

namespace Authentication.EventStore
{
    public interface IAuthenticationEventRepository
    {
        void Store(AuthenticationEvent authenticationEvent);
        IList<AuthenticationEvent> All(Guid aggregateId);

    }
}
