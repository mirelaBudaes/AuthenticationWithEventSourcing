﻿using Authentication.Library;
using Authentication.Web.Mappers;
using Authentication.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Authentication.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly UsersViewModelMapper _usersViewModelMapper;

        public UserController(IAuthenticationService authenticationService, UsersViewModelMapper usersViewModelMapper)
        {
            _authenticationService = authenticationService;
            _usersViewModelMapper = usersViewModelMapper;
        }
        public IActionResult Index(string userId)
        {
            Guid userGuid;
            Guid.TryParse(userId, out userGuid);

            var viewModel = new UserViewModel();

            if (userGuid != Guid.Empty)
            {
                var events = _authenticationService.GetHistoryForUserId(userGuid);
                var storedUser = _authenticationService.GetStoredUser(userGuid);

                viewModel.History = events.Select(_usersViewModelMapper.Map).ToList();
                viewModel.StoredUser = _usersViewModelMapper.Map(storedUser);
            }

            return View(viewModel);
        }
    }
}