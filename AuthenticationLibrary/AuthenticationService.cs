using System;
using System.Collections.Generic;
using System.Linq;
using Authentication.Query;
using Authentication.Command;
using Authentication.EventStore.Models;
using Authentication.SqlStore.Models;

namespace Authentication.Library
{
    public interface IAuthenticationService
    {
        void RegisterUser(string emailAddress);

        IList<AuthenticationEvent> GetLastEvents(int topX);

        IList<User> GetLastUpdatedUsers(int topX);
    }

    internal class AuthenticationService : IAuthenticationService
    {
        private readonly IEventSourceManager _eventSourceManager;
        private readonly UserRepository _userRepository;

        public AuthenticationService(IEventSourceManager eventSourceManager, UserRepository userRepository)
        {
            _eventSourceManager = eventSourceManager;
            _userRepository = userRepository;
        }

        public void RegisterUser(string emailAddress)
        {
            if (_userRepository.UserExists(emailAddress))
            {
                //todo: create an event here also
                _eventSourceManager.Log(EventAction.EmailUniqueValidationFailed, emailAddress);

                throw new Exception($"User {emailAddress} already exists");
            }

            _eventSourceManager.Log(EventAction.UserRegistered, emailAddress);


            //var registerUserCommand = new RegisterUserCommand();
        }

        public IList<AuthenticationEvent> GetLastEvents(int topX)
        {
            return _eventSourceManager.GetEvents(topX)
                .OrderByDescending(x=> x.TimeStamp)
                .ToList();
        }

        public IList<User> GetLastUpdatedUsers(int topX)
        {
            return _userRepository.GetLastUpdatedUsers(topX);
        }
    }
}
