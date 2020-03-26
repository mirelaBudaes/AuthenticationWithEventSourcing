using System;
using System.Collections.Generic;
using System.Linq;

namespace Authentication.EventStore.Data
{
    internal class MemoryEventDb : IEventStore
    {
        private readonly IList<LoggedEvent> _events;

        public MemoryEventDb()
        {
            _events = new List<LoggedEvent>();
        }

        public void Save(LoggedEvent newEvent)
        {
            newEvent.Id = Guid.NewGuid();

            _events.Add(newEvent);
        }

        public List<LoggedEvent> GetAll()
        {
            return _events
                .ToList();
        }

        public List<LoggedEvent> GetAll(int topX)
        {
            throw new NotImplementedException();
        }

        public List<LoggedEvent> GetAll(Guid aggregateId)
        {
            return _events
                .Where(x => x.AggregateId == aggregateId )
                .ToList();
        }
    }
}
