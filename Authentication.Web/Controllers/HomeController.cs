﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Authentication.Library;
using Authentication.Web.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Authentication.Web.Models;

namespace Authentication.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly UsersViewModelMapper _usersViewModelMapper;

        public HomeController(IAuthenticationService authenticationService, UsersViewModelMapper usersViewModelMapper)
        {
            _authenticationService = authenticationService;
            _usersViewModelMapper = usersViewModelMapper;
        }
        public IActionResult Index()
        {
            var lastEvents = _authenticationService.GetLastEvents(20).ToList();
            var lastUsers = _authenticationService.GetLastUpdatedUsers(10);

            var viewModel = _usersViewModelMapper.Map(lastEvents, lastUsers);

            return View(viewModel);
        }
    }
}
