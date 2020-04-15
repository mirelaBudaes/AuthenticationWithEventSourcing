using System;
using System.Collections.Generic;
using System.Linq;
using Authentication.EventStore;
using Authentication.EventStore.Events;
using Authentication.EventStore.Models;
using Authentication.Infrastructure;
using Authentication.SqlStore.Models;

namespace Authentication.Command
{
    public interface IEventSourceManager
    {
        void Log(EventAction whatHappened, string emailAddresss);
        void Log(EventAction whatHappened, string emailAddresss, Guid userId);
    }

    internal class EventSourceManager : IEventSourceManager
    {
        private readonly IAuthenticationEventRepository _eventRepository;
        private readonly UserSyncronizer _userSyncronizer;

        public EventSourceManager(IAuthenticationEventRepository eventRepository,
            UserSyncronizer userSyncronizer)
        {
            _eventRepository = eventRepository;
            _userSyncronizer = userSyncronizer;
        }

        public void Log(EventAction whatHappened, string emailAddresss)
        {
            Log(whatHappened, emailAddresss, Guid.NewGuid());

        }
        
        public void Log(EventAction whatHappened, string emailAddresss, Guid userId)
        {
            var authenticationEvent = EventBuilder.New(whatHappened, emailAddresss, userId);
            _eventRepository.Store(authenticationEvent);

            //// Sync up with read model
            if (whatHappened != EventAction.EmailUniqueValidationFailed
            && whatHappened != EventAction.EmailChangeRequested)
            {
                var user = _userSyncronizer.Replay(userId);
                _userSyncronizer.Save(user);
            }
        }
    }
}
