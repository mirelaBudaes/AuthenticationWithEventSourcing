using System;
using System.Collections.Generic;
using System.Linq;
using Authentication.Query;
using Authentication.Command;
using Authentication.EventStore;
using Authentication.EventStore.Data;
using Authentication.EventStore.Models;
using Authentication.Library.Exceptions;
using Authentication.SqlStore.Models;

namespace Authentication.Library
{
    public interface IAuthenticationService
    {
        void RegisterUser(string emailAddress);

        IList<LoggedEvent> GetLastEvents(int topX);

        IList<User> GetLastUpdatedUsers(int topX);
        User GetStoredUser(Guid userId);

        User GetStoredUser(string emailAddress);

        IList<LoggedEvent> GetHistoryForUserId(Guid userId);
    }

    internal class AuthenticationService : IAuthenticationService
    {
        private readonly IEventSourceManager _eventSourceManager;
        private readonly UserRepository _userRepository;
        private readonly IAuthenticationEventRepository _eventRepository;
        private readonly ILoggedEventRepository _loggedEventRepository;

        public AuthenticationService(IEventSourceManager eventSourceManager, UserRepository userRepository,
            IAuthenticationEventRepository eventRepository,
            ILoggedEventRepository loggedEventRepository)
        {
            _eventSourceManager = eventSourceManager;
            _userRepository = userRepository;
            _eventRepository = eventRepository;
            _loggedEventRepository = loggedEventRepository;
        }

        public void RegisterUser(string emailAddress)
        {
            if (_userRepository.UserExists(emailAddress))
            {
                //todo: create an event here also
                _eventSourceManager.Log(EventAction.EmailUniqueValidationFailed, emailAddress);

                throw new EmailExistsException();
            }

            _eventSourceManager.Log(EventAction.UserRegistered, emailAddress);


            //var registerUserCommand = new RegisterUserCommand();
        }

        public User GetStoredUser(Guid userId)
        {
            return _userRepository.GetUser(userId);
        }

        public User GetStoredUser(string emailAddress)
        {
            return _userRepository.GetUser(emailAddress);
        }

        public IList<LoggedEvent> GetHistoryForUserId(Guid userId)
        {
            return _loggedEventRepository.GetAll(userId)
                .OrderByDescending(x => x.TimeStamp)
                .ToList();
        }

        public IList<LoggedEvent> GetLastEvents(int topX)
        {
            return _loggedEventRepository.GetAll(topX)
                .OrderByDescending(x=> x.TimeStamp)
                .ToList();
        }

        public IList<User> GetLastUpdatedUsers(int topX)
        {
            return _userRepository.GetLastUpdatedUsers(topX);
        }
    }
}
