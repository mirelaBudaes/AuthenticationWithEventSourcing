using System;
using System.Collections.Generic;
using Authentication.EventStore.Models;

namespace Authentication.EventStore.Data
{
    internal interface IEventStore
    {
        List<LoggedEvent> GetAll();

        List<LoggedEvent> GetAll(int topX);

        List<LoggedEvent> GetAll(Guid aggregateId);

        void Save(LoggedEvent newEvent);
    }
}