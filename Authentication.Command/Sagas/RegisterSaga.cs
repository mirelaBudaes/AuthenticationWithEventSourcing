using System;
using System.Collections.Generic;
using System.Text;
using Authentication.Command.Commands;
using Authentication.Command.Handlers;
using Authentication.EventStore.Events;
using Authentication.EventStore.Models;
using Authentication.Infrastructure;

namespace Authentication.Command.Sagas
{
    public class RegisterSaga : Saga,
        IStartWithMessage<RegisterUserCommand>
    {
        private readonly UserSyncronizer _userSyncronizer;

        public RegisterSaga(IBus bus, IAuthenticationEventRepository eventStore,
             UserSyncronizer userSyncronizer) : base(bus, eventStore)
        {
            _userSyncronizer = userSyncronizer;
        }

        public void Handle(RegisterUserCommand message)
        {
            //save the event
            var userId = Guid.NewGuid();
            var whatHappened = (EventAction)Enum.Parse(typeof(EventAction), message.EventActionName);
            var userRegisteredEvent = EventBuilder.New(whatHappened, message.EmailAddresss, userId);
           
           // EventStore.Store(authenticationEvent);

            //save user in the sql DB
            var user = _userSyncronizer.Replay(userId);
            _userSyncronizer.Save(user);

            //raise the event
            Bus.RaiseEvent(userRegisteredEvent);
        }
    }
}
