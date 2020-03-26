using System;
using System.Collections.Generic;
using Authentication.EventStore.Models;

namespace Authentication.EventStore
{
    internal class AuthenticationEventRepository : IAuthenticationEventRepository
    {
        private readonly IEventStore _eventDb;
        public AuthenticationEventRepository(IEventStore eventDb)
        {
            _eventDb = eventDb;
        }

        public IList<AuthenticationEvent> All()
        {
            return _eventDb.GetAll();
        }

        public IList<AuthenticationEvent> GetLastEvents(int topX)
        {
            return _eventDb.GetAll(topX);
        }

        public IList<AuthenticationEvent> All(Guid aggregateId)
        {
            return _eventDb.GetAll(aggregateId);
        }

        public void Store(AuthenticationEvent authenticationEvent)
        {
            authenticationEvent.TimeStamp = DateTime.Now;
            
            _eventDb.Save(authenticationEvent);
        }
    }
}
