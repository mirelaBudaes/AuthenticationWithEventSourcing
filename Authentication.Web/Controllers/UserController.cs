using Authentication.Library;
using Authentication.Web.Mappers;
using Authentication.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Authentication.Library.Exceptions;

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

        [HttpPost]
        public IActionResult ChangeEmail(string emailAddress, string userId)
        {
            //todo: email address validation

            Guid userGuid;
            Guid.TryParse(userId, out userGuid);

            try
            {
                _authenticationService.RequestChangeEmail(userGuid, emailAddress);
            }
            catch (EmailExistsException)
            {
                ViewBag.ErrorMessage = $"User {emailAddress} already exists";

                return RedirectToAction("Index", new { userId = userId });
            }

            return RedirectToAction("Index", "User", new { userId = userId });
        }

        [HttpPost]
        public IActionResult VerifyEmailAddress(string emailAddress, string userId)
        {
            //todo: email address validation

            Guid userGuid;
            Guid.TryParse(userId, out userGuid);

            _authenticationService.VerifyEmailAddress(userGuid, emailAddress);

            return RedirectToAction("Index", "User", new { userId = userId });
        }
    }
}