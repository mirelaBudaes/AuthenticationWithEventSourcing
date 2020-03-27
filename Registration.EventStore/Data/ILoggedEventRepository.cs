using System;
using System.Collections.Generic;
using Authentication.EventStore.Models;

namespace Authentication.EventStore.Data
{
    public interface ILoggedEventRepository
    {
        List<LoggedEvent> GetAll();

        List<LoggedEvent> GetAll(int topX);

        List<LoggedEvent> GetAll(Guid aggregateId);

        void Save(LoggedEvent newEvent);
    }
}