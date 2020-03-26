using System;
using System.Collections.Generic;
using Authentication.EventStore.Models;

namespace Authentication.EventStore
{
    internal interface IEventStore
    {
        List<AuthenticationEvent> GetAll();

        List<AuthenticationEvent> GetAll(Guid aggregateId);

        void Save(AuthenticationEvent newEvent);
    }
}