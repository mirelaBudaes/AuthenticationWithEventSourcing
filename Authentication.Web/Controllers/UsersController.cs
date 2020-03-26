using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authentication.Library;
using Authentication.Web.Mappers;
using Authentication.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly UsersViewModelMapper _usersViewModelMapper;

        public UsersController(IAuthenticationService authenticationService, UsersViewModelMapper usersViewModelMapper)
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