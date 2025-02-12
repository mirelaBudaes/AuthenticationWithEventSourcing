﻿using System;
using System.Collections.Generic;
using System.Linq;
using Authentication.Query;
using Authentication.Command;
using Authentication.EventStore.Data;
using Authentication.EventStore.Models;
using Authentication.Library.Exceptions;
using Authentication.SqlStore.Models;

namespace Authentication.Library
{
    public interface IAuthenticationService
    {
        void RegisterUser(string emailAddress);

        void RequestChangeEmail(Guid userId, string newEmailAddress);

        void VerifyEmailAddress(Guid userId, string newEmailAddress);

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
        private readonly ILoggedEventRepository _loggedEventRepository;

        public AuthenticationService(IEventSourceManager eventSourceManager, UserRepository userRepository,
            ILoggedEventRepository loggedEventRepository)
        {
            _eventSourceManager = eventSourceManager;
            _userRepository = userRepository;
            _loggedEventRepository = loggedEventRepository;
        }

        public void RegisterUser(string emailAddress)
        {
            if (_userRepository.UserExists(emailAddress))
            {
                _eventSourceManager.Log(EventAction.EmailUniqueValidationFailed, emailAddress);

                throw new EmailExistsException();
            }

            _eventSourceManager.Log(EventAction.UserRegistered, emailAddress);

            //var registerUserCommand = new RegisterUserCommand();
        }

        public void RequestChangeEmail(Guid userId, string newEmailAddress)
        {
            if (_userRepository.UserExists(newEmailAddress))
            {
                _eventSourceManager.Log(EventAction.EmailUniqueValidationFailed, newEmailAddress);

                throw new EmailExistsException();
            }

            _eventSourceManager.Log(EventAction.EmailChangeRequested, newEmailAddress, userId);


        }

        public void VerifyEmailAddress(Guid userId, string newEmailAddress)
        {
            _eventSourceManager.Log(EventAction.EmailVerified, newEmailAddress, userId);
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
                .OrderByDescending(x => x.TimeStamp)
                .ToList();
        }

        public IList<User> GetLastUpdatedUsers(int topX)
        {
            return _userRepository.GetLastUpdatedUsers(topX);
        }
    }
}
