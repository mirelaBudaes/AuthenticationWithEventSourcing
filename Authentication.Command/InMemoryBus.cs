using System;
using System.Collections.Generic;
using System.Linq;
using Authentication.EventStore;
using Authentication.Infrastructure;

namespace Authentication.Command
{
    public class InMemoryBus : IBus
    {
        private readonly IAuthenticationEventRepository _authenticationEventRepository;
        private readonly UserSyncronizer _userSyncronizer;

        private static readonly IList<Type> RegisteredHandlers = new List<Type>();
        private static readonly IDictionary<Type, Type> RegisteredSagas = new Dictionary<Type, Type>();

        public InMemoryBus(IAuthenticationEventRepository authenticationEventRepository,
            UserSyncronizer userSyncronizer)
        {
            _authenticationEventRepository = authenticationEventRepository;
            _userSyncronizer = userSyncronizer;
        }
        public void Send<T>(T command) where T : Infrastructure.Command
        {
            SendInternal<T>(command);
        }

        public void RaiseEvent<T>(T theEvent) where T : AuthenticationEvent
        {
            //if (_authenticationEventRepository != null)
            //    _authenticationEventRepository.Store(theEvent);
            SendInternal(theEvent);
        }

        private void SendInternal<T>(T message) where T : Message
        {
            LaunchSagasThatStartWithMessage(message);
            DeliverMessageToRunningSagas(message);
            DeliverMessageToRegisteredHandlers(message);

            // Saga and handlers are similar things. Handlers are  one-off event handlers
            // whereas saga may be persisted and survive sessions, wait for more messages and so forth.
            // Saga are mostly complex workflows; handlers are plain one-off event handlers.
        }

        private void LaunchSagasThatStartWithMessage<T>(T message) where T : Message
        {
            var messageType = message.GetType();
            var openInterface = typeof(IStartWithMessage<>);
            var closedInterface = openInterface.MakeGenericType(messageType);
            var sagasToLaunch = from s in RegisteredSagas.Values
                where closedInterface.IsAssignableFrom(s)
                select s;
            foreach (var s in sagasToLaunch)
            {
                dynamic sagaInstance = Activator.CreateInstance(s, this, _authenticationEventRepository, _userSyncronizer);
                sagaInstance.Handle(message);
            }
        }

        private void DeliverMessageToRunningSagas<T>(T message) where T : Message
        {
            var messageType = message.GetType();
            var openInterface = typeof(IHandleMessage<>);
            var closedInterface = openInterface.MakeGenericType(messageType);
            var sagasToNotify = from s in RegisteredSagas.Values
                where closedInterface.IsAssignableFrom(s)
                select s;
            foreach (var s in sagasToNotify)
            {
                dynamic sagaInstance = Activator.CreateInstance(s, this, _authenticationEventRepository, _userSyncronizer);
                sagaInstance.Handle(message);
            }
        }

        private void DeliverMessageToRegisteredHandlers<T>(T message) where T : Message
        {
            var messageType = message.GetType();
            var openInterface = typeof(IHandleMessage<>);
            var closedInterface = openInterface.MakeGenericType(messageType);
            var handlersToNotify = from h in RegisteredHandlers
                where closedInterface.IsAssignableFrom(h)
                select h;
            foreach (var h in handlersToNotify)
            {
                dynamic sagaInstance = Activator.CreateInstance(h, _authenticationEventRepository);     // default ctor is enough
                sagaInstance.Handle(message);
            }
        }

        public void RegisterHandler<T>()
        {
            RegisteredHandlers.Add(typeof(T));
        }

        public void RegisterSaga<T>() where T:Saga
        {
            var sagaType = typeof(T);
            if (sagaType.GetInterfaces().Count(i => i.Name.StartsWith(typeof(IStartWithMessage<>).Name)) != 1)
            {
                throw new InvalidOperationException("The specified saga must implement the IStartWithMessage<T> interface.");
            }
            var messageType = sagaType.
                GetInterfaces().First(i => i.Name.StartsWith(typeof(IStartWithMessage<>).Name)).
                GenericTypeArguments.
                First();
            RegisteredSagas.Add(messageType, sagaType);
        }
    }
}