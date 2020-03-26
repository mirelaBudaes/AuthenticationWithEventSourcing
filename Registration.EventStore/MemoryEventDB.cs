using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Authentication.EventStore.Models;

namespace Authentication.EventStore
{
    internal class MemoryEventDb : IEventStore
    {
        private readonly IList<AuthenticationEvent> _events;

        public MemoryEventDb()
        {
            _events = new List<AuthenticationEvent>();
        }

        public void Save(AuthenticationEvent newEvent)
        {
            newEvent.Id = Guid.NewGuid();

            _events.Add(newEvent);
        }

        public List<AuthenticationEvent> GetAll()
        {
            return _events
                .ToList();
        }

        public List<AuthenticationEvent> GetAll(int topX)
        {
            throw new NotImplementedException();
        }

        public List<AuthenticationEvent> GetAll(Guid aggregateId)
        {
            return _events
                .Where(x => x.UserId == aggregateId )
                .ToList();
        }
    }
}
